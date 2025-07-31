using SVM.Core.Data;
using SVM.Core.FuncImpl;
using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using static SVM.Core.stdc.stdlib;
namespace SVM.Core
{
	/// <summary>
	/// Memory Layout:
	/// <br/>
	/// Index 0 - Program.<br/>
	/// Index 1 - GPMemory:<br/>
	/// Offset	|Length			|Usage<br/>
	/// 0 |StackLength	|	Stack
	/// </summary>
	public unsafe class SimpleVirtualMachine : IDisposable
	{
		public Registers registers;
		public MemoryBlock Stack;
		public List<MemoryBlock> Memories = new List<MemoryBlock>();
		public SVMConfig? Config = null;
		public MState MachineState;
		public SVMProgram* Program = null;
		public static uint InitGPMemorySize = 696320;
		public Object? AdditionalData = null;
		public ErrorIDs ErrorIDs = new ErrorIDs();
		public void Init(uint StackSize = 1024 * 1024, uint RegisterSize = 512, uint GPMemory = uint.MaxValue)
		{
			registers.Init(RegisterSize);

			uint SPOffset = 2;
			if (Config != null)
			{
				SPOffset = Config.SPRegisterID;
			}
			if (GPMemory == 0)
			{

			}
			else
			{
				if (GPMemory == uint.MaxValue)
				{
					GPMemory = InitGPMemorySize;
				}
				MemoryBlock block = new MemoryBlock();
				block.Init(GPMemory);
				SetMemory(1, block);
				registers.SetData<SVMPointer>((int)SPOffset, new SVMPointer() { index = 1, offset = 0 });
			}
		}
		public bool isReachBinaryEnd()
		{
			uint SPOffset = 2;
			uint PCOffset = 1;
			uint ErrorIDOffset = 3;
			if (Config != null)
			{
				SPOffset = Config.SPRegisterID;
				PCOffset = Config.PCRegisterID;
				ErrorIDOffset = Config.EIDRegisterID;
			}
			var PC = registers.GetData<ulong>((int)PCOffset);
			Console.WriteLine($"{PC},{Program->InstructionCount}");
			return PC >= Program->InstructionCount;
		}

		public void Step()
		{
			uint SPOffset = 2;
			uint PCOffset = 1;
			uint ErrorIDOffset = 3;
			if (Config != null)
			{
				SPOffset = Config.SPRegisterID;
				PCOffset = Config.PCRegisterID;
				ErrorIDOffset = Config.EIDRegisterID;
			}
			if (Program == null) return;
			var PC = registers.GetData<ulong>((int)PCOffset);
			if (PC >= Program->InstructionCount) return;
			var currentInstPtr = GetPointer(PC);
			var Instruction = currentInstPtr.GetData<SVMInstruction>();
			var def = Instruction.GetDef();
			Console.WriteLine(def);
			fixed (MState* statePtr = &MachineState)
			{

				switch (def)
				{
					case PrimaryInstruction.BMath:
						{
							var Op = Instruction.GetData<BMathOp>(1);
							var NativeType = Instruction.GetData<SVMNativeTypes>(2);
							var L = Instruction.GetData<byte>(3);
							var R = Instruction.GetData<byte>(4);
							var T = Instruction.GetData<byte>(5);
							switch (Op)
							{
								case BMathOp.Add:
									MathImpl.MathAdd(registers, statePtr, NativeType, L, R, T, false);
									break;
								case BMathOp.Sub:
									MathImpl.MathSub(registers, statePtr, NativeType, L, R, T, false);
									break;
								case BMathOp.Mul:
									MathImpl.MathMul(registers, statePtr, NativeType, L, R, T, false);
									break;
								case BMathOp.Div:
									MathImpl.MathDiv(registers, statePtr, NativeType, L, R, T, false);
									break;
								case BMathOp.Mod:
									MathImpl.MathMod(registers, statePtr, NativeType, L, R, T);
									break;
								default:
									break;
							}
						}
						break;
					case PrimaryInstruction.CBMath:
						{
							var Op = Instruction.GetData<BMathOp>(1);
							var NativeType = Instruction.GetData<SVMNativeTypes>(2);
							var L = Instruction.GetData<byte>(3);
							var R = Instruction.GetData<byte>(4);
							var T = Instruction.GetData<byte>(5);
							switch (Op)
							{
								case BMathOp.Add:
									MathImpl.MathAdd(registers, statePtr, NativeType, L, R, T, true);
									break;
								case BMathOp.Sub:
									MathImpl.MathSub(registers, statePtr, NativeType, L, R, T, true);
									break;
								case BMathOp.Mul:
									MathImpl.MathMul(registers, statePtr, NativeType, L, R, T, true);
									break;
								case BMathOp.Div:
									MathImpl.MathDiv(registers, statePtr, NativeType, L, R, T, true);
									break;
								case BMathOp.Mod:
									MathImpl.MathMod(registers, statePtr, NativeType, L, R, T);
									break;
								default:
									break;
							}
						}
						break;
					case PrimaryInstruction.UMath:
						break;
					case PrimaryInstruction.Cvt:
						{
							Convert(Instruction);
						}
						break;
					case PrimaryInstruction.Cmp:
						break;
					case PrimaryInstruction.SD:
						{
							var Reg = Instruction.GetData<byte>(1);
							PC++;
							var dataPtr = GetPointer(PC);
							var data = dataPtr.GetData<int>();
							registers.SetDataInRegister(Reg, data);
							Console.WriteLine($"SVM:SD:{data} form PC={PC}");
						}
						break;
					case PrimaryInstruction.JAL:
						break;
					case PrimaryInstruction.JALF:
						break;
					case PrimaryInstruction.Load:
						break;
					case PrimaryInstruction.Save:
						break;
					case PrimaryInstruction.Call:
						break;
					case PrimaryInstruction.Return:
						break;
					case PrimaryInstruction.System:
						if (Config != null)
						{
							var target = Instruction.GetData<uint>(4);
							if (Config.FuncCalls.TryGetValue(target, out var func))
							{
								func(this);
							}
							else
							{
								registers.SetData((int)ErrorIDOffset,ErrorIDs.SyscallCallNotExist);
							}
						}
						else
						{
							registers.SetData((int)ErrorIDOffset, ErrorIDs.SyscallCallNotExist);
						}
						break;
					case PrimaryInstruction.SIMD:
						break;
					default:
						break;
				}
			}
			PC++;
			registers.SetData<ulong>((int)PCOffset, PC);
			PC = registers.GetData<ulong>((int)PCOffset);
		}

		private void Convert(SVMInstruction Instruction)
		{
			var SType = Instruction.GetData<SVMNativeTypes>(1);
			var TType = Instruction.GetData<SVMNativeTypes>(2);
			var L = Instruction.GetData<byte>(3);
			var T = Instruction.GetData<byte>(4);
			ICastable castable;
			switch (SType)
			{
				case SVMNativeTypes.Int8:
					castable = registers.GetData<CompactSByte>(L);
					break;
				case SVMNativeTypes.Int16:
					castable = registers.GetData<CompactShort>(L);
					break;
				case SVMNativeTypes.Int32:
					castable = registers.GetData<CompactInt>(L);
					break;
				case SVMNativeTypes.Int64:
					castable = registers.GetData<CompactLong>(L);
					break;
				case SVMNativeTypes.UInt8:
					castable = registers.GetData<CompactByte>(L);
					break;
				case SVMNativeTypes.UInt16:
					castable = registers.GetData<CompactUShort>(L);
					break;
				case SVMNativeTypes.UInt32:
					castable = registers.GetData<CompactUInt>(L);
					break;
				case SVMNativeTypes.UInt64:
					castable = registers.GetData<CompactULong>(L);
					break;
				case SVMNativeTypes.Float:
					castable = registers.GetData<CompactSingle>(L);
					break;
				case SVMNativeTypes.Double:
					castable = registers.GetData<CompactDouble>(L);
					break;
				default:
					return;
			}
			switch (TType)
			{
				case SVMNativeTypes.Int8:
					castable.Cast_SByte().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.Int16:
					castable.Cast_Short().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.Int32:
					castable.Cast_Int().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.Int64:
					castable.Cast_Long().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.UInt8:
					castable.Cast_Byte().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.UInt16:
					castable.Cast_UShort().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.UInt32:
					castable.Cast_UInt().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.UInt64:
					castable.Cast_ULong().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.Float:
					castable.Cast_Float().Write(registers.GetPtr(T));
					break;
				case SVMNativeTypes.Double:
					castable.Cast_Double().Write(registers.GetPtr(T));
					break;
				default:
					break;
			}
		}

		public IntPtr GetPointer(ulong PC)
		{
			return GetPointer(new SVMPointer() { offset = (uint)(PC * (uint)sizeof(SVMInstruction)), index = 0 });
		}
		public IntPtr GetPointer(SVMPointer absoluteAddress)
		{
			if (absoluteAddress.index == 0)
			{
				ulong offset0 = 0;
				ulong offset1 = 0;

				if (Program != null)
				{
					offset0 = Program->InstructionCount * (ulong)sizeof(SVMInstruction);
					offset1 = Program->DataSize;
					if (absoluteAddress.offset < offset0)
					{

						return (IntPtr)(Program->instructions + absoluteAddress.offset / sizeof(SVMInstruction));
					}
					else if (absoluteAddress.offset < offset1)
					{
						return IntPtr.Add((IntPtr)Program->instructions, (int)(absoluteAddress.offset - offset0));

					}
				}
			}
			var realIndex = absoluteAddress.index - 1;
			if (realIndex < Memories.Count)
			{
				return IntPtr.Add(Memories[(int)realIndex].StartAddress, (int)absoluteAddress.offset);

			}
			return IntPtr.Zero;
		}
		public MemoryBlock SetMemory(int id, MemoryBlock block)
		{
			var realID = id - 1;
			if (id < Memories.Count)
			{
				var old = Memories[(int)realID];
				Memories[(int)realID] = block;
				return old;
			}
			else
			{
				var count = realID - Memories.Count;
				for (int i = 0; i <= count; i++)
				{
					Memories.Add(default);
				}
				Memories[(int)realID] = block;
				return default;
			}
		}
		public void Dispose()
		{
			registers.Dispose();
			Stack.Dispose();
			foreach (var item in Memories)
			{
				item.Dispose();
			}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct SVMPointer
	{
		public uint offset;
		public uint index;
	}
	public class SVMConfig
	{
		public Dictionary<uint, FuncCall> FuncCalls = new Dictionary<uint, FuncCall>();
		public uint SPRegisterID;
		public uint PCRegisterID;
		/// <summary>
		/// Error ID Register.
		/// </summary>
		public uint EIDRegisterID;
	}
	public class ErrorIDs
	{
		public int SyscallCallNotExist = 1;
		public int MAX_ERROR_ID = short.MaxValue;
	}
	public delegate void FuncCall(SimpleVirtualMachine machine);
	[StructLayout(LayoutKind.Sequential)]
	public struct MState
	{
		public byte OF;
		public byte CF;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct MemoryBlock : IDisposable
	{
		public IntPtr StartAddress;
		public uint Size;
		public void Init(uint Size)
		{
			this.Size = Size;
			StartAddress = malloc(Size);
		}
		public void Dispose()
		{
			free(StartAddress);
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct Callframe
	{
		public ulong PC;
		public ulong SP;
	}
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SVMInstruction
	{
		public ulong data;
		public T GetData<T>(int offset) where T : unmanaged
		{
			fixed (ulong* dataPtr = &data)
			{
				return ((T*)(((byte*)dataPtr) + offset))[0];
			}
		}
		public PrimaryInstruction GetDef()
		{
			fixed (ulong* dataPtr = &data)
			{
				return ((PrimaryInstruction*)dataPtr)[0];
			}
		}
	}
}

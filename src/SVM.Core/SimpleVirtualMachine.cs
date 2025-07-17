using SVM.Core.FuncImpl;
using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
			fixed (MState* statePtr = &MachineState)
			{

				switch (def)
				{
					case SVMInstDef.BMath:
						{
							var Op = Instruction.GetData<BMathOp>(1);
							var NativeType = Instruction.GetData<SVMNativeTypes>(2);
							var L = Instruction.GetData<byte>(3);
							var R = Instruction.GetData<byte>(4);
							var T = Instruction.GetData<byte>(5);
							var Of = Instruction.GetData<byte>(6);
							switch (Op)
							{
								case BMathOp.Add:
									MathImpl.MathAdd(registers, statePtr, NativeType, L, R, T, Of == 1);
									break;
								case BMathOp.Sub:
									MathImpl.MathSub(registers, statePtr, NativeType, L, R, T, Of == 1);
									break;
								case BMathOp.Mul:
									MathImpl.MathMul(registers, statePtr, NativeType, L, R, T, Of == 1);
									break;
								case BMathOp.Div:
									MathImpl.MathDiv(registers, statePtr, NativeType, L, R, T, Of == 1);
									break;
								case BMathOp.Mod:
									MathImpl.MathMod(registers, statePtr, NativeType, L, R, T);
									break;
								default:
									break;
							}
						}
						break;
					case SVMInstDef.UMath:
						break;
					case SVMInstDef.Cvt:
						break;
					case SVMInstDef.Cmp:
						break;
					case SVMInstDef.SD:
						{
							var Reg = Instruction.GetData<byte>(1);
							PC++;
							var dataPtr = GetPointer(PC);
							var data = currentInstPtr.GetData<ulong>();
							registers.SetData(Reg, data);
						}
						break;
					case SVMInstDef.JAL:
						break;
					case SVMInstDef.JALF:
						break;
					case SVMInstDef.Load:
						break;
					case SVMInstDef.Save:
						break;
					case SVMInstDef.Call:
						break;
					case SVMInstDef.Return:
						break;
					case SVMInstDef.System:
						if (Config != null)
						{
							var target = Instruction.GetData<uint>(1);
							if (Config.FuncCalls.TryGetValue(target, out var func))
							{
								func(this);
							}
						}
						break;
					case SVMInstDef.SIMD:
						break;
					default:
						break;
				}
			}
			PC++;
			registers.SetData<ulong>((int)PCOffset, PC);
		}
		public IntPtr GetPointer(ulong PC)
		{
			return GetPointer(new SVMPointer() { offset = (uint)PC, index = 0 });
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

						return IntPtr.Add((IntPtr)Program->instructions, (int)absoluteAddress.offset);
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
	public unsafe struct SVMProgram
	{
		public UInt64 InstructionCount;
		public UInt64 DataSize;
		public SVMInstruction* instructions;
		public byte* data;
		public static SVMProgram* LoadFromStream(Stream stream)
		{
			var program = (SVMProgram*)malloc(sizeof(SVMProgram));

			return program;
		}
	}
	public delegate void FuncCall(SimpleVirtualMachine machine);
	[StructLayout(LayoutKind.Sequential)]
	public struct MState
	{
		public byte OF;
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
		public SVMInstDef GetDef()
		{
			fixed (ulong* dataPtr = &data)
			{
				return ((SVMInstDef*)dataPtr)[0];
			}
		}
	}
}

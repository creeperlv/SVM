using LibCLCC.NET.Operations;
using Microsoft.Win32.SafeHandles;
using SVM.Core;
using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace SVM.Assembler.Core
{
	public class Linker
	{
		public static OperationResult<IntermediateObject?> Link(List<IntermediateObject> objs)
		{
			OperationResult<IntermediateObject?> operationResult = new OperationResult<IntermediateObject?>(null);
			IntermediateObject intermediateObject = new IntermediateObject();
			foreach (var item in objs)
			{
				foreach (var inst in item.instructions)
				{
					intermediateObject.instructions.Add(inst);
				}
				foreach (var data in item.data)
				{
					if (!intermediateObject.data.TryAdd(data.Key, data.Value))
					{
						intermediateObject.data[data.Key] = data.Value;
					}
				}
				foreach (var kv in item.consts)
				{
					if (!intermediateObject.consts.TryAdd(kv.Key, kv.Value))
					{
						intermediateObject.consts[kv.Key] = kv.Value;
					}
				}
			}
			operationResult.Result = intermediateObject;
			return operationResult;
		}
		public static bool TryParseRegister(string input, LinkingContext context, out byte registerID)
		{
			if (input.StartsWith("$"))
			{
				var regValue = input[1..];
				if (context.Definition.RegisterNames.TryGetValue(regValue, out var regID))
				{
					registerID = regID;
					return true;
				}
				else
				{
					return byte.TryParse(regValue, out registerID);
				}
			}
			else
			{
				if (context.IntermediateObject.TryGetConst(input, out var realStr))
				{
					return TryParseRegister(realStr, context, out registerID);
				}
			}
			registerID = byte.MaxValue;
			return false;
		}
		public static bool TryParseInt32(string input, LinkingContext context, out int value)
		{
			if (int.TryParse(input, out value))
			{
				return true;
			}
			else
			{
				if (context.IntermediateObject.TryGetConst(input, out var realStr))
				{
					return TryParseInt32(realStr, context, out value);
				}
			}
			value = byte.MaxValue;
			return false;
		}
		public static bool TryParseInt64(string input, LinkingContext context, out long value)
		{
			if (long.TryParse(input, out value))
			{
				return true;
			}
			else
			{
				if (context.IntermediateObject.TryGetConst(input, out var realStr))
				{
					return TryParseInt64(realStr, context, out value);
				}
			}
			value = byte.MaxValue;
			return false;
		}
		public unsafe static void WriteData(SVMInstruction* inst, SVMNativeTypes nativeType, int Pos, byte* dataStart)
		{
			var size = nativeType switch
			{
				SVMNativeTypes.Int8 => sizeof(sbyte),
				SVMNativeTypes.Int16 => sizeof(short),
				SVMNativeTypes.Int32 => sizeof(int),
				SVMNativeTypes.Int64 => sizeof(long),
				SVMNativeTypes.UInt8 => sizeof(byte),
				SVMNativeTypes.UInt16 => sizeof(ushort),
				SVMNativeTypes.UInt32 => sizeof(uint),
				SVMNativeTypes.UInt64 => sizeof(ulong),
				SVMNativeTypes.Float => sizeof(float),
				SVMNativeTypes.Double => sizeof(double),
				_ => 0,
			};
			Buffer.MemoryCopy(dataStart, ((byte*)inst) + Pos * sizeof(byte), size, size);
		}
		public unsafe static bool ParseAndWriteData(SVMInstruction* inst, SVMNativeTypes nativeType, int Pos, string value)
		{
			switch (nativeType)
			{
				case SVMNativeTypes.Int8:
					{
						if (sbyte.TryParse(value, out sbyte v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.Int16:
					{
						if (short.TryParse(value, out short v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.Int32:
					{
						if (int.TryParse(value, out int v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.Int64:
					{
						if (long.TryParse(value, out long v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.UInt8:
					{
						if (byte.TryParse(value, out byte v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.UInt16:
					{
						if (ushort.TryParse(value, out ushort v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.UInt32:
					{
						if (uint.TryParse(value, out uint v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.UInt64:
					{
						if (ulong.TryParse(value, out ulong v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.Float:
					{
						if (float.TryParse(value, out float v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				case SVMNativeTypes.Double:
					{
						if (double.TryParse(value, out double v))
						{
							WriteData(inst, nativeType, Pos, (byte*)&v);
							return true;
						}
					}
					break;
				default:
					break;
			}
			return false;
		}
		public unsafe static bool ProcessDefinedEnum(string enumName, string input, InstructionParameter parameter, LinkingContext context, SVMInstruction* inst)
		{
			if (context.Definition.Enums.TryGetValue(enumName, out var enumDef))
			{
				if (enumDef.TryGetValue(input, out var enumValue))
				{
					return ParseAndWriteData(inst, parameter.ExpectdValue.Type, parameter.ExpectdValue.Pos, enumValue);
				}
				if (context.IntermediateObject.TryGetConst(input, out var constValue))
				{
					return ProcessDefinedEnum(enumName, constValue, parameter, context, inst);
				}
			}
			return false;
		}
		public unsafe static bool ProcessInternalEnum(string enumName, string input, InstructionParameter parameter, LinkingContext context, SVMInstruction* inst)
		{
			switch (enumName)
			{
				case "NativeType":
					{
						if (Enum.TryParse<SVMNativeTypes>(input, out var enumV))
						{
							WriteData(inst, parameter.ExpectdValue.Type, parameter.ExpectdValue.Pos, (byte*)&enumV);
							return true;
						}
					}
					break;
				case "bOp":
					{
						if (Enum.TryParse<BMathOp>(input, out var enumV))
						{
							WriteData(inst, parameter.ExpectdValue.Type, parameter.ExpectdValue.Pos, (byte*)&enumV);
							return true;
						}
					}
					break;
				default:
					break;
			}
			if (context.IntermediateObject.TryGetConst(input, out var value))
			{
				return ProcessInternalEnum(enumName, value, parameter, context, inst);
			}
			return false;
		}
		public unsafe static OperationResult<bool> translate(InstructionDefinition def, LinkingContext context, IntermediateInstruction iinstruction, SVMInstruction* instruction)
		{
			OperationResult<bool> result = new OperationResult<bool>(false);
			//SVMInstruction instruction = new SVMInstruction();
			for (int i = 0; i < iinstruction.Parameters.Count; i++)
			{
				var para = iinstruction.Parameters[i];
				var paraDef = def.ParameterPattern[i];
				string converter = paraDef.ExpectdValue.Converter;
				if (para.Content == null)
				{
					return result;
				}
				if (converter.StartsWith("InternalEnum:"))
				{
					var enumName = converter["InternalEnum:".Length..];
					ProcessInternalEnum(enumName, para.Content, paraDef, context, instruction);
				}
				else
				if (converter.StartsWith("Enum:"))
				{
					var enumName = converter["Enum:".Length..];
					ProcessInternalEnum(enumName, para.Content, paraDef, context, instruction);
				}
				else
				{
					switch (converter)
					{
						case "Register":
							{
								if (!TryParseRegister(para.Content, context, out var registerID))
								{
									return result;
								}
								WriteData(instruction, paraDef.ExpectdValue.Type, paraDef.ExpectdValue.Pos, &registerID);
							}
							break;
						case "Integer32":
							{
								if (!TryParseInt32(para.Content, context, out var registerID))
								{
									return result;
								}
								WriteData(instruction, paraDef.ExpectdValue.Type, paraDef.ExpectdValue.Pos, (byte*)&registerID);
							}
							break;
						default:
							break;
					}
				}
			}
			result.Result = true;
			return result;
		}
		public unsafe static OperationResult<ManagedSVMProgram?> Finialize(ISADefinition definition, IntermediateObject Obj)
		{
			OperationResult<ManagedSVMProgram?> operationResult = new OperationResult<ManagedSVMProgram?>(null);
			ManagedSVMProgram program = new ManagedSVMProgram();
			LinkingContext context = new LinkingContext(program, Obj, definition);
			List<byte[]> Data = new List<byte[]>();
			uint offset = 0;
			foreach (var item in Obj.data)
			{
				var data = Encoding.UTF8.GetBytes(item.Value);
				byte[] data2 = new byte[data.Length + sizeof(int)];
				fixed (byte* ptr = data2)
				{
					int len = data.Length;
					((IntPtr)ptr).SetData(len);
				}
				Buffer.BlockCopy(data, 0, data2, sizeof(int), data.Length);
				context.DataOffsets.Add(item.Key, offset);
				offset += (uint)data2.Length;
				Data.Add(data);
			}
			foreach (var item in Obj.instructions)
			{
				if (definition.InstructionDefinitions.TryGetValue(item.inst, out var def))
				{

					var instruction = stackalloc SVMInstruction[def.InstructionCount];
					var inst = translate(def, context, item, instruction);
					if (operationResult.CheckAndInheritErrorAndWarnings(inst))
					{
						return operationResult;
					}
					if (inst.Result)
					{
						for (int i = 0; i < def.InstructionCount; i++)
						{

							program.instructions.Add(instruction[i]);
						}

					}
				}
			}
			program.Datas = new byte[offset];
			int offset2 = 0;
			foreach (var item in Data)
			{
				Buffer.BlockCopy(item, 0, program.Datas, offset2, item.Length);
				offset2 += item.Length;
			}
			operationResult.Result = program;
			return operationResult;
		}
	}
}

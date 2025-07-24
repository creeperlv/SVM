using LibCLCC.NET.Operations;
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
			return operationResult;
		}
		public bool TryParseRegister(string input, IntermediateObject obj, LinkingContext context, out byte registerID)
		{
			if (input.StartsWith("$"))
			{

			}
			registerID = byte.MaxValue;
			return false;
		}
		public static OperationResult<SVMInstruction> translate(InstructionDefinition def, LinkingContext context, IntermediateInstruction iinstruction)
		{
			OperationResult<SVMInstruction> result = new OperationResult<SVMInstruction>();
			for (int i = 0; i < iinstruction.Parameters.Count; i++)
			{
				var para = iinstruction.Parameters[i];
				var paraDef = def.ParameterPattern[i];
				string converter = paraDef.ExpectdValue.Converter;
				if (converter.StartsWith("InternalEnum:"))
				{
					var enumName = converter["InternalEnum:".Length..];
					switch (enumName)
					{
						default:
							break;
					}
				}
				else
				{
					switch (converter)
					{
						case "Register":

							break;
						default:
							break;
					}
				}
			}
			return result;
		}
		public unsafe static OperationResult<ManagedSVMProgram?> Finialize(ISADefinition definition, IntermediateObject Obj)
		{
			OperationResult<ManagedSVMProgram?> operationResult = new OperationResult<ManagedSVMProgram?>(null);
			ManagedSVMProgram program = new ManagedSVMProgram();
			LinkingContext context = new LinkingContext(program, Obj);
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

					var inst = translate(def, context, item);
					if (operationResult.CheckAndInheritErrorAndWarnings(inst))
					{
						return operationResult;
					}
					program.instructions.Add(inst.Result);
				}
			}
			program.Datas = new byte[offset];
			int offset2 = 0;
			foreach (var item in Data)
			{
				Buffer.BlockCopy(item, 0, program.Datas, offset2, item.Length);
				offset2 += item.Length;
			}
			return operationResult;
		}
	}
}

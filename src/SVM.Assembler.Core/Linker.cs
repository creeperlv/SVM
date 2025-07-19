using LibCLCC.NET.Operations;
using SVM.Core;
using System.Collections.Generic;

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
		public static OperationResult<ManagedSVMProgram?> Freeze(List<IntermediateObject> objs)
		{
			OperationResult<ManagedSVMProgram?> operationResult = new OperationResult<ManagedSVMProgram?>(null);

			return operationResult;
		}
	}
}

using SVM.Core;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	public class ISADefinition
	{
		public Dictionary<string, InstructionDefinition> InstructionDefinitions = new Dictionary<string, InstructionDefinition>();
		public Dictionary<PrimaryInstruction, LinkerFunction> LinkerFunctions = new Dictionary<PrimaryInstruction, LinkerFunction>();
		public void Init()
		{
			foreach (var item in InstructionDefinitions)
			{
				if (!LinkerFunctions.TryAdd(item.Value.PrimaryInstruction, item.Value.linkerFunction))
				{
					LinkerFunctions[item.Value.PrimaryInstruction] = item.Value.linkerFunction;
				}
			}
		}
	}
}

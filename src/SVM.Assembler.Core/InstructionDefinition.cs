using SVM.Core;
using System;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	[Serializable]
	public class InstructionDefinition
	{
		public PrimaryInstruction PrimaryInstruction = PrimaryInstruction.Nop;
		public List<string> aliases = new List<string>();
		public List<InstructionParameter> ParameterPattern = new List<InstructionParameter>();
	}
}

using SVM.Core;
using System;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	[Serializable]
	public class InstructionDefinition
	{
		public string MatchID;
		public PrimaryInstruction PrimaryInstruction;
		public LinkerFunction linkerFunction;
		public InstructionDefinition(string matchID, LinkerFunction assemblerFunction)
		{
			MatchID = matchID;
			this.linkerFunction = assemblerFunction;
		}

		public List<InstructionParameter> ParameterPattern = new List<InstructionParameter>();
	}
}

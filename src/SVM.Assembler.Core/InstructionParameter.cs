using SVM.Core;
using System;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	[Serializable]
	public class InstructionParameter
	{
		public List<string> AllowedTokenIds = new List<string>();
		public ExpectdValue ExpectdValue = new ExpectdValue();
	}
	[Serializable]
	public class ExpectdValue
	{
		public SVMNativeTypes Type;
		public int Pos;
		public string Converter = "";
	}
}

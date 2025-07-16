using System;
using System.Collections.Generic;
namespace SVM.Core
{
	[Serializable]
	public class DebugSymbol
	{
		public Dictionary<string, uint> Functions = new Dictionary<string, uint>();
	}
	public class RuntimeBinary
	{
		public uint offset;
		public SimpleVirtualMachine? BindedMachine;
		public DebugSymbol? Symbol;
		public bool Invoke(string funcName)
		{
			if (BindedMachine == null) return false;
			if (Symbol == null) return false;
			if (!Symbol.Functions.TryGetValue(funcName, out var func))
			{
				return false;
			}

			return false;
		}
	}
}

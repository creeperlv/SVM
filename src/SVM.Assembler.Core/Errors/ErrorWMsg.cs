using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Assembler.Core.Errors
{
	public class ErrorWMsg : Error
	{
		public String Message;
		public LexSegment? Pos;
		public ErrorWMsg(string message, LexSegment? pos = null)
		{
			Message = message;
			Pos = pos;
		}

		public override string ToString()
		{
			if (Pos == null)
				return Message;
			else
				return Message + $" At: {Pos.SourceID}:{Pos.Position}";
		}
	}
}

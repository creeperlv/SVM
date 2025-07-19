using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using SVM.Core;
using System;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	public class Assembler
	{
		public const string LexDefinition =
@"
Match:

D [0-9]
Number {D}+
InstMath math
InstCvt cvt
InstSystem system
InstSys sys
InstSD sd
Register \${D}+
LabelCode \.code\:
LabelData \.data\:
 \.const\:
string "".*""
word \w*
GenericLabel {word}\:
LineEnd \n

OpAdd add
OpSub sub
OpMul mul
OpDiv div

Id:

InstMath inst
InstCvt inst
InstSystem inst
InstSys inst
InstSD sd
string String
Number Number
Register Register
OpAdd BOp
OpSub BOp
OpMul BOp
OpDiv BOp
LineEnd LE
GenericLabel LblG
LabelCode InternalLbl
LabelData InternalLbl
LabelConstant InternalLbl
";
		LexerDefinition? definition;
		public Assembler()
		{
			if (LexerDefinition.TryParse(LexDefinition, out definition))
			{
				definition.Substitute();
			}
		}
		public OperationResult<IntermediateObject> Assemble(string input, string ID = "main.asm")
		{
			StringLexer lexer = new StringLexer();
			lexer.SetDefinition(definition);
			lexer.Content = input;
			lexer.SourceID = ID;
			OperationResult<IntermediateObject> operationResult = new OperationResult<IntermediateObject>();
			while (true)
			{
				var lexResult = lexer.Lex();
				if (lexResult.Result == null) break;
				var r = lexResult.Result;
				switch (r.LexSegmentId)
				{
					case "InternalLbl":
						break;
					default:
						break;
				}
			}

			return operationResult;
		}
	}
	[Serializable]
	public class IntermediateObject
	{
		public Dictionary<string, string> data = new Dictionary<string, string>();
		public List<IntermediateInstruction> instructions = new List<IntermediateInstruction>();
	}
	[Serializable]
	public class IntermediateInstruction
	{
		public string? Label = null;
		public PrimaryInstruction inst;
		public List<LexSegment> Parameters = new List<LexSegment>();
	}
}

using LibCLCC.NET.Lexer;
using System;

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
string "".*""
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
";
		LexerDefinition? definition;
		public Assembler()
		{
			LexerDefinition.TryParse(LexDefinition, out definition);
		}
	}
}

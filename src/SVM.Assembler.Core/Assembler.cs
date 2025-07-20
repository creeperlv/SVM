using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using SVM.Core;
using System;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	public delegate OperationResult<SVMInstruction> LinkerFunction(LinkingContext context, IntermediateInstruction instruction);
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
		public ISADefinition ISA;
		public Assembler(ISADefinition isaDefinition)
		{
			if (LexerDefinition.TryParse(LexDefinition, out definition))
			{
				definition.Substitute();
			}

			ISA = isaDefinition;
		}
		public OperationResult<LexSegment?> Lex(ILexer lexer)
		{
			while (true)
			{
				var r = lexer.Lex();
				if (r.HasError()) return r;
				if (r.Result == null) return r;
				if (r.Result.LexSegmentId != null && r.Result.LexMatchedItemId != null)
				{
					return r;
				}
			}
		}
		public OperationResult<(string, string)?> ParseKVPair(ILexer lexer, LexSegment currentSeg)
		{
			OperationResult<(string, string)?> operationResult = new OperationResult<(string, string)?>(null);
			var r = Lex(lexer);
			if (operationResult.CheckAndInheritErrorAndWarnings(r)) { return operationResult; }
			if (currentSeg.Content == null) return operationResult;
			if (r.Result == null) return operationResult;
			if (r.Result.Content == null) return operationResult;
			return (currentSeg.Content, r.Result.Content);
		}
		private OperationResult<IntermediateInstruction?> ParseInstruction(StringLexer lexer, LexSegment CurrentSeg, LexSegment? Label)
		{
			OperationResult<IntermediateInstruction?> operationResult = new OperationResult<IntermediateInstruction?>(null);
			IntermediateInstruction intermediateInstruction = new IntermediateInstruction();
			intermediateInstruction.Label = Label;
			LexSegment LexDef;
			if (Label is null)
			{
				LexDef = CurrentSeg;
			}
			else
			{
				var next = Lex(lexer);
				if (operationResult.CheckAndInheritErrorAndWarnings(next))
				{
					return operationResult;
				}
				if (next.Result is null)
				{
					return operationResult;
				}
				LexDef = next.Result;
			}
			if (LexDef.LexMatchedItemId == null)
			{
				return operationResult;
			}
			if (!ISA.InstructionDefinitionAliases.TryGetValue(LexDef.LexMatchedItemId, out var instructionDef))
			{
				return operationResult;
			}
			foreach (var item in instructionDef.ParameterPattern)
			{
				var next = Lex(lexer);
				if (operationResult.CheckAndInheritErrorAndWarnings(next))
				{
					return operationResult;
				}
				if (next.Result is null)
				{
					return operationResult;
				}
				if (next.Result.LexSegmentId is null)
				{
					return operationResult;
				}
				if (!item.AllowedTokenIds.Contains(next.Result.LexSegmentId))
				{
					return operationResult;
				}
				intermediateInstruction.Parameters.Add(next.Result);
			}
			return intermediateInstruction;
		}
		public OperationResult<IntermediateObject> AssembleIntermediateObject(string input, string ID = "main.asm")
		{
			StringLexer lexer = new StringLexer();
			lexer.SetDefinition(definition);
			lexer.Content = input;
			lexer.SourceID = ID;
			OperationResult<IntermediateObject> operationResult = new OperationResult<IntermediateObject>(new IntermediateObject());
			InternalLabel CurrentLabel = InternalLabel.Code;
			while (true)
			{
				var lexResult = Lex(lexer);
				if (lexResult.Result == null) break;
				var r = lexResult.Result;
				switch (r.LexSegmentId)
				{
					case "InternalLbl":
						switch (r.LexMatchedItemId)
						{
							case "LabelCode":
								CurrentLabel = InternalLabel.Code;
								break;
							case "LabelData":
								CurrentLabel = InternalLabel.Data;
								break;
							case "LabelConstant":
								CurrentLabel = InternalLabel.Const;
								break;
							default:
								break;
						}
						break;
					default:
						switch (CurrentLabel)
						{
							case InternalLabel.Code:
								{
									OperationResult<IntermediateInstruction?> instR = r.LexSegmentId switch
									{
										"GenericLabel" => ParseInstruction(lexer, r, r),
										_ => ParseInstruction(lexer, r, null),
									};
									if (operationResult.CheckAndInheritErrorAndWarnings(instR))
									{
										return operationResult;
									}
									if (instR.Result is null) return operationResult;
									operationResult.Result.instructions.Add(instR.Result);
								}
								break;
							case InternalLabel.Data:
								{
									var KV = ParseKVPair(lexer, r);
									if (operationResult.CheckAndInheritErrorAndWarnings(KV))
									{
										return operationResult;
									}
									if (KV.Result == null)
									{
										return operationResult;
									}
									operationResult.Result.data.Add(KV.Result.Value.Item1, KV.Result.Value.Item2);
								}
								break;
							case InternalLabel.Const:
								{
									var KV = ParseKVPair(lexer, r);
									if (operationResult.CheckAndInheritErrorAndWarnings(KV))
									{
										return operationResult;
									}
									if (KV.Result == null)
									{
										return operationResult;
									}
									operationResult.Result.consts.Add(KV.Result.Value.Item1, KV.Result.Value.Item2);
								}
								break;
							default:
								break;
						}
						break;
				}
			}

			return operationResult;
		}
	}
	public enum InternalLabel
	{
		Code, Data, Const
	}
	[Serializable]
	public class IntermediateObject
	{
		public Dictionary<string, string> data = new Dictionary<string, string>();
		public Dictionary<string, string> consts = new Dictionary<string, string>();
		public List<IntermediateInstruction> instructions = new List<IntermediateInstruction>();
	}
	[Serializable]
	public class IntermediateInstruction
	{
		public LexSegment? Label = null;
		public PrimaryInstruction inst;
		public List<LexSegment> Parameters = new List<LexSegment>();
	}
}

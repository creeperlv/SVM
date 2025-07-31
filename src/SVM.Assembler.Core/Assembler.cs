using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using SVM.Assembler.Core.Errors;
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

InstMath bmath
InstSDInt32 sd\.int32
InstSDInt sd\.int
InstSDInt64 sd\.int64
InstSDLong sd\.long
InstCvt cvt
InstSystem system
InstSys sys
InstSD sd
Register \${D}+
LabelCode \.code\:
LabelData \.data\:
LabelConst \.const\:
word [\w\d\.]+
GenericLabel {word}\:
LineEnd \n
string ""\"".*\""""
Number \d+
D \d


Id:
word Word
InstSDInt64 inst
InstSDLong inst
InstMath inst
InstSDInt inst
InstSDInt32 inst
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
			else
			{
			}

			ISA = isaDefinition;
		}
		private OperationResult<LexSegment?> Lex(ILexer lexer)
		{
			while (true)
			{
				var r = lexer.Lex();
				if (r.HasError()) return r;
				if (r.Result == null) return r;

				if (r.Result.LexSegmentId != null && r.Result.LexMatchedItemId != null)
				{
					if (r.Result.LexSegmentId == "LE") continue;

					return r;
				}

			}
		}
		private OperationResult<(string, string)?> ParseKVPair(ILexer lexer, LexSegment currentSeg)
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
			if (!ISA.InstructionDefinitionAliases.TryGetValue(LexDef.Content ?? "", out var instructionDef))
			{
				operationResult.AddError(new ErrorWMsg($"Unknown Instruction:{LexDef.Content ?? "<null>"}", LexDef));
				return operationResult;
			}
			intermediateInstruction.InstDefID = instructionDef.Id;
			intermediateInstruction.inst = instructionDef.PrimaryInstruction;
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
					operationResult.AddError(new ErrorWMsg($"Token: {LexDef.Content ?? "<null>"} is not allowed here.", LexDef));
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
										"LblG" => ParseInstruction(lexer, r, r),
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
		public int DetermineFinalInstructionCount(LinkingContext context)
		{
			int count = 0;
			foreach (var item in instructions)
			{
				if (context.Definition.InstructionDefinitions.TryGetValue(item.InstDefID, out var def))
				{
					count += def.InstructionCount;
				}
			}
			return count;
		}
		public bool TryGetConst(string str, out string value)
		{
			return consts.TryGetValue(str, out value);
		}
		public bool TryGetDataOffset(string str, out string value)
		{
			return consts.TryGetValue(str, out value);
		}
		public bool TryGetLabelPC(string label, out int PC)
		{
			for (int i = 0; i < instructions.Count; i++)
			{
				IntermediateInstruction? item = instructions[i];
				if (item.Label != null && item.Label.Content == label)
				{
					PC = i;
					return true;
				}
			}
			PC = -1;
			return false;
		}
	}
	[Serializable]
	public class IntermediateInstruction
	{
		public LexSegment? Label = null;
		public string InstDefID = "";
		public PrimaryInstruction inst;
		public List<LexSegment> Parameters = new List<LexSegment>();
	}
}

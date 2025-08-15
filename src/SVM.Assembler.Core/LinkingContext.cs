using SVM.Core;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	public class LinkingContext
	{
		public ISADefinition Definition;
		public Dictionary<string, uint> DataOffsets = new Dictionary<string, uint>();
		public ManagedSVMProgram Program;
		public Dictionary<string, int> label = new Dictionary<string, int>();
		public IntermediateObject IntermediateObject;
		public LinkingContext(ManagedSVMProgram program, IntermediateObject intermediateObject, ISADefinition definition)
		{
			Program = program;
			IntermediateObject = intermediateObject;
			Definition = definition;
		}
		public unsafe bool TryFindData(string label, out uint offset)
		{
			if (DataOffsets.TryGetValue(label, out offset))
			{
				//Don't directly give the length there.
				offset += (uint)IntermediateObject.DetermineFinalInstructionCount(this) * (uint)sizeof(SVMInstruction) + sizeof(int);
				return true;
			}
			return false;
		}
		public bool TryFindLabel(string label, out int offset)
		{
			label = label + ":";
			if (this.label.TryGetValue(label, out offset))
			{
				return true;
			}
			int acc=0;
			for (int i = 0; i < IntermediateObject.instructions.Count; i++)
			{
				IntermediateInstruction? item = IntermediateObject.instructions[i];
				if (item.Label != null)
					if (item.Label.Content == label)
					{
						this.label.Add(label, acc);
						offset = acc;
						return true;
					}
				acc += Definition.InstructionDefinitions[item.InstDefID].InstructionCount;
			}
			return false;
		}
	}
}

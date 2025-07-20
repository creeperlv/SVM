using SVM.Core;
using System.Collections.Generic;

namespace SVM.Assembler.Core
{
	public class LinkingContext
	{
		public Dictionary<string, uint> DataOffsets = new Dictionary<string, uint>();
		public ManagedSVMProgram Program;
		public Dictionary<string, int> label = new Dictionary<string, int>();
		public IntermediateObject IntermediateObject;
		public LinkingContext(ManagedSVMProgram program, IntermediateObject intermediateObject)
		{
			Program = program;
			IntermediateObject = intermediateObject;
		}
		public bool TryFindLabel(string label, out int offset)
		{
			label = label + ":";
			if (this.label.TryGetValue(label, out offset))
			{
				return true;
			}
			for (int i = 0; i < IntermediateObject.instructions.Count; i++)
			{
				IntermediateInstruction? item = IntermediateObject.instructions[i];
				if (item.Label != null)
					if (item.Label.Content == label)
					{
						this.label.Add(label, i);
						offset = i;
						return true;
					}
			}
			return false;
		}
	}
}

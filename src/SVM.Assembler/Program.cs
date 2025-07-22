using Newtonsoft.Json;
using SVM.Assembler.Core;
using System.Reflection;

namespace SVM.Assembler
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("SVM.Assembler.ISA.xml");
			if (fs is null)
			{
				Console.WriteLine("Cannot find ISA definition!");
				return;
			}
			if (!ISADefinition.TryParse(fs, out var def))
			{
				Console.WriteLine("Cannot load ISA definition!");
				return;
			}
			Console.WriteLine(JsonConvert.SerializeObject(def, Formatting.Indented));
		}
	}
}

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
			List<string> files = new List<string>();
			List<IntermediateObject> objs = new();
			string outputfile = "a.out";
			for (int i = 0; i < args.Length; i++)
			{
				string? item = args[i];
				switch (item)
				{
					case "-o":
						outputfile = args[i + 1];
						i++;
						break;
					default:
						if (File.Exists(item))
						{
							files.Add(item);
							continue;
						}
						break;
				}
			}
			if (files.Count == 0)
			{
				return;
			}
			Assembler.Core.Assembler assembler = new Core.Assembler(def);
			foreach (var item in files)
			{
				var result = assembler.AssembleIntermediateObject(File.ReadAllText(item), item);
				if (result.HasError())
				{
					foreach (var error in result.Errors)
					{
						Console.Error.WriteLine(error.ToString());
					}
					Console.Error.WriteLine($"Error at assembling {item}. Abort.");
					return;
				}
				objs.Add(result.Result);
			}
			{
				var lResult = Linker.Link(objs);
				if (lResult.HasError())
				{
					Console.Error.WriteLine("Linking error!");
					return;
				}
				if (lResult.Result == null)
				{
					Console.Error.WriteLine("Linker return no data!");
					return;
				}
				var fResult = Linker.Finialize(def, lResult.Result);
				if (fResult.HasError())
				{
					Console.Error.WriteLine("Finalizer error!");
					foreach (var item in fResult.Errors)
					{
						Console.Error.WriteLine(item.ToString());
					}
					return;
				}
				if (fResult.Result == null)
				{
					Console.Error.WriteLine("Linker Finalizer return no data!");
					return;
				}
				if (File.Exists(outputfile)) File.Delete(outputfile);
				using var stream = File.OpenWrite(outputfile);
				fResult.Result.WriteToStream(stream);
			}
		}
	}
}

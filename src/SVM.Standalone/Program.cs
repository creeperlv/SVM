using SVM.Core;

namespace SVM.Standalone;

class Program
{
	unsafe static void Main(string[] args)
	{
		string inputFile = "a.out";
		foreach (string arg in args)
		{
			if (File.Exists(arg))
			{
				inputFile = arg;
			}
		}
		if (!File.Exists(inputFile))
		{
			Console.WriteLine("Not input file found!");
			return;
		}
		using var fs = File.OpenRead(inputFile);
		var program = SVMProgram.LoadFromStream(fs);
		SimpleVirtualMachine svm = new()
		{
			Program = program
		};
		svm.Init();
		while (!svm.isReachBinaryEnd())
		{
			Console.WriteLine("Step");
			svm.Step();
		}
	}
}

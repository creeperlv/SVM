using SVM.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace SVM.Advanced.BSDStyleVM
{
	public class BSDLikeWrapper
	{
		public SimpleVirtualMachine baseVM;
		public Dictionary<int, FileDescripter> FDs = new Dictionary<int, FileDescripter>();
		public BSDLikeWrapper(SimpleVirtualMachine baseVM)
		{
			this.baseVM = baseVM;
			baseVM.AdditionalData = this;
			baseVM.Config = BSDConfig.CreateConfig();
		}
	}
	public class BSDConfig
	{
		public static SVMConfig CreateConfig()
		{
			var config = new SVMConfig();
			config.PCRegisterID = 1;
			config.SPRegisterID = 2;

			SetupSyscall(config);
			return config;
		}
		public static void SetupSyscall(SVMConfig config)
		{
			config.FuncCalls.Add(1, BSDStyleFunctions0.__exit);
		}
	}
	public static class BSDStyleFunctions0
	{
		public static void __exit(SimpleVirtualMachine machine)
		{
			var status = machine.registers.GetData<int>(10);
			Console.WriteLine("Bye-bye!");
			Environment.Exit(status);
		}
	}
	public class FileDescripter
	{
		public Stream stream;

		public FileDescripter(Stream stream)
		{
			this.stream = stream;
		}
	}
}

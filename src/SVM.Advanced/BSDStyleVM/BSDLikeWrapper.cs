using SVM.Core;
using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			FDs.Add(0, new FileDescripter(Console.OpenStandardInput()));
			FDs.Add(1, new FileDescripter(Console.OpenStandardOutput()));
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
			config.FuncCalls.Add(4, BSDStyleFunctions0.__write);
		}
	}
	public static class BSDStyleFunctions0
	{
		public static void __exit(SimpleVirtualMachine machine)
		{
			var status = machine.registers.GetData<int>(10);
			Environment.Exit(status);
		}
		public static void __write(SimpleVirtualMachine machine)
		{
			if (machine.AdditionalData is BSDLikeWrapper w)
			{
				var fd = machine.registers.GetData<int>(10);
				var ptr = machine.registers.GetData<SVMPointer>(11);
				var size = machine.registers.GetData<ulong>(12);
				if (w.FDs.TryGetValue(fd, out var descripter))
				{
					descripter.stream.WriteData(machine.GetPointer(ptr), size);
				}
				else
					Console.WriteLine($"FD:{fd} does not exist.");
			}
			else
			{
				Console.WriteLine("Incorrectly set wrapper!");
			}
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

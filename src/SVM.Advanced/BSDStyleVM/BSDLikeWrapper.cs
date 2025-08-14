using SVM.Core;
using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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
		public int FindAvailableFDID()
		{
			for (int i = 0; i < int.MaxValue; i++)
			{
				if (FDs.ContainsKey(i)) continue;
				return i;
			}
			return -1;
		}
	}
	public class BSDConfig
	{
		public static SVMConfig CreateConfig()
		{
			var config = new SVMConfig();
			config.PCRegisterID = 1;
			config.CFRegisterID = 2;

			SetupSyscall(config);
			return config;
		}
		public static void SetupSyscall(SVMConfig config)
		{
			config.FuncCalls.Add(1, BSDStyleFunctions0.__exit);
			config.FuncCalls.Add(4, BSDStyleFunctions0.__write);
			config.FuncCalls.Add(5, BSDStyleFunctions0.__open);
		}
	}
	public static class BSDFcntl
	{
		public const int O_RDONLY = 0x0;
		public const int O_WRONLY = 0x01;
		public const int O_RDWR = 0x02;
		public const int O_ACCMODE = 0x03;
	}
	public static class BSDStyleFunctions0
	{
		public static void __exit(SimpleVirtualMachine machine)
		{
			var status = machine.registers.GetData<int>(10);
			Environment.Exit(status);
		}
		public unsafe static void __open(SimpleVirtualMachine machine)
		{
			if (machine.AdditionalData is BSDLikeWrapper w)
			{
				var ptr = machine.registers.GetData<SVMPointer>(10);
				var size = machine.registers.GetData<ulong>(11);
				var flag = machine.registers.GetData<int>(12);
				var fn = Encoding.UTF8.GetString((byte*)machine.GetPointer(ptr), (int)size);
				FileMode fm = FileMode.Create;
				FileAccess fa = default;
				if ((flag & BSDFcntl.O_WRONLY) == BSDFcntl.O_WRONLY)
				{
					fa = FileAccess.Write;
				}
				var fdID = w.FindAvailableFDID();
				if (fdID == -1) return;
				var stream = File.Open(fn, fm, fa);
				FileDescripter fd = new FileDescripter(stream);
				w.FDs.Add(fdID, fd);
				machine.registers.SetDataInRegister<int>(10,fdID);
			}
			else
			{
				Console.WriteLine("Incorrectly set wrapper!");
			}
		}
		public unsafe static void __write(SimpleVirtualMachine machine)
		{
			if (machine.AdditionalData is BSDLikeWrapper w)
			{
				var fd = machine.registers.GetData<int>(10);
				var ptr = machine.registers.GetData<SVMPointer>(11);
				var size = machine.registers.GetData<ulong>(12);
				if (w.FDs.TryGetValue(fd, out var descripter))
				{
					Console.OpenStandardOutput().WriteData(machine.GetPointer(ptr), size);
					descripter.stream.WriteData(machine.GetPointer(ptr), size);
					descripter.stream.Flush();
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

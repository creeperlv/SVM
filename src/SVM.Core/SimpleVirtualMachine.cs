using System;
using static SVM.Core.stdc.stdlib;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using SVM.Core.Utils;
namespace SVM.Core
{
	public unsafe class SimpleVirtualMachine : IDisposable
	{
		public Registers registers;
		public MemoryBlock Stack;
		public MemoryBlock* GPMemory;
		public SVMConfig? Config = null;
		public void Init(uint StackSize = 1024 * 1024, uint RegisterSize = 512)
		{
			GPMemory = null;
		}
		public void Step()
		{
			uint SPOffset = 4;
			uint PCOffset = 0;
			if (Config != null)
			{
				SPOffset = Config.SPRegisterOffset;
				PCOffset = Config.PCRegisterOffset;
			}
		}
		public void SetGPMemory(MemoryBlock* ptr)
		{
			GPMemory = ptr;
		}
		public void Dispose()
		{
			registers.Dispose();
			Stack.Dispose();
		}
	}
	public class SVMConfig
	{
		public Dictionary<uint, FuncCall> FuncCalls = new Dictionary<uint, FuncCall>();
		public uint SPRegisterOffset;
		public uint PCRegisterOffset;
	}
	public delegate void FuncCall(SimpleVirtualMachine machine);
	[StructLayout(LayoutKind.Sequential)]
	public struct MState
	{
		public uint PC;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct MemoryBlock : IDisposable
	{
		public IntPtr StartAddress;
		public int Size;

		public void Dispose()
		{
			free(StartAddress);
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct Registers : IDisposable
	{
		IntPtr Data;
		public void Init(int size)
		{
			Data = malloc((uint)size);
		}
		public T ReadData<T>(int RegisterID) where T : unmanaged
		{
			return Data.GetDataWithOffsetInBytes<T>(RegisterID * sizeof(Int64));
		}
		public void Dispose()
		{
			free(Data);
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct Callframe
	{
		public int PC;
		public uint SP;
	}

}

using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SVM.Core.stdc
{
	/// <summary>
	/// A simple library contains POSIX-like impl of memory functions.
	/// </summary>
	public static class stdlib
	{
		public static IntPtr malloc(uint size)
		{
			return Marshal.AllocHGlobal((int)size);
		}
		public static IntPtr malloc(int size)
		{
			return Marshal.AllocHGlobal(size);
		}
		public static IntPtr calloc(int size)
		{
			var ptr = Marshal.AllocHGlobal(size);
			for (int i = 0; i < size; i++)
			{
				System.Runtime.InteropServices.Marshal.WriteByte(ptr, i, 0);
			}
			return ptr;
		}
		public static IntPtr calloc(uint size)
		{
			var ptr = Marshal.AllocHGlobal((int)size);
			for (int i = 0; i < size; i++)
			{
				System.Runtime.InteropServices.Marshal.WriteByte(ptr, i, 0);
			}
			return ptr;
		}
		public static IntPtr realloc(IntPtr ptr, uint size)
		{
			return Marshal.ReAllocHGlobal(ptr, (IntPtr)size);
		}
		public static void free(IntPtr ptr)
		{
			Marshal.FreeHGlobal(ptr);
		}
	}
	public static class cstring
	{
		public static void memset(IntPtr ptr, byte value, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Marshal.WriteByte(ptr, value);
			}
		}
		public unsafe static void memcpy(IntPtr src, IntPtr dst, int size, int count)
		{
			Buffer.MemoryCopy((void*)src, (void*)dst, size, count);
		}
	}
}

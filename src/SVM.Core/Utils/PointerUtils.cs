using System;
using System.Runtime.InteropServices;
using static SVM.Core.stdc.cstring;
namespace SVM.Core.Utils
{
	public unsafe static class PointerUtils
	{
		public static T GetData<T>(this IntPtr ptr) where T : unmanaged
		{
			return Marshal.PtrToStructure<T>(ptr);
		}
		public static T GetDataWithOffsetInBytes<T>(this IntPtr ptr, int Offset) where T : unmanaged
		{
			return Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Offset));
		}
		public static T GetDataWithOffsetInStructCount<T>(this IntPtr ptr, int Count) where T : unmanaged
		{
			return Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Count * sizeof(T)));
		}
		public static void GetData<T>(this IntPtr ptr, IntPtr dest) where T : unmanaged
		{
			memcpy(ptr, dest, sizeof(T), 1);
		}
		public static void GetDataWithOffsetInBytes<T>(this IntPtr ptr, IntPtr dest, int offset) where T : unmanaged
		{
			memcpy(IntPtr.Add(ptr, offset), dest, sizeof(T), 1);
		}
		public static void GetDataWithOffsetInStructCount<T>(this IntPtr ptr, IntPtr dest, int offset) where T : unmanaged
		{
			memcpy(IntPtr.Add(ptr, offset * sizeof(T)), dest, sizeof(T), 1);
		}
		public static void GetData<T>(this IntPtr ptr, T* dest) where T : unmanaged
		{
			GetData<T>(ptr, (IntPtr)dest);
		}
		public static void GetDataWithOffsetInBytes<T>(this IntPtr ptr, T* dest, int offset) where T : unmanaged
		{
			GetDataWithOffsetInBytes<T>(ptr, (IntPtr)dest, offset);
		}
		public static void GetDataWithOffsetInStructCount<T>(this IntPtr ptr, T* dest, int offset) where T : unmanaged
		{
			GetDataWithOffsetInStructCount<T>(ptr, (IntPtr)dest, offset);
		}
		public static void SetData<T>(this IntPtr ptr, T data) where T : unmanaged
		{
			var srcPtr = (byte*)&data;
			Buffer.MemoryCopy(srcPtr, (byte*)ptr, sizeof(T), sizeof(T));
		}
	}
}

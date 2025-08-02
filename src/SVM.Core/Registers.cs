using SVM.Core.Utils;
using System;
using System.Runtime.InteropServices;
using static SVM.Core.stdc.stdlib;
namespace SVM.Core
{
	/// <summary>
	/// Offset 0 is always 0.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Registers : IDisposable
	{
		IntPtr Data;
		uint Size;
		public void Init(uint size)
		{
			Size = size;
			Data = calloc(size);
		}
		public T ReadData<T>(int RegisterID) where T : unmanaged
		{
			if (RegisterID == 0) return default;
			return Data.GetDataWithOffsetInBytes<T>(RegisterID * sizeof(Int64));
		}
		public T GetData<T>(int RegisterID) where T : unmanaged
		{
			if (RegisterID == 0) return default;
			return Data.GetDataWithOffsetInBytes<T>(RegisterID * sizeof(Int64));
		}
		public unsafe byte* GetPtr(int RegisterID)
		{
			if (RegisterID == 0) return null;
			return ((byte*)Data) + RegisterID * sizeof(long);
		}
		public unsafe void SetData<T>(int offset, T d) where T : unmanaged
		{
			if (offset == 0) return;
			if (offset + sizeof(T) > Size) return;
			((T*)Data + offset)[0] = d;
		}
		public unsafe void SetDataInRegister<T>(int offset, T d) where T : unmanaged
		{
			if (offset == 0) return;
			if (offset * sizeof(ulong) + sizeof(T) > Size) return;
			((T*)((byte*)Data + offset * sizeof(ulong)))[0] = d;
		}
		public unsafe void SetDataInRegister(int offset, IntPtr ptr, int size)
		{
			if (offset == 0) return;
			if (offset * sizeof(ulong) + size > Size) return;
			Buffer.MemoryCopy((byte*)ptr, (byte*)Data + offset * sizeof(ulong), size, size);
		}
		public unsafe void SetDataOffsetInBytes<T>(int offset, T d) where T : unmanaged
		{
			if (offset == 0) return;
			if (offset + sizeof(T) > Size) return;
			((T*)((byte*)Data + offset))[0] = d;
		}
		public void Dispose()
		{
			free(Data);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SVM.Core.Utils
{
	public static class StreamUtils
	{
		public unsafe static void WriteData<T>(this Stream s, T data) where T : unmanaged
		{
			var ptr=&data;
			Span<byte> buffer = new(ptr, sizeof(T));
			s.Write(buffer);
		}
		public unsafe static bool TryReadData<T>(this Stream stream, out T data) where T : unmanaged
		{
			int len = sizeof(T);
			var dataPtr = stackalloc byte[len];
			Span<byte> buffer = new Span<byte>(dataPtr, len);
			var count = stream.Read(buffer);
			if (count != len)
			{
				data = default;
				return false;
			}
			data = ((T*)dataPtr)[0];
			return true;
		}
	}
}

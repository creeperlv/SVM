using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SVM.Core.Data
{
	public interface INumbericData<T> : IPointerWritable, ICastable where T : unmanaged
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Add(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Sub(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Mul(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Div(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Mod(T R);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		SVMSimpleResult<T> AddOF(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		SVMSimpleResult<T> SubOF(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		SVMSimpleResult<T> DivOF(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		SVMSimpleResult<T> MulOF(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool LT(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool GT(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool LE(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool GE(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool EQ(T R);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool NE(T R);
	}
	public interface ICastable
	{
		INumbericData<CompactByte> Cast_Byte();
		INumbericData<CompactSByte> Cast_SByte();
		INumbericData<CompactShort> Cast_Short();
		INumbericData<CompactUShort> Cast_UShort();
		INumbericData<CompactInt> Cast_Int();
		INumbericData<CompactUInt> Cast_UInt();
		INumbericData<CompactLong> Cast_Long();
		INumbericData<CompactULong> Cast_ULong();
		INumbericData<CompactDouble> Cast_Double();
		INumbericData<CompactSingle> Cast_Float();
	}

	public unsafe interface IPointerWritable
	{
		public void Write(byte* targetPtr);
		public int SizeOf();
	}
	public struct SVMSimpleResult<T> where T : unmanaged
	{
		public bool IsSuccess;
		public T Value;

		public SVMSimpleResult(bool isSuccess, T value)
		{
			IsSuccess = isSuccess;
			Value = value;
		}
	}
}

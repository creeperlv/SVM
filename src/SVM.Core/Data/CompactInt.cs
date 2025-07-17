using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace SVM.Core.Data
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CompactInt : INumbericData<CompactInt>
	{
		public int Value;
		public CompactInt(int value) { Value = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactInt Add(CompactInt R)
		{
			return new CompactInt(Value + R.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactInt Sub(CompactInt R)
		{
			return new CompactInt(Value - R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactInt Mul(CompactInt R)
		{
			return new CompactInt(Value * R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactInt Div(CompactInt R)
		{
			return new CompactInt(Value / R.Value);
		}

		public SCVMSimpleResult<CompactInt> AddOF(CompactInt R)
		{
			CompactInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactInt(Value + R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactInt(Value + R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactInt>(IsOF, result);
		}

		public SCVMSimpleResult<CompactInt> SubOF(CompactInt R)
		{
			CompactInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactInt(Value - R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactInt(Value - R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactInt>(IsOF, result);
		}

		public SCVMSimpleResult<CompactInt> DivOF(CompactInt R)
		{
			CompactInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactInt(Value / R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactInt(Value / R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactInt>(IsOF, result);
		}

		public SCVMSimpleResult<CompactInt> MulOF(CompactInt R)
		{
			CompactInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactInt(Value * R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactInt(Value * R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactInt>(IsOF, result);
		}

		public bool LT(CompactInt R)
		{
			return Value < R.Value;
		}

		public bool GT(CompactInt R)
		{
			return Value > R.Value;
		}

		public bool LE(CompactInt R)
		{
			return Value <= R.Value;
		}

		public bool GE(CompactInt R)
		{
			return Value >= R.Value;
		}

		public bool EQ(CompactInt R)
		{
			return Value == R.Value;
		}

		public bool NE(CompactInt R)
		{
			return Value != R.Value;
		}
		public INumbericData<CompactByte> Cast_Byte()
		{
			return new CompactByte((byte)Value);
		}

		public INumbericData<CompactSByte> Cast_SByte()
		{
			return new CompactSByte((sbyte)Value);
		}

		public INumbericData<CompactShort> Cast_Short()
		{
			return new CompactShort((short)Value);
		}

		public INumbericData<CompactUShort> Cast_UShort()
		{
			return new CompactUShort((ushort)Value);
		}

		public INumbericData<CompactInt> Cast_Int()
		{
			return new CompactInt(Value);
		}

		public INumbericData<CompactUInt> Cast_UInt()
		{
			return new CompactUInt((uint)Value);
		}

		public INumbericData<CompactLong> Cast_Long()
		{
			return new CompactLong(Value);
		}

		public INumbericData<CompactULong> Cast_ULong()
		{
			return new CompactULong((ulong)Value);
		}

		public INumbericData<CompactDouble> Cast_Double()
		{
			return new CompactDouble(Value);
		}

		public INumbericData<CompactSingle> Cast_Float()
		{
			return new CompactSingle(Value);
		}

		public unsafe void Write(byte* targetPtr)
		{
			((int*)targetPtr)[0] = Value;
		}
		public int SizeOf()
		{
			return sizeof(int);
		}

		public CompactInt Mod(CompactInt R)
		{
			return new CompactInt(Value % R.Value);
		}
	}

}

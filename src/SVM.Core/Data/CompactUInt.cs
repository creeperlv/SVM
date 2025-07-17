using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SVM.Core.Data
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CompactUInt : INumbericData<CompactUInt>
	{
		public uint Value;
		public CompactUInt(uint value) { Value = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUInt Add(CompactUInt R)
		{
			return new CompactUInt(Value + R.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUInt Sub(CompactUInt R)
		{
			return new CompactUInt(Value - R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUInt Mul(CompactUInt R)
		{
			return new CompactUInt(Value * R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUInt Div(CompactUInt R)
		{
			return new CompactUInt(Value / R.Value);
		}
		public SCVMSimpleResult<CompactUInt> AddOF(CompactUInt R)
		{
			CompactUInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUInt(Value + R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUInt(Value + R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactUInt>(IsOF, result);
		}

		public SCVMSimpleResult<CompactUInt> SubOF(CompactUInt R)
		{
			CompactUInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUInt(Value - R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUInt(Value - R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactUInt>(IsOF, result);
		}

		public SCVMSimpleResult<CompactUInt> DivOF(CompactUInt R)
		{
			CompactUInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUInt(Value / R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUInt(Value / R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactUInt>(IsOF, result);
		}

		public SCVMSimpleResult<CompactUInt> MulOF(CompactUInt R)
		{
			CompactUInt result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUInt(Value * R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUInt(Value * R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactUInt>(IsOF, result);
		}

		public bool LT(CompactUInt R)
		{
			return Value < R.Value;
		}

		public bool GT(CompactUInt R)
		{
			return Value > R.Value;
		}

		public bool LE(CompactUInt R)
		{
			return Value <= R.Value;
		}

		public bool GE(CompactUInt R)
		{
			return Value >= R.Value;
		}

		public bool EQ(CompactUInt R)
		{
			return Value == R.Value;
		}

		public bool NE(CompactUInt R)
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
			return new CompactInt((int)Value);
		}

		public INumbericData<CompactUInt> Cast_UInt()
		{
			return new CompactUInt(Value);
		}

		public INumbericData<CompactLong> Cast_Long()
		{
			return new CompactLong(Value);
		}

		public INumbericData<CompactULong> Cast_ULong()
		{
			return new CompactULong(Value);
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
			((uint*)targetPtr)[0] = Value;
		}
		public int SizeOf()
		{
			return sizeof(uint);
		}

		public CompactUInt Mod(CompactUInt R)
		{
			return new CompactUInt(Value % R.Value);
		}
	}

}

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace SVM.Core.Data
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CompactByte : INumbericData<CompactByte>
	{
		public byte Value;
		public CompactByte(byte value) { Value = value; }
		public CompactByte(int value) { Value = (byte)value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactByte Add(CompactByte R)
		{
			return new CompactByte(Value + R.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactByte Sub(CompactByte R)
		{
			return new CompactByte(Value - R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactByte Mul(CompactByte R)
		{
			return new CompactByte(Value * R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactByte Div(CompactByte R)
		{
			return new CompactByte(Value / R.Value);
		}
		public SVMSimpleResult<CompactByte> AddOF(CompactByte R)
		{
			CompactByte result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactByte(Value + R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactByte(Value + R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactByte>(IsOF, result);
		}

		public SVMSimpleResult<CompactByte> SubOF(CompactByte R)
		{
			CompactByte result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactByte(Value - R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactByte(Value - R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactByte>(IsOF, result);
		}

		public SVMSimpleResult<CompactByte> DivOF(CompactByte R)
		{
			CompactByte result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactByte(Value / R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactByte(Value / R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactByte>(IsOF, result);
		}

		public SVMSimpleResult<CompactByte> MulOF(CompactByte R)
		{
			CompactByte result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactByte(Value * R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactByte(Value * R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactByte>(IsOF, result);
		}

		public bool LT(CompactByte R)
		{
			return Value < R.Value;
		}

		public bool GT(CompactByte R)
		{
			return Value > R.Value;
		}

		public bool LE(CompactByte R)
		{
			return Value <= R.Value;
		}

		public bool GE(CompactByte R)
		{
			return Value >= R.Value;
		}

		public bool EQ(CompactByte R)
		{
			return Value == R.Value;
		}

		public bool NE(CompactByte R)
		{
			return Value != R.Value;
		}

		public INumbericData<CompactByte> Cast_Byte()
		{
			return new CompactByte(Value);
		}

		public INumbericData<CompactSByte> Cast_SByte()
		{
			return new CompactSByte((sbyte)Value);
		}

		public INumbericData<CompactShort> Cast_Short()
		{
			return new CompactShort(Value);
		}

		public INumbericData<CompactUShort> Cast_UShort()
		{
			return new CompactUShort(Value);
		}

		public INumbericData<CompactInt> Cast_Int()
		{
			return new CompactInt(Value);
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
			targetPtr[0] = Value;
		}
		public int SizeOf()
		{
			return sizeof(byte);
		}

		public CompactByte Mod(CompactByte R)
		{
			return new CompactByte(Value % R.Value);
		}
		public override string ToString()
		{
			return this.Value.ToString();
		}
	}

}

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SVM.Core.Data
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CompactUShort : INumbericData<CompactUShort>
	{
		public ushort Value;
		public CompactUShort(ushort value) { Value = value; }
		public CompactUShort(int value) { Value = (ushort)value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUShort Add(CompactUShort R)
		{
			return new CompactUShort(Value + R.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUShort Sub(CompactUShort R)
		{
			return new CompactUShort(Value - R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUShort Mul(CompactUShort R)
		{
			return new CompactUShort(Value * R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactUShort Div(CompactUShort R)
		{
			return new CompactUShort(Value / R.Value);
		}
		public SVMSimpleResult<CompactUShort> AddOF(CompactUShort R)
		{
			CompactUShort result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUShort(Value + R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUShort(Value + R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactUShort>(IsOF, result);
		}

		public SVMSimpleResult<CompactUShort> SubOF(CompactUShort R)
		{
			CompactUShort result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUShort(Value - R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUShort(Value - R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactUShort>(IsOF, result);
		}

		public SVMSimpleResult<CompactUShort> DivOF(CompactUShort R)
		{
			CompactUShort result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUShort(Value / R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUShort(Value / R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactUShort>(IsOF, result);
		}

		public SVMSimpleResult<CompactUShort> MulOF(CompactUShort R)
		{
			CompactUShort result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactUShort(Value * R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactUShort(Value * R.Value);
					}
				}
			}
			return new SVMSimpleResult<CompactUShort>(IsOF, result);
		}

		public bool LT(CompactUShort R)
		{
			return Value < R.Value;
		}

		public bool GT(CompactUShort R)
		{
			return Value > R.Value;
		}

		public bool LE(CompactUShort R)
		{
			return Value <= R.Value;
		}

		public bool GE(CompactUShort R)
		{
			return Value >= R.Value;
		}

		public bool EQ(CompactUShort R)
		{
			return Value == R.Value;
		}

		public bool NE(CompactUShort R)
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
			((ushort*)targetPtr)[0] = Value;
		}

		public int SizeOf()
		{
			return sizeof(ushort);
		}

		public CompactUShort Mod(CompactUShort R)
		{
			return new CompactUShort(Value % R.Value);
		}
		public override string ToString()
		{
			return this.Value.ToString();
		}
	}

}

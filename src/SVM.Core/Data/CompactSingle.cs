using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SVM.Core.Data
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CompactSingle : INumbericData<CompactSingle>
	{
		public float Value;
		public CompactSingle(float value) { Value = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactSingle Add(CompactSingle R)
		{
			return new CompactSingle(Value + R.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactSingle Sub(CompactSingle R)
		{
			return new CompactSingle(Value - R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactSingle Mul(CompactSingle R)
		{
			return new CompactSingle(Value * R.Value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CompactSingle Div(CompactSingle R)
		{
			return new CompactSingle(Value / R.Value);
		}
		public SCVMSimpleResult<CompactSingle> AddOF(CompactSingle R)
		{
			CompactSingle result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactSingle(Value + R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactSingle(Value + R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactSingle>(IsOF, result);
		}

		public SCVMSimpleResult<CompactSingle> SubOF(CompactSingle R)
		{
			CompactSingle result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactSingle(Value - R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactSingle(Value - R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactSingle>(IsOF, result);
		}

		public SCVMSimpleResult<CompactSingle> DivOF(CompactSingle R)
		{
			CompactSingle result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactSingle(Value / R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactSingle(Value / R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactSingle>(IsOF, result);
		}

		public SCVMSimpleResult<CompactSingle> MulOF(CompactSingle R)
		{
			CompactSingle result = default;
			bool IsOF = false;
			checked
			{
				try
				{
					result = new CompactSingle(Value * R.Value);
				}
				catch (System.Exception)
				{
					unchecked
					{
						IsOF = true;
						result = new CompactSingle(Value * R.Value);
					}
				}
			}
			return new SCVMSimpleResult<CompactSingle>(IsOF, result);
		}

		public bool LT(CompactSingle R)
		{
			return Value < R.Value;
		}

		public bool GT(CompactSingle R)
		{
			return Value > R.Value;
		}

		public bool LE(CompactSingle R)
		{
			return Value <= R.Value;
		}

		public bool GE(CompactSingle R)
		{
			return Value >= R.Value;
		}

		public bool EQ(CompactSingle R)
		{
			return Value == R.Value;
		}

		public bool NE(CompactSingle R)
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
			return new CompactUInt((uint)Value);
		}

		public INumbericData<CompactLong> Cast_Long()
		{
			return new CompactLong((long)Value);
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
			((float*)targetPtr)[0] = Value;
		}
		public int SizeOf()
		{
			return sizeof(float);
		}

		public CompactSingle Mod(CompactSingle R)
		{
			return new CompactSingle(Value % R.Value);
		}
	}

}

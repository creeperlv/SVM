using SVM.Core.Data;
using System;
using System.Runtime.CompilerServices;

namespace SVM.Core.FuncImpl
{
	public static unsafe class MathImpl
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MathAdd(Registers register, MState* state, SVMNativeTypes Type, int L, int R, int T, bool CheckOF)
		{
			switch (Type)
			{
				case SVMNativeTypes.Int8:
					GenericAdd<CompactSByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int16:
					GenericAdd<CompactShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int32:
					GenericAdd<CompactInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int64:
					GenericAdd<CompactLong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt8:
					GenericAdd<CompactByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt16:
					GenericAdd<CompactUShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt32:
					GenericAdd<CompactUInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt64:
					GenericAdd<CompactULong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Float:
					GenericAdd<CompactSingle>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Double:
					GenericAdd<CompactDouble>(register, state, L, R, T, CheckOF);
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MathSub(Registers register, MState* state, SVMNativeTypes Type, int L, int R, int T, bool CheckOF)
		{
			switch (Type)
			{
				case SVMNativeTypes.Int8:
					GenericSub<CompactSByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int16:
					GenericSub<CompactShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int32:
					GenericSub<CompactInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int64:
					GenericSub<CompactLong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt8:
					GenericSub<CompactByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt16:
					GenericSub<CompactUShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt32:
					GenericSub<CompactUInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt64:
					GenericSub<CompactULong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Float:
					GenericSub<CompactSingle>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Double:
					GenericSub<CompactDouble>(register, state, L, R, T, CheckOF);
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MathMod(Registers register, MState* state, SVMNativeTypes Type, int L, int R, int T)
		{
			switch (Type)
			{
				case SVMNativeTypes.Int8:
					GenericMod<CompactSByte>(register, state, L, R, T);
					break;
				case SVMNativeTypes.Int16:
					GenericMod<CompactShort>(register, state, L, R, T);
					break;
				case SVMNativeTypes.Int32:
					GenericMod<CompactInt>(register, state, L, R, T);
					break;
				case SVMNativeTypes.Int64:
					GenericMod<CompactLong>(register, state, L, R, T);
					break;
				case SVMNativeTypes.UInt8:
					GenericMod<CompactByte>(register, state, L, R, T);
					break;
				case SVMNativeTypes.UInt16:
					GenericMod<CompactUShort>(register, state, L, R, T);
					break;
				case SVMNativeTypes.UInt32:
					GenericMod<CompactUInt>(register, state, L, R, T);
					break;
				case SVMNativeTypes.UInt64:
					GenericMod<CompactULong>(register, state, L, R, T);
					break;
				case SVMNativeTypes.Float:
					GenericMod<CompactSingle>(register, state, L, R, T);
					break;
				case SVMNativeTypes.Double:
					GenericMod<CompactDouble>(register, state, L, R, T);
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MathMul(Registers register, MState* state, SVMNativeTypes Type, int L, int R, int T, bool CheckOF)
		{
			switch (Type)
			{
				case SVMNativeTypes.Int8:
					GenericMul<CompactSByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int16:
					GenericMul<CompactShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int32:
					GenericMul<CompactInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int64:
					GenericMul<CompactLong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt8:
					GenericMul<CompactByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt16:
					GenericMul<CompactUShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt32:
					GenericMul<CompactUInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt64:
					GenericMul<CompactULong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Float:
					GenericMul<CompactSingle>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Double:
					GenericMul<CompactDouble>(register, state, L, R, T, CheckOF);
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MathDiv(Registers register, MState* state, SVMNativeTypes Type, int L, int R, int T, bool CheckOF)
		{
			switch (Type)
			{
				case SVMNativeTypes.Int8:
					GenericDiv<CompactSByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int16:
					GenericDiv<CompactShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int32:
					GenericDiv<CompactInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Int64:
					GenericDiv<CompactLong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt8:
					GenericDiv<CompactByte>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt16:
					GenericDiv<CompactUShort>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt32:
					GenericDiv<CompactUInt>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.UInt64:
					GenericDiv<CompactULong>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Float:
					GenericDiv<CompactSingle>(register, state, L, R, T, CheckOF);
					break;
				case SVMNativeTypes.Double:
					GenericDiv<CompactDouble>(register, state, L, R, T, CheckOF);
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericAdd<N>(Registers Register, MState* state, int L, int R, int T, bool CheckOF) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			N TN = default;
			if (CheckOF)
			{
				var result = LN.AddOF(RN);
				TN = result.Value;
				state->OF = (byte)(result.IsSuccess ? 0 : 1);
			}
			else
			{
				TN = LN.Add(RN);
			}
			Console.WriteLine($"SVM:Add:{typeof(N)}{LN}+{RN}={TN.ToString()}");
			Register.SetData(T, TN);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericSub<N>(Registers Register, MState* state, int L, int R, int T, bool CheckOF) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			N TN = default;
			if (CheckOF)
			{
				var result = LN.SubOF(RN);
				TN = result.Value;
				state->OF = (byte)(result.IsSuccess ? 0 : 1);
			}
			else
			{
				TN = LN.Sub(RN);
			}
			Register.SetData<N>(T, TN);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericMul<N>(Registers Register, MState* state, int L, int R, int T, bool CheckOF) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			N TN = default;
			if (CheckOF)
			{
				var result = LN.MulOF(RN);
				TN = result.Value;
				state->OF = (byte)(result.IsSuccess ? 0 : 1);
			}
			else
			{
				TN = LN.Mul(RN);
			}
			Register.SetData<N>(T, TN);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericDiv<N>(Registers Register, MState* state, int L, int R, int T, bool CheckOF) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			N TN = default;
			if (CheckOF)
			{
				var result = LN.DivOF(RN);
				TN = result.Value;
				state->OF = (byte)(result.IsSuccess ? 0 : 1);
			}
			else
			{
				TN = LN.Div(RN);
			}
			Register.SetData<N>(T, TN);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericMod<N>(Registers Register, MState* state, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			Register.SetData<N>(T, LN.Mod(RN));
		}

	}
}

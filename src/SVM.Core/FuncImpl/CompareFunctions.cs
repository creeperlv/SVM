using SVM.Core.Data;
using System.Runtime.CompilerServices;

namespace SVM.Core.FuncImpl
{
	public unsafe static class CompareFunctions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LT(Registers context, SVMInstruction_InSeparateBytes inst)
		{
			var type = (NativeType)inst.D2;
			var L = inst.D3;
			var R = inst.D4;
			var T = inst.D4;

			switch (type)
			{
				case NativeType.BU:
					GenericLT<CompactByte>(context, L, R, T);
					break;
				case NativeType.BS:
					GenericLT<CompactSByte>(context, L, R, T);
					break;
				case NativeType.S:
					GenericLT<CompactShort>(context, L, R, T);
					break;
				case NativeType.SU:
					GenericLT<CompactUShort>(context, L, R, T);
					break;
				case NativeType.I:
					GenericLT<CompactInt>(context, L, R, T);
					break;
				case NativeType.IU:
					GenericLT<CompactUInt>(context, L, R, T);
					break;
				case NativeType.L:
					GenericLT<CompactLong>(context, L, R, T);
					break;
				case NativeType.LU:
					GenericLT<CompactULong>(context, L, R, T);
					break;
				case NativeType.F:
					GenericLT<CompactSingle>(context, L, R, T);
					break;
				case NativeType.D:
					GenericLT<CompactDouble>(context, L, R, T);
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GT(Registers context, SVMInstruction_InSeparateBytes inst)
		{
			var type = (NativeType)inst.D2;
			var L = inst.D3;
			var R = inst.D4;
			var T = inst.D4;

			switch (type)
			{
				case NativeType.BU:
					GenericGT<CompactByte>(context, L, R, T);
					break;
				case NativeType.BS:
					GenericGT<CompactSByte>(context, L, R, T);
					break;
				case NativeType.S:
					GenericGT<CompactShort>(context, L, R, T);
					break;
				case NativeType.SU:
					GenericGT<CompactUShort>(context, L, R, T);
					break;
				case NativeType.I:
					GenericGT<CompactInt>(context, L, R, T);
					break;
				case NativeType.IU:
					GenericGT<CompactUInt>(context, L, R, T);
					break;
				case NativeType.L:
					GenericGT<CompactLong>(context, L, R, T);
					break;
				case NativeType.LU:
					GenericGT<CompactULong>(context, L, R, T);
					break;
				case NativeType.F:
					GenericGT<CompactSingle>(context, L, R, T);
					break;
				case NativeType.D:
					GenericGT<CompactDouble>(context, L, R, T);
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LE(Registers context, SVMInstruction_InSeparateBytes inst)
		{
			var type = (NativeType)inst.D2;
			var L = inst.D3;
			var R = inst.D4;
			var T = inst.D4;

			switch (type)
			{
				case NativeType.BU:
					GenericLT<CompactByte>(context, L, R, T);
					break;
				case NativeType.BS:
					GenericLT<CompactSByte>(context, L, R, T);
					break;
				case NativeType.S:
					GenericLT<CompactShort>(context, L, R, T);
					break;
				case NativeType.SU:
					GenericLT<CompactUShort>(context, L, R, T);
					break;
				case NativeType.I:
					GenericLT<CompactInt>(context, L, R, T);
					break;
				case NativeType.IU:
					GenericLT<CompactUInt>(context, L, R, T);
					break;
				case NativeType.L:
					GenericLT<CompactLong>(context, L, R, T);
					break;
				case NativeType.LU:
					GenericLT<CompactULong>(context, L, R, T);
					break;
				case NativeType.F:
					GenericLT<CompactSingle>(context, L, R, T);
					break;
				case NativeType.D:
					GenericLT<CompactDouble>(context, L, R, T);
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GE(Registers context, SVMInstruction_InSeparateBytes inst)
		{
			var type = (NativeType)inst.D2;
			var L = inst.D3;
			var R = inst.D4;
			var T = inst.D4;

			switch (type)
			{
				case NativeType.BU:
					GenericGE<CompactByte>(context, L, R, T);
					break;
				case NativeType.BS:
					GenericGE<CompactSByte>(context, L, R, T);
					break;
				case NativeType.S:
					GenericGE<CompactShort>(context, L, R, T);
					break;
				case NativeType.SU:
					GenericGE<CompactUShort>(context, L, R, T);
					break;
				case NativeType.I:
					GenericGE<CompactInt>(context, L, R, T);
					break;
				case NativeType.IU:
					GenericGE<CompactUInt>(context, L, R, T);
					break;
				case NativeType.L:
					GenericGE<CompactLong>(context, L, R, T);
					break;
				case NativeType.LU:
					GenericGE<CompactULong>(context, L, R, T);
					break;
				case NativeType.F:
					GenericGE<CompactSingle>(context, L, R, T);
					break;
				case NativeType.D:
					GenericGE<CompactDouble>(context, L, R, T);
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EQ(Registers context, SVMInstruction_InSeparateBytes inst)
		{
			var type = (NativeType)inst.D2;
			var L = inst.D3;
			var R = inst.D4;
			var T = inst.D4;

			switch (type)
			{
				case NativeType.BU:
					GenericEQ<CompactByte>(context, L, R, T);
					break;
				case NativeType.BS:
					GenericEQ<CompactSByte>(context, L, R, T);
					break;
				case NativeType.S:
					GenericEQ<CompactShort>(context, L, R, T);
					break;
				case NativeType.SU:
					GenericEQ<CompactUShort>(context, L, R, T);
					break;
				case NativeType.I:
					GenericEQ<CompactInt>(context, L, R, T);
					break;
				case NativeType.IU:
					GenericEQ<CompactUInt>(context, L, R, T);
					break;
				case NativeType.L:
					GenericEQ<CompactLong>(context, L, R, T);
					break;
				case NativeType.LU:
					GenericEQ<CompactULong>(context, L, R, T);
					break;
				case NativeType.F:
					GenericEQ<CompactSingle>(context, L, R, T);
					break;
				case NativeType.D:
					GenericEQ<CompactDouble>(context, L, R, T);
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void NE(Registers context, SVMInstruction_InSeparateBytes inst)
		{
			var type = (NativeType)inst.D2;
			var L = inst.D3;
			var R = inst.D4;
			var T = inst.D4;

			switch (type)
			{
				case NativeType.BU:
					GenericNE<CompactByte>(context, L, R, T);
					break;
				case NativeType.BS:
					GenericNE<CompactSByte>(context, L, R, T);
					break;
				case NativeType.S:
					GenericNE<CompactShort>(context, L, R, T);
					break;
				case NativeType.SU:
					GenericNE<CompactUShort>(context, L, R, T);
					break;
				case NativeType.I:
					GenericNE<CompactInt>(context, L, R, T);
					break;
				case NativeType.IU:
					GenericNE<CompactUInt>(context, L, R, T);
					break;
				case NativeType.L:
					GenericNE<CompactLong>(context, L, R, T);
					break;
				case NativeType.LU:
					GenericNE<CompactULong>(context, L, R, T);
					break;
				case NativeType.F:
					GenericNE<CompactSingle>(context, L, R, T);
					break;
				case NativeType.D:
					GenericNE<CompactDouble>(context, L, R, T);
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericLT<N>(Registers Register, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			bool TN = LN.LT(RN);
			Register.SetData(T, TN ? 1 : 0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericGT<N>(Registers Register, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			bool TN = LN.GT(RN);
			Register.SetData(T, TN ? 1 : 0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericLE<N>(Registers Register, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			bool TN = LN.LE(RN);
			Register.SetData(T, TN ? 1 : 0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericGE<N>(Registers Register, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			bool TN = LN.GE(RN);
			Register.SetData(T, TN ? 1 : 0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericEQ<N>(Registers Register, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			bool TN = LN.EQ(RN);
			Register.SetData(T, TN ? 1 : 0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GenericNE<N>(Registers Register, int L, int R, int T) where N : unmanaged, INumbericData<N>
		{
			var LN = Register.GetData<N>(L);
			var RN = Register.GetData<N>(R);
			bool TN = LN.NE(RN);
			Register.SetData(T, TN ? 1 : 0);
		}
	}
}

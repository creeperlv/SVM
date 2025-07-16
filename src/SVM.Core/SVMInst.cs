namespace SVM.Core
{
	public enum SVMInst : byte
	{
		// 0	1		2		3		4
		// Add	[I]Type	[R]L	[R]R	[R]T
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		// Set Conditional Register to 1 if condition met.
		// 0	1		2		3		4
		// Cmp [R]Op	[R]L	[R]R	[R]T
		Cmp,
		// 0	1
		// JAL	RD
		// [I]Address (int32)
		JAL,
		// Jump And Link If Conditional Register is set.
		// JALF RD
		// [I]Address (int32)
		JALF,
		// 0	1			2		3
		// Load [R]Address	[I]Len	[R]T
		Load,
		Save,
		// 0 
		// Call
		// [I]Address (int64)
		Call,
		// Return
		Return,
		// System
		// [I]CallID (uint64)
		System,
		// 0	1	2			3			4			5
		// SIMD Op	[R]LAddr	[R]RAddr	[R]TAddr	[R]Len
		SIMD,
		// 0		1	2		3		4
		// AdvMath	Op	[R]L	[R]R	[R]T
		AdvMath,
	}
	public enum CmpOperator : byte
	{
		Eq, Ne, LT, GT, LE, GE
	}
	public enum SVMNativeTypes : byte
	{
		Int8,
		Int16,
		Int32,
		Int64,
		UInt8,
		UInt16,
		UInt32,
		UInt64,
		Float,
		Double,
	}
	public enum SIMDOperator : byte
	{
		Add, 
		Sub, 
		Mul, 
		Div,
	}
}

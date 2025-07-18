namespace SVM.Core
{
	public enum SVMInstDef : byte
	{
		// 0	1		2		3		4		5		6
		// Math	[I]Op	[I]Type	[R]L	[R]R	[R]T	[I]CheckOF
		BMath,
		// 0	1		2		3		4
		// Math	[I]Op	[I]Type	[R]L	[R]T
		UMath,
		//0		1			2			3		4
		//Cvt	[I]SType	[I]TType	[I]S	[I]T
		Cvt,
		// Set Conditional Register to 1 if condition met.
		// 0	1		2		3		4
		// Cmp [R]Op	[R]L	[R]R	[R]T
		Cmp,
		//Set Data to Register
		//SD T
		//Data (64-bit)
		SD,

		// 0	1
		// JAL	RD
		// [I]Address (int32)
		JAL,
		// Jump And Link If Conditional Register is set.
		// JALF RD FlagID
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
		// System [I]CallID (uint32)
		System,
		// 0	1	2			3			4			5
		// SIMD Op	[R]LAddr	[R]RAddr	[R]TAddr	[R]Len
		SIMD,
		//0		1		2
		//Alloc	[R]Size	[R]Pointer
		Alloc,
		//0		1		2			3
		//Alloc	[R]Size	[R]SPointer [R]TPointer
		Realloc,
		//0		1
		//Alloc	[R]Pointer
		Free,
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
	public enum ConditionFlag
	{
		/// <summary>
		/// Compare Flag
		/// </summary>
		CF,
		/// <summary>
		/// Overflow Flag
		/// </summary>
		Of,
	}
	public enum SIMDOperator : byte
	{
		Add,
		Sub,
		Mul,
		Div,
	}
	public enum BMathOp : byte
	{
		Add,
		Sub,
		Mul,
		Div,
		Mod,
	}
	public enum UMathOp : byte
	{
		Sin,
		Cos,
		Tan,
		Log,
		Sqrt
	}
}

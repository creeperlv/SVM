namespace SVM.Core
{
	public enum PrimaryInstruction : byte
	{
		Nop,
		// 0	1		2		3		4		5
		// Math	[I]Op	[I]Type	[R]L	[R]R	[R]T
		BMath,
		/// <summary>
		/// Checked Binary Math
		/// </summary>
		CBMath,
		// 0	1		2		3		4
		// Math	[I]Op	[I]Type	[R]L	[R]T
		UMath,
		//0		1			2			3		4
		//Cvt	[I]SType	[I]TType	[I]S	[I]T
		Cvt,
		// Set Conditional Register to 1 if condition met.
		// 0	1		2		3		4		5
		// Cmp [R]Op	[I]Type	[R]L	[R]R	[R]T
		Cmp,
		//Set Data to Register
		//SD T
		//Data (64-bit)
		SD,

		// 0	1
		// JMP	[R]TargetPC
		JMP,
		// Jump If Conditional Register is set.
		// 0	1			2
		// JIF [R]TargetPC	[I]FlagID
		JIF,
		// 0	1			2		3
		// Load [R]Address	[I]Len	[R]T
		Load,
		// 0	1		2		3
		// Load [R]Src	[I]Len	[R]TAddr
		Save,
		// 0	1
		// Call [R]Address
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

	public enum NativeType : byte
	{
		BS = SVMNativeTypes.Int8,
		BU = SVMNativeTypes.UInt8,
		S = SVMNativeTypes.Int16, 
		SU = SVMNativeTypes.UInt16,
		I = SVMNativeTypes.Int32,
		IU = SVMNativeTypes.UInt32,
		L = SVMNativeTypes.Int64,
		LU = SVMNativeTypes.UInt64,
		F = SVMNativeTypes.Float, 
		D = SVMNativeTypes.Double, 
		R
	}
	public enum ConditionFlag : byte
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
	public enum CompareOperator : byte
	{
		LT = 0, GT = 1, GE = 2, EQ = 3, LE = 4, NE = 5,
	}
}

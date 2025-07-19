using SVM.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using static SVM.Core.stdc.stdlib;
namespace SVM.Core
{
	public unsafe class ManagedSVMProgram
	{
		public List<SVMInstruction> instructions = new List<SVMInstruction>();
		public List<byte> Datas = new List<byte>();
	}
	public unsafe struct SVMProgram : IDisposable
	{
		public UInt64 InstructionCount;
		public UInt64 DataSize;
		public SVMInstruction* instructions;
		public byte* data;
		public static SVMProgram* LoadFromStream(Stream stream)
		{
			if (!stream.TryReadData(out uint dataSectionLength))
			{
				return null;
			}
			if (!stream.TryReadData(out uint codeCount))
			{
				return null;
			}
			var dataPtr = malloc(dataSectionLength);
			Span<byte> dataBuffer = new Span<byte>((byte*)dataPtr, (int)dataSectionLength);
			if (stream.Read(dataBuffer) != dataSectionLength)
			{
				free(dataPtr);
				return null;
			}
			var codeLen = codeCount * sizeof(SVMInstruction);
			var codePtr = malloc((uint)codeLen);
			Span<byte> codeBuffer = new Span<byte>((byte*)codePtr, (int)codeLen);
			if (stream.Read(codeBuffer) != codeLen)
			{
				free(dataPtr);
				free(codePtr);
				return null;
			}
			var program = (SVMProgram*)malloc(sizeof(SVMProgram));
			program->data = (byte*)dataPtr;
			program->DataSize = dataSectionLength;
			program->instructions = (SVMInstruction*)codePtr;
			program->InstructionCount = codeCount;
			return program;
		}

		public void Dispose()
		{
			free((IntPtr)data);
			free((IntPtr)instructions);
		}
	}
}

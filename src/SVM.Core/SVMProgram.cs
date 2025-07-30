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
		public byte[]? Datas;
		public void WriteToStream(Stream stream)
		{
			stream.WriteData(Datas?.Length ?? 0);
			stream.WriteData(instructions.Count);
			if (Datas != null)
				stream.Write(Datas);
			foreach (SVMInstruction instruction in instructions)
			{
				stream.WriteData(instruction);
			}
		}
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
			Console.WriteLine(dataSectionLength);
			Console.WriteLine(codeCount);
			IntPtr dataPtr = malloc(dataSectionLength);
			if (dataSectionLength != 0)
			{

				Span<byte> dataBuffer = new Span<byte>((byte*)dataPtr, (int)dataSectionLength);
				if (stream.Read(dataBuffer) != dataSectionLength)
				{
					free(dataPtr);
					return null;
				}
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
			program->data = dataSectionLength == 0 ? null : (byte*)dataPtr;
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

using SVM.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SVM.Assembler.Core
{
	[Serializable]
	public class ISADefinition
	{
		public Dictionary<PrimaryInstruction, InstructionDefinition> InstructionDefinitions = new Dictionary<PrimaryInstruction, InstructionDefinition>();
		[NonSerialized]
		public Dictionary<string, InstructionDefinition> InstructionDefinitionAliases = new Dictionary<string, InstructionDefinition>();
		public void Init()
		{
			foreach (var item in InstructionDefinitions)
			{
				foreach (var alias in item.Value.aliases)
				{
					if (!InstructionDefinitionAliases.TryAdd(alias, item.Value))
					{
						InstructionDefinitionAliases[alias] = item.Value;
					}
				}
			}
		}
		static void PrintDepth(int depth)
		{
			for (int i = 0; i < depth; i++)
			{
				Console.Write("\t");
			}
		}
		static void ShowNode(XmlNode node, int depth = 0)
		{
			PrintDepth(depth);
			Console.WriteLine($"[+]{node.NodeType}:{node.Name}");
			foreach (XmlAttribute item in node.Attributes)
			{
				PrintDepth(depth + 1);
				Console.WriteLine($"[i]{item.NodeType}:{item.Name}={item.InnerText}");

			}
			foreach (XmlElement item in node.ChildNodes)
			{
				if (item is XmlNode cnode)
				{
					ShowNode(cnode, depth + 1);
				}
				else
				{
					PrintDepth(depth + 1);
					Console.Write($"[?]{item.NodeType}:{item.Name}");
				}
			}
		}
		public static bool TryParse(Stream inputStream, [MaybeNullWhen(false)] out ISADefinition definition)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(inputStream);
			foreach (XmlNode item in xmlDocument.ChildNodes)
			{
				ShowNode(item, 0);
			}
			definition = null;
			return false;
		}
	}
}

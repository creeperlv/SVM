using SVM.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Linq;
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
				foreach (var alias in item.Value.Aliases)
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
		static bool ParseDefinition(XmlNode node, ref ISADefinition definition)
		{

			InstructionDefinition instDefinition = new InstructionDefinition();
			foreach (XmlNode item in node.ChildNodes)
			{
				switch (item.Name)
				{
					case "Aliases":
						foreach (XmlNode aliasNode in item.ChildNodes)
						{
							if (aliasNode.Name == "Alias")
							{
								instDefinition.Aliases.Add(aliasNode.Attributes["Name"].Value);
							}
							else
							{
								return false;
							}
						}
						break;
					case "Parameters":

						break;
					default:
						break;
				}
				if (item.Name != "InstructionDefinition")
				{
					return false;
				}
			}
			return true;
		}
		static bool ParseDefinitions(XmlNode node, ref ISADefinition definition)
		{
			foreach (XmlNode item in node.ChildNodes)
			{
				if (item.Name != "InstructionDefinition")
				{
					return false;
				}
				if (ParseDefinition(node, ref definition) == false)
				{
					return false;
				}
			}
			return true;
		}
		public static bool TryParse(Stream inputStream, [MaybeNullWhen(false)] out ISADefinition definition)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(inputStream);
			ISADefinition isaDefinition = new ISADefinition();
			foreach (XmlNode item in xmlDocument.ChildNodes)
			{
				ShowNode(item, 0);
				switch (item.Name)
				{
					case "Enums":
						break;
					case "Definitions":
						if (ParseDefinitions(item, ref isaDefinition) == false)
						{
							definition = null;
							return false;
						}
						break;
					default:
						break;
				}
			}
			definition = null;
			return false;
		}
	}
}

using SVM.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public Dictionary<string, byte> RegisterNames = new Dictionary<string, byte>();
		public Dictionary<string, Dictionary<string, string>> Enums = new Dictionary<string, Dictionary<string, string>>();
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
				Trace.Write("\t");
			}
		}
		static void ShowNode(XmlNode node, int depth = 0)
		{
			PrintDepth(depth);
			Trace.WriteLine($"[+]{node.NodeType}:{node.Name}");
			foreach (XmlAttribute item in node.Attributes)
			{
				PrintDepth(depth + 1);
				Trace.WriteLine($"[i]{item.NodeType}:{item.Name}={item.InnerText}");

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
					Trace.Write($"[?]{item.NodeType}:{item.Name}");
				}
			}
		}
		static bool ParseParameter(XmlNode node, ref InstructionDefinition instruction)
		{
			Trace.WriteLine("Parse:Parameter");
			InstructionParameter parameter = new InstructionParameter();
			foreach (XmlNode subNode in node)
			{
				switch (subNode.Name)
				{
					case "MatchingItems":
						foreach (XmlNode item in subNode.ChildNodes)
						{
							if (item.Name == "Item")
							{
								var result = item.Attributes.GetNamedItem("Id");
								if (result == null) return false;
								Trace.WriteLine($"Item:{result.InnerText}");
								parameter.AllowedTokenIds.Add(result.InnerText);
							}
						}
						break;
					case "ExpectedValue":
						{
							var TypeAttr = subNode.Attributes.GetNamedItem("Type");
							var PosAttr = subNode.Attributes.GetNamedItem("Pos");
							var ConverterAttr = subNode.Attributes.GetNamedItem("Converter");
							if (TypeAttr == null) return false;
							if (PosAttr == null) return false;
							if (ConverterAttr == null) return false;
							if (!Enum.TryParse<SVMNativeTypes>(TypeAttr.InnerText, out var nType))
							{
								Trace.WriteLine($"ParseSVMNativeTypes:{TypeAttr.InnerText}");
								return false;
							}
							if (!int.TryParse(PosAttr.InnerText, out var pos))
							{
								Trace.WriteLine($"ParseInt:{PosAttr.InnerText}");
								return false;
							}
							parameter.ExpectdValue.Type = nType;
							parameter.ExpectdValue.Pos = pos;
							parameter.ExpectdValue.Converter = ConverterAttr.InnerText;
						}
						break;
					default:
						return false;
				}
			}
			instruction.ParameterPattern.Add(parameter);
			return true;
		}
		static bool ParseDefinition(XmlNode node, ref ISADefinition definition)
		{
			Trace.WriteLine($"ParseDefinition:{node.Name}");
			InstructionDefinition instDefinition = new InstructionDefinition();
			var PIAttr = node.Attributes.GetNamedItem("PrimaryInstruction");
			if (PIAttr == null) return false;
			if (!Enum.TryParse<PrimaryInstruction>(PIAttr.InnerText, out var pi))
			{
				return false;
			}
			instDefinition.PrimaryInstruction = pi;
			foreach (XmlNode item in node.ChildNodes)
			{
				Trace.WriteLine($"{item.Name}");
				switch (item.Name)
				{
					case "Aliases":
						foreach (XmlNode aliasNode in item.ChildNodes)
						{
							Trace.WriteLine($"Aliases->{aliasNode.Name}");
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
						foreach (XmlNode parameterNode in item.ChildNodes)
						{
							Trace.WriteLine($"Parameters->{parameterNode.Name}");
							if (parameterNode.Name == "InstructionParameter")
							{
								if (!ParseParameter(parameterNode, ref instDefinition))
								{
									return false;
								}
							}
							else
							{
								return false;
							}
						}
						break;
					default:
						Trace.WriteLine($"???{item.Name}");
						break;
				}
			}
			definition.InstructionDefinitions.Add(pi, instDefinition);
			foreach (var item in instDefinition.Aliases)
			{

				definition.InstructionDefinitionAliases.Add(item, instDefinition);
			}
			return true;
		}
		static bool ParseDefinitions(XmlNode node, ref ISADefinition definition)
		{
			Trace.WriteLine("Parse:Definitions");
			foreach (XmlNode item in node.ChildNodes)
			{
				if (item.Name != "InstructionDefinition")
				{
					Trace.WriteLine($"Not Matching:{item.Name}");

					return false;
				}
				if (ParseDefinition(item, ref definition) == false)
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
			foreach (XmlNode rootNode in xmlDocument.ChildNodes)
			{
				//ShowNode(rootNode, 0);
				if (rootNode.Name == "ISARoot")
				{
					foreach (XmlNode item in rootNode)
					{

						switch (item.Name)
						{
							case "Enums":
								{
									foreach (XmlNode enumNode in item.ChildNodes)
									{
										if (enumNode.Name == "Enum")
										{
											Dictionary<string, string> enumItem = new Dictionary<string, string>();
											var EnumNameAttr = enumNode.Attributes.GetNamedItem("Name");
											if (EnumNameAttr == null)
											{
												definition = null;
												return false;
											}
											foreach (XmlNode enumItemNode in enumNode.ChildNodes)
											{

												if (enumItemNode.Name == "Item")
												{

													var keyAttr = enumItemNode.Attributes.GetNamedItem("Key");
													var valueAttr = enumItemNode.Attributes.GetNamedItem("Value");
													if (keyAttr == null || valueAttr == null)
													{
														definition = null;
														return false;
													}
													enumItem.Add(keyAttr.InnerText, valueAttr.InnerText);
												}
												else
												{
													definition = null;
													return false;
												}
											}
											isaDefinition.Enums.Add(EnumNameAttr.InnerText, enumItem);
										}
										else
										{
											definition = null;
											return false;
										}
									}
								}
								break;
							case "Registers":
								{
									foreach (XmlNode enumNode in item.ChildNodes)
									{
										if (enumNode.Name == "Item")
										{

											var keyAttr = enumNode.Attributes.GetNamedItem("Key");
											var valueAttr = enumNode.Attributes.GetNamedItem("Value");
											if (keyAttr == null || valueAttr == null)
											{
												definition = null;
												return false;
											}
											if (!byte.TryParse(valueAttr.InnerText, out var RegID))
											{
												definition = null;
												return false;
											}
											isaDefinition.RegisterNames.Add(keyAttr.InnerText, RegID);
										}
										else
										{
											definition = null;
											return false;
										}

									}
								}
								break;
							case "Definitions":
								if (ParseDefinitions(item, ref isaDefinition) == false)
								{
									definition = null;
									return false;
								}
								break;
							default:
								Trace.WriteLine("Unknown Node!");
								break;
						}
					}
				}
			}
			definition = isaDefinition;
			return true;
		}
	}
}

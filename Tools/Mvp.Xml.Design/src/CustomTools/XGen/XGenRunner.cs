using System;
using System.IO;

namespace Mvp.Xml.Design.CustomTools.XGen
{
	/// <summary>
	/// Class that performs the actual code generation in the isolated design domain.
	/// </summary>
	internal class XGenRunner : MarshalByRefObject
	{
		/// <summary>
		/// Generates the code for the received type.
		/// </summary>
		public XGenRunner(string outputFile, string[] forTypes, string targetNamespace)
		{
			using (StreamWriter writer = new StreamWriter(outputFile))
			{
				Type[] types = new Type[forTypes.Length];
				for (int i = 0; i < forTypes.Length; i++)
				{
					types[i] = Type.GetType(forTypes[i], true);
				}

				writer.Write(XmlSerializerGenerator.GenerateCode(
					types, targetNamespace));
			}
		}
	}
}

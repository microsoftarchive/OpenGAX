//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Extensions
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

using System;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.RecipeFramework.Configuration.Tests
{
	[TestClass]
	[DeploymentItem("GuidancePackageConfig.xsd")]
	[DeploymentItem("smallconfig.xml")]
	public class XmlSerializerTests
	{
		[TestMethod]
		public void SerializeKeepsFixedValueAttribute()
		{
			// Regression for bug: http://lab.msdn.microsoft.com/ProductFeedback/viewfeedback.aspx?feedbackid=e9fa2f82-42cd-4d48-90b1-35211c8913d7
			XmlSerializer serializer = new XmlSerializer(typeof(GuidancePackage));

			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.ValidationType = ValidationType.Schema;
			readerSettings.Schemas.Add(XmlSchema.Read(new XmlTextReader("GuidancePackageConfig.xsd"), null));

			GuidancePackage package;
			using (XmlReader reader = XmlReader.Create("smallconfig.xml", readerSettings))
			{
				package = (GuidancePackage)serializer.Deserialize(reader);
			}

			Assert.AreEqual("1.0", package.SchemaVersion);

			MemoryStream mem = new MemoryStream();
			serializer.Serialize(mem, package);
			mem.Position = 0;

			GuidancePackage package2 = (GuidancePackage)serializer.Deserialize(mem);

			Assert.AreEqual(package.SchemaVersion, package2.SchemaVersion);
		}
	}
}

#region Using directives

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Mvp.Xml.XInclude;
using Microsoft.Practices.RecipeFramework.Configuration.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.WizardFramework.Configuration;

#endregion

namespace Microsoft.Practices.RecipeFramework.Configuration.Tests
{
	[TestClass]
	[DeploymentItem("help.xml")]
	[DeploymentItem("wizard.xml")]
	[DeploymentItem("wizardtypes.xml")]
	[DeploymentItem("types.xml")]
	[DeploymentItem("recipe.xml")]
	[DeploymentItem("mainconfig.xml")]
	[DeploymentItem("smallconfig.xml")]
	[DeploymentItem("GuidancePackageConfig.xsd")]
	public class IncludeTests
	{
		private string baseDirectory = @".\";

		private string BaseDir
		{
			get
			{
				return baseDirectory;
			}
		}

		[TestMethod]
		//[Ignore("XmlSerialization doesn't work on dynamic AppDomains")]
		public void RecipeLoad()
		{
			XIncludingReader ir = new XIncludingReader(new XmlTextReader(BaseDir + "recipe.xml"));
			XmlSerializer ser = new XmlSerializer(typeof(Recipe));
			Recipe rec = (Recipe)ser.Deserialize(ir);
			// Two types should have been included.
			Assert.AreEqual(2, rec.Types.Length);
		}

		[TestMethod]
		//[Ignore("XmlSerialization doesn't work on dynamic AppDomains")]
		public void WizardLoad()
		{
			XIncludingReader ir = new XIncludingReader(BaseDir + "wizard.xml");
			ir.MoveToContent();
			Console.WriteLine(ir.ReadOuterXml());
			ir = new XIncludingReader(BaseDir + "wizard.xml");

			XmlSerializer ser = new XmlSerializer(typeof(Wizard));
			Wizard wz = (Wizard)ser.Deserialize(ir);

			ir.Close();

			Assert.IsNotNull(wz.Types);
			Assert.AreEqual(2, wz.Types.Length);
		}

		[TestMethod]
		//[Ignore("XInclude migration in progress.")]
        public void FullLoad()
		{
			XIncludingReader ir = new XIncludingReader(new XmlTextReader(BaseDir + "mainconfig.xml"));

			// Dump for debugging purposes.
			XmlTextWriter tw = new XmlTextWriter(Console.Out);
			tw.WriteNode(ir, false);
			ir = new XIncludingReader(new XmlTextReader(BaseDir + "mainconfig.xml"));

			XmlSerializer ser = new GuidancePackageSerializer();
			GuidancePackage package = (GuidancePackage)ser.Deserialize(ir);
			// Check included recipe
			Assert.AreEqual("ReusableRecipe", package.Recipes[0].Name);
			// Check included types in included recipe
			Assert.AreEqual("Page", package.Recipes[0].Types[1].Name);
			// Check included wizard
			Assert.IsNotNull(package.Recipes[0].GatheringServiceData);

			XmlSerializer wzser = new XmlSerializer(typeof(Wizard));
			Wizard wz = (Wizard)wzser.Deserialize(new XmlNodeReader(package.Recipes[0].GatheringServiceData.Any));

			Assert.AreEqual("ExistingWizard", wz.Name);
		}

        [TestMethod]
		//[Ignore("Validation with inclusions fails as per bug #")]
        public void FullLoadAndValidation()
		{
			XIncludingReader ir = new XIncludingReader(BaseDir + "mainconfig.xml");
			XmlReaderSettings validateReaderSettings = new XmlReaderSettings();
			validateReaderSettings.ValidationType = ValidationType.Schema;
			validateReaderSettings.Schemas.Add(XmlSchema.Read(
				new XmlTextReader("GuidancePackageConfig.xsd"), null));

			// Dump for debugging purposes.
			XmlReader vr = XmlReader.Create(ir, validateReaderSettings);
			XmlWriter tw = XmlWriter.Create(Console.Out);
			tw.WriteNode(ir, false);

			// Read into memory with full inclusions as workaround
			ir = new XIncludingReader(BaseDir + "mainconfig.xml");
			MemoryStream mem = new MemoryStream();
			vr = XmlReader.Create(ir, validateReaderSettings);
			tw = XmlWriter.Create(mem);
			tw.WriteNode(ir, false);
			tw.Flush();
			mem.Position = 0;

			vr = XmlReader.Create(mem, validateReaderSettings);
			XmlSerializer ser = new GuidancePackageSerializer();
			
			// Deserialize with validating reader
			GuidancePackage package = (GuidancePackage)ser.Deserialize(vr);
			// Check included recipe
			Assert.AreEqual("ReusableRecipe", package.Recipes[0].Name);
			// Check included types in included recipe
			Assert.AreEqual("Page", package.Recipes[0].Types[1].Name);

			XmlSerializer wzser = new XmlSerializer(typeof(Wizard));
			Wizard wz = (Wizard)wzser.Deserialize(new XmlNodeReader(package.Recipes[0].GatheringServiceData.Any));

			Assert.AreEqual("ExistingWizard", wz.Name);
		}

        [TestMethod]
		public void LoadNoValidateOrInclusions()
		{
			XIncludingReader ir = new XIncludingReader(new XmlTextReader(BaseDir + "smallconfig.xml"));
			XmlReaderSettings validateReaderSettings = new XmlReaderSettings();
			// Add the schema to validate against.
			validateReaderSettings.ValidationType = ValidationType.Schema;
			validateReaderSettings.Schemas.Add("http://schemas.microsoft.com/pag/gax-core", "GuidancePackageConfig.xsd");
			XmlReader vr = XmlReader.Create(ir, validateReaderSettings);

			// Deserialize with validating reader.
			XmlSerializer ser = new GuidancePackageSerializer();
			GuidancePackage package = (GuidancePackage)ser.Deserialize(vr);
			// Done!
		}
	}
}
using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Common.Services;
using CM = Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests.Mocks;
using Microsoft.Practices.RecipeFramework.Library.TextTemplate;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.RecipeFramework.Services;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests
{
    /// <summary>
    /// Test Microsoft.Practices.RecipeFramework.Library.TextTemplate class.
    /// </summary>
    [TestClass]
	[DeploymentItem(@"TextTemplate\", @"TextTemplate\")]
	[DeploymentItem(@"TestTemplates\", @"Templates\")]
	public class TextTemplateActionFixture 
    {
		private static CM.ServiceContainer sc = null;

		[TestInitialize]
		public void SetUp()
		{
			sc = new CM.ServiceContainer();

			// Add TypeResolutionService
			TypeResolutionService typeResolutionService = new TypeResolutionService(
				AppDomain.CurrentDomain.BaseDirectory);
			sc.AddService(typeof(ITypeResolutionService), typeResolutionService);
			sc.AddService(typeof(IConfigurationService), new MockConfigurationService(AppDomain.CurrentDomain.BaseDirectory));

			// Add ValueInfoService
			IValueInfoService valueInfoService = new MockValueInfoService();
			sc.AddService(typeof(IValueInfoService), valueInfoService);
		}

		// Null Template passed to Microsoft.Practices.RecipeFramework.Library.TextTemplateAction
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteWithNullTemplate()
        {
            // fields
            string templateName = null;

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
            action.Template = templateName;
            sc.Add(action);

            // Execute
            action.Execute();
        }

        // Uses a file path that is out of the package root.
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteWithWrongRootedFilePath()
        {
            // fields
            string templateName = @"C:\ValidTemplate1cs";

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
            action.Template = templateName + ".ipe";
            sc.Add(action);

            // Execute
            action.Execute();
        }

        // Uses a TemplatePath that is out of the package root.
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteWithWrongRootedTemplatePath()
        {
            // fields
            string templateName = "ValidTemplate1cs";

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
            action.Template = "C:\\" + templateName + ".ipe";
            sc.Add(action);

            // Execute
            action.Execute();
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.Execute() with a simple valid template
        [TestMethod]
        public void ExecuteSimpleTemplate1cs()
        {
            // fields
            string templateName = "ValidTemplate1cs";
            string result = null;

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
            Assert.AreEqual(expected, result);
        }

        // Test for template without ValueInfoService
        [TestMethod]
        public void ExecuteSimpleTemplate1csWithoutValueInfoService()
        {
            // fields
            string templateName = "ValidTemplate1cs";

			sc.RemoveService(typeof(IValueInfoService));

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
        }

        // Test Properties generator with a simple valid template
        // using 2 string arguments: NameSpace and ClassName
        // and template language="C#"
        [TestMethod]
        public void TestValidTemplateProperties1cs()
        {
            // fields
            string templateName = "ValidTemplateProperties1cs";
            string result = null;

            // Create Action and set Properties
            TextTemplateAction action = new TextTemplateAction();

            TypeDescriptor.GetProperties(action)["NameSpace"].SetValue(action, "TestNameSpace");
            TypeDescriptor.GetProperties(action)["ClassName"].SetValue(action, "TestClass");

			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Properties generator with a simple valid template
        // using 2 string arguments: NameSpace and ClassName
        // and template language="VB"
        [TestMethod]
        public void TestValidTemplateProperties1vb()
        {
            // fields
            string templateName = "ValidTemplateProperties1vb";
            string result = null;

            // Create Action and set Properties
            TextTemplateAction action = new TextTemplateAction();

            TypeDescriptor.GetProperties(action)["NameSpace"].SetValue(action, "TestNameSpace");
            TypeDescriptor.GetProperties(action)["ClassName"].SetValue(action, "TestClass");

			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Properties generator with a simple valid template
        // using 2 string arguments and 1 Decimal: NameSpace, ClassName and Amount
        // and template language="C#"
        [TestMethod]
        public void TestValidTemplateProperties2cs()
        {
            // fields
            string templateName = "ValidTemplateProperties2cs";
            string result = null;

            // Create Action and set Properties
            TextTemplateAction action = new TextTemplateAction();

            TypeDescriptor.GetProperties(action)["NameSpace"].SetValue(action, "TestNameSpace");
            TypeDescriptor.GetProperties(action)["ClassName"].SetValue(action, "TestClass");
            TypeDescriptor.GetProperties(action)["Amount"].SetValue(action, (decimal)1003.75);

			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Properties generator with a simple valid template
        // using a List<string>: ClientName
        // and template language="C#"
        [TestMethod]
        public void TestValidTemplateProperties3csEnum()
        {
            // fields
            string templateName = "ValidTemplateProperties3cs";
            string result = null;

            // Create Action and set Properties
            TextTemplateAction action = new TextTemplateAction();

            List<string> colorNames = new List<string>();
            colorNames.Add("Red");
            colorNames.Add("Blue");
            colorNames.Add("Green");
            TypeDescriptor.GetProperties(action)["NameSpace"].SetValue(action, "TestNameSpace");
            TypeDescriptor.GetProperties(action)["ClassName"].SetValue(action, "TestClass");
            TypeDescriptor.GetProperties(action)["ColorNames"].SetValue(action, colorNames);

			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Properties generator with a simple valid template
        // using MockColor object
        // and template language="C#"
        [TestMethod]
        public void TestValidTemplateProperties4cs()
        {
            // fields
            string templateName = "ValidTemplateProperties4cs";
            string result = null;

            // Create Action and set Properties
            TextTemplateAction action = new TextTemplateAction();

            MockColors mockColors = new MockColors();
            TypeDescriptor.GetProperties(action)["NameSpace"].SetValue(action, "TestNameSpace");
            TypeDescriptor.GetProperties(action)["ClassName"].SetValue(action, "TestClass");
            TypeDescriptor.GetProperties(action)["MockColors"].SetValue(action, mockColors);

			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
			try
			{
				action.Execute();

			}
			catch
			{	
				throw;
			} 
			
			result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.Execute() 
        // using template with DirectiveProcessor include
        [TestMethod]
        public void ExecuteSimpleTemplateInclude1cs()
        {
            // fields
            string templateName = "ValidTemplateInclude1cs";

			// Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            string result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.Execute() 
        // using template with DirectiveProcessor import, assembly
        [TestMethod]
        public void ExecuteSimpleTemplateImportAssembly1cs()
        {
            // fields
            string templateName = "ValidTemplateImportAssembly1cs";
            string result = null;

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
            result = action.Content;

            // Evaluate results
			string expected = GetExpectedContent(templateName);
			Assert.AreEqual(expected, result);
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.Execute() 
        // using template with DirectiveProcessor import, assembly
        // iterating inside the template code.
        [TestMethod] 
        public void ExecuteSimpleTemplateImportAssembly2cs()
        {
            // fields
            string templateName = "ValidTemplateImportAssembly2cs";

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.Execute() 
        // using an invalid template code
        [TestMethod]
        [ExpectedException(typeof(TemplateException))]
        public void ExecuteInvalidTemplate1cs()
        {
            // fields
            string templateName = "InvalidTemplate1cs";

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
			sc.Add(action);

            // Execute
            action.Execute();
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.Execute() 
        // using a Microsoft.Practices.RecipeFramework.Library.TextTemplate that not exist
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ExecuteTemplateThatNotExist()
        {
            // fields
            string templateName = "TemplatThatNotExist";

            // Create Action
            TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName(templateName);
            sc.Add(action);

            // Execute
            action.Execute();
        }

		[TestMethod]
		public void AddsPropertyForDynamicValues()
		{
			TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName("AddsPropertyForDynamicValues");
			sc.Add(action);
			DateTime dt = DateTime.Now;

			TypeDescriptor.GetProperties(action)["DynamicValue"].SetValue(action, dt);
			action.Execute();

			Assert.AreEqual(dt.ToString(CultureInfo.InvariantCulture), action.Content);
		}

		[TestMethod]
		[ExpectedException(typeof(TemplateException))]
		public void ThrowsIfTemplatePropertyNotExistsAsValue()
		{
			TextTemplateAction action = new TextTemplateAction();
			action.Template = GetTemplateFileName("ThrowsIfTemplatePropertyNotExistsAsValue");
			sc.Add(action);

			action.Execute();
		}

		private static string GetTemplateFileName(string templateName)
		{
			return "TextTemplate\\" + templateName + ".ipe";
		}

		private static string GetExpectedContent(string templateName)
		{
			return File.OpenText(@"TextTemplate\TemplatesResults\" + templateName + ".txt").ReadToEnd();
		}
    }

    #region Mock Class
    [Serializable]
    public class MockColors
    {
        private List<string> colorsNames = new List<string>();
        private List<int> colorsNumbers = new List<int>();
        public MockColors()
        {
            colorsNames.Add("Red");
            colorsNames.Add("Blue");
            colorsNames.Add("Green");
            colorsNumbers.Add(7);
            colorsNumbers.Add(3);
            colorsNumbers.Add(8);
        }
        public List<string> Names
        {
            get { return this.colorsNames; }
        }
        public List<int> Numbers
        {
            get { return this.colorsNumbers; }
        }
    }
    #endregion
}

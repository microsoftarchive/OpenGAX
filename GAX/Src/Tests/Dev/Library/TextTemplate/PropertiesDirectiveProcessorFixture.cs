using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TextTemplating;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using Microsoft.Practices.RecipeFramework.Library.TextTemplate;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using System.ComponentModel;
using System.Drawing.Design;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests
{
    /// <summary>
    /// Test PropertiesDirectiveProcessor
    /// </summary>
    [TestClass]
    public class PropertiesDirectiveProcessorFixture
    {
        [TestMethod]
        public void CanCreatePropertiesDirectiveProcessor()
        {
            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            Assert.IsNotNull(pdp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeHostIsNull()
        {
            // fields
            MockTemplateHost mockHost = null;
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.Initialize(mockHost);
        }

        [TestMethod]
        public void Initialize()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.Initialize(mockHost);
        }

        [TestMethod]
        public void PropertiesDirectiveProcessorIsDirectiveProcessor()
        {
            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            Assert.IsTrue(pdp is DirectiveProcessor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsDirectiveSupportedWithNull()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string directiveName = null;
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.Initialize(mockHost);

            bool result = pdp.IsDirectiveSupported(directiveName);
        }

        [TestMethod]
        public void IsDirectiveSupported()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string directiveName = "property";
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.Initialize(mockHost);

            Assert.IsTrue(pdp.IsDirectiveSupported(directiveName));
        }

        [TestMethod]
        public void IsDirectiveSupportedWithWrongDirectiveName()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string directiveName = "propertyThetNorExist";
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.Initialize(mockHost);

            Assert.IsFalse(pdp.IsDirectiveSupported(directiveName));
        }

        [TestMethod]
        public void StartProcessingRun()
        {
            // fields
            CodeDomProvider languageProvider = new CSharpCodeProvider();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.StartProcessingRun(languageProvider, null, new CompilerErrorCollection());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartProcessingRunWithNull()
        {
            // fields
            CodeDomProvider languageProvider = null;

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.StartProcessingRun(languageProvider, null, new CompilerErrorCollection());
        }

        [TestMethod]
        public void ProcessDirective()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string directiveName = "property";
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            CodeDomProvider languageProvider = new CSharpCodeProvider();

            IDictionary<string, string> arguments = new Dictionary<string, string>(0);
            PropertyData propertyData = new PropertyData("ClientName", typeof(string));
            mockHost.Arguments.Add("Client", propertyData);
            arguments.Add("name", "Client");

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider, null, new CompilerErrorCollection());
            pdp.ProcessDirective(directiveName, arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProcessDirectiveDirectiveNameIsNull()
        {
            // fields
            string directiveName = null;
            IDictionary<string, string> arguments = new Dictionary<string, string>(0);

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.ProcessDirective(directiveName, arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProcessDirectiveArgumentsIsNull()
        {
            // fields
            string directiveName = "property";
            IDictionary<string, string> arguments = null;

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            pdp.ProcessDirective(directiveName, arguments);
        }

        [TestMethod]
        public void GetClassCodeForProcessingRun()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider, null, new CompilerErrorCollection());
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();

            Assert.IsTrue(result == "");
        }

        // Try to get the results without invoke the StartProcessingRun Method
        [TestMethod]
        public void GetClassCodeForProcessingRunWithoutStartProcessing()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            pdp.Initialize(mockHost);

            try
            {
                string result = pdp.GetClassCodeForProcessingRun();
            }
            catch (InvalidOperationException)
            {
                return;
            }
            Assert.Fail("The method 'GetClassCodeForProcessingRun' do not throw an exception.");
        }

        [TestMethod]
        public void GetClassCodeForProcessingRunWithWrongDirectiveName()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            string directiveName = "WrongDirectiveName";
            IDictionary<string, string> arguments = new Dictionary<string, string>(0);

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider,null, new CompilerErrorCollection());
            pdp.ProcessDirective(directiveName, arguments);
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetClassCodeForProcessingRunWithProcessDirective()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            string directiveName = "property";
            IDictionary<string, string> arguments = new Dictionary<string, string>(0);

            PropertyData propertyData = new PropertyData("ClientName", typeof(string));
            mockHost.Arguments.Add("Client", propertyData);
            arguments.Add("name", "Client");

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider,null, new CompilerErrorCollection());
            pdp.ProcessDirective(directiveName, arguments);
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();
            string expectedResult = @"
[Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates.TemplatePropertyAttribute()]
public string Client
{
    get
    {
        return ((string)(Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates.TemplateHost.CurrentHost.Arguments[""Client""].Value));
    }
}
";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void CanProcessConverterAttribute()
        {
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            string directiveName = "property";
            IDictionary<string, string> directiveAttributes = new Dictionary<string, string>();

            PropertyData propertyData = new PropertyData("TheProperty", typeof(string));
            mockHost.Arguments.Add("TheProperty", propertyData);
            directiveAttributes.Add("name", "TheProperty");
			directiveAttributes.Add("converter", typeof(StringConverter).FullName);

            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
            pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider, null, new CompilerErrorCollection());
            pdp.ProcessDirective(directiveName, directiveAttributes);
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();
			
            Assert.IsTrue(result.Contains("[System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.StringConverter))]"));
        }

        [TestMethod]
        public void CanProcessEditorAttribute()
        {
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            string directiveName = "property";
            IDictionary<string, string> directiveAttributes = new Dictionary<string, string>();

            PropertyData propertyData = new PropertyData("TheProperty", typeof(string));
            mockHost.Arguments.Add("TheProperty", propertyData);
            directiveAttributes.Add("name", "TheProperty");
			directiveAttributes.Add("editor", typeof(UITypeEditor).FullName);

            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
                  pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider,null, new CompilerErrorCollection());;
            pdp.ProcessDirective(directiveName, directiveAttributes);
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();
			
            Assert.IsTrue(result.Contains("[System.ComponentModel.EditorAttribute(typeof(System.Drawing.Design.UITypeEditor))]"));
        }

        [TestMethod]
        public void PropertyTypeMatchesTemplatePropertyTypeIfSpecified()
        {
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            string directiveName = "property";
            IDictionary<string, string> directiveAttributes = new Dictionary<string, string>();

            PropertyData propertyData = new PropertyData("TheProperty", typeof(string));
            mockHost.Arguments.Add("TheProperty", propertyData);
            directiveAttributes.Add("name", "TheProperty");
			directiveAttributes.Add("type", typeof(object).FullName);

            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
                  pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider,null, new CompilerErrorCollection());;
            pdp.ProcessDirective(directiveName, directiveAttributes);
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();
			
            Assert.IsTrue(result.Contains("public object TheProperty"));
        }		

        [TestMethod]
        public void GetClassCodeForProcessingRunWithSeveralProcessDirective()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost mockHost = new MockTemplateHost(path);
            CompilerErrorCollection errors = new CompilerErrorCollection();
            CodeDomProvider languageProvider = new CSharpCodeProvider();
            string directiveName = "property";
            IDictionary<string, string> argumentsClient = new Dictionary<string, string>(0);
            IDictionary<string, string> argumentsAmount = new Dictionary<string, string>(0);

            PropertyData dataClient = new PropertyData("ClientName", typeof(string));
            mockHost.Arguments.Add("Client", dataClient);
            argumentsClient.Add("name", "Client");

            PropertyData dataAmount = new PropertyData(1200.33, typeof(decimal));
            mockHost.Arguments.Add("Amount", dataAmount);
            argumentsAmount.Add("name", "Amount");

            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();
                  pdp.Initialize(mockHost);
            pdp.StartProcessingRun(languageProvider,null, new CompilerErrorCollection());;
            pdp.ProcessDirective(directiveName, argumentsClient);
            pdp.ProcessDirective(directiveName, argumentsAmount);
            pdp.FinishProcessingRun();

            string result = pdp.GetClassCodeForProcessingRun();
            string expectedResult = @"
[Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates.TemplatePropertyAttribute()]
public string Client
{
    get
    {
        return ((string)(Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates.TemplateHost.CurrentHost.Arguments[""Client""].Value));
    }
}

[Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates.TemplatePropertyAttribute()]
public decimal Amount
{
    get
    {
        return ((decimal)(Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates.TemplateHost.CurrentHost.Arguments[""Amount""].Value));
    }
}
";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetReferencesForProcessingRun()
        {
            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            string[] references = pdp.GetReferencesForProcessingRun();
            Assert.IsTrue(references.Length == 1);

			Assert.IsTrue(String.Equals("Microsoft.Practices.RecipeFramework.VisualStudio.Library.dll", 
				Path.GetFileName(references[0]),
				StringComparison.InvariantCultureIgnoreCase));
			
			//Assert.IsTrue(String.Equals("Microsoft.Practices.RecipeFramework.VisualStudio.Library.dll", references[0], 
			//    StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void GetImportsForProcessingRun()
        {
            // Create the PropertiesDirectiveProcessor
            PropertiesDirectiveProcessor pdp = new PropertiesDirectiveProcessor();

            Assert.IsNull(pdp.GetImportsForProcessingRun());
        }

        private class MockTemplateHost : TemplateHost
        {
            public MockTemplateHost(string path)
				: base(path, new Dictionary<string, PropertyData>())
            {
            }
        }
    }
}

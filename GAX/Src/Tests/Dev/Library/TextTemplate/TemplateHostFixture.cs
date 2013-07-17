using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text;
using System.Runtime.Remoting;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.VisualStudio.TextTemplating;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests
{
    /// <summary>
    /// Test Microsoft.Practices.RecipeFramework.Library.TextTemplateHost
    /// </summary>
    [TestClass]
    public class TemplateHostFixture
    {
        [TestMethod]
        public void CanCreateTemplateHostWithPaths()
        {
            // fields
            string path = Directory.GetCurrentDirectory();

            // Create the Host
            TemplateHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
            Assert.IsNotNull(host);
        }

		[TestMethod]
		public void ResolvesIncludeRelativeToTemplatePath()
		{
			// fields
			string path = Directory.GetCurrentDirectory();

			// Create the Host
			TemplateHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
			
			Assert.AreEqual(path + "\\Templates\\File.t4", host.ResolveFileName("Templates\\File.t4"));
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfCreateTemplateHostWithBinPathNull()
        {
            // Create the Host
			TemplateHost host = new TemplateHost(null, new Dictionary<string, PropertyData>());
        }

        [TestMethod]
        public void CanCreateTemplateHostWithArguments()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            int accountValue = 111;
            string clientName = "ClientName";

            // Create Arguments object
            IDictionary<string, PropertyData> arguments = new Dictionary<string, PropertyData>();
            arguments.Add("Account", new PropertyData(accountValue, typeof(int)));
            arguments.Add("Client", new PropertyData(clientName, typeof(string)));
        
            // Create the Host
            TemplateHost host = new TemplateHost(path, arguments);

            Assert.IsNotNull(host);
        }

        [TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfCreateTemplateHostWithArgumentsNull()
        {
            // Create the Host
            TemplateHost host = new TemplateHost(Directory.GetCurrentDirectory(), null);
        }

        [TestMethod]
        public void CanAccessArguments()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            int accountValue = 111;
            string clientName = "ClientName";

            // Create Arguments object
            IDictionary<string, PropertyData> arguments = new Dictionary<string, PropertyData>();
            arguments.Add("Account", new PropertyData(accountValue, typeof(string)));
            arguments.Add("Client", new PropertyData(clientName, typeof(string)));

            // Create the Host
			TemplateHost host = new TemplateHost(path, arguments);

            Assert.AreEqual(accountValue, host.Arguments["Account"].Value);
            Assert.AreEqual(clientName, host.Arguments["Client"].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfLoadIncludeTextFileWithNull()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string fullFileName = null;
            string fileContentResult = string.Empty;

            // Create the Host
            ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
            string a;
            string b;
            host.LoadIncludeText(fullFileName, out a, out b);
        }

        [TestMethod]
        public void LoadIncludeTextFile1()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string fileName = "LoadIncludeTextFile1.inc";
            string fullFileName = Path.Combine(path, fileName);
            string fileContent = "LoadIncludeTextFile1";

            // Create File
            File.WriteAllText(fullFileName, fileContent);

            // Create the Host
            ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            string a;
            string b;
            host.LoadIncludeText(fullFileName, out a, out b);

            // Evaluate result
            Assert.AreEqual(fileContent, a);

            // Delete File
            File.Delete(fullFileName);
        }

        [TestMethod]
        public void LoadIncludeTextFile2()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string fileName = "LoadIncludeTextFile2.inc";
            string fullFileName = Path.Combine(path, fileName);
            string fileContent = string.Empty;

            // Init fileContent
            StringBuilder sb = new StringBuilder();
            sb.Append("LoadIncludeTextFile2" + Environment.NewLine);

            for (int idx = Char.MinValue; idx < System.Int16.MaxValue; idx++)
            {
                sb.Append((Char)idx);
            }
            fileContent = sb.ToString();

            // Create File
            File.WriteAllText(fullFileName, fileContent);

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
            string a;
            string b;
            host.LoadIncludeText(fullFileName, out a, out b);
            // Evaluate result
            Assert.AreEqual(fileContent, a);

            // Delete File
            File.Delete(fullFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveAssemblyReferenceWithNull()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string fileName = null;
            string fileContentResult = string.Empty;

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
            fileContentResult = host.ResolveAssemblyReference(fileName);
        }

		[TestMethod]
		public void ResolveAssemblyExistsBinPath()
		{
			if (Directory.Exists("Temp"))
			{
				Directory.Delete("Temp", true);
			}
			Directory.CreateDirectory("Temp");
			File.Copy(new Uri(this.GetType().Assembly.CodeBase).LocalPath, "Temp\\Test.dll");

			ITextTemplatingEngineHost host = new TemplateHost(
				"Temp",new Dictionary<string, PropertyData>());
			string result = host.ResolveAssemblyReference("Test.dll");

			Assert.AreEqual(new FileInfo("Temp\\Test.dll").FullName, result, "Resolution did not include bin path even if the file existed.");

			Directory.Delete("Temp", true);
		}

		[TestMethod]
		public void ResolveAssemblyNotExistsBinPathExistsCurrDir()
		{
			if (Directory.Exists("Temp"))
			{
				Directory.Delete("Temp", true);
			}
			Directory.CreateDirectory("Temp");
			string asmName = this.GetType().Module.Name;

			ITextTemplatingEngineHost host = new TemplateHost(
				"Temp",new Dictionary<string, PropertyData>());
			string result = host.ResolveAssemblyReference(asmName);

			Assert.AreEqual(asmName, result, "Resolution did not resolve to the dll in the base directory.");
			
			Directory.Delete("Temp", true);
		}

        [TestMethod]
        public void StandardAssemblyReferences()
        {
            // fields
            string path = Directory.GetCurrentDirectory();

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path,new Dictionary<string, PropertyData>());

            Assert.IsNull(host.StandardAssemblyReferences);
        }

        [TestMethod]
        public void StandardImports()
        {
            // fields
            string path = Directory.GetCurrentDirectory();

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            Assert.IsNull(host.StandardImports);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveDirectiveProcessorWithNull()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string fileName = null;

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
            Type result = host.ResolveDirectiveProcessor(fileName);
        }

        [TestMethod]
        public void ResolveDirectiveProcessor()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string processorName = "PropertyProcessor";

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            Type result = host.ResolveDirectiveProcessor(processorName);

            Assert.AreEqual(typeof(PropertiesDirectiveProcessor), result);
        }

        [TestMethod]
        public void ResolveDirectiveProcessorWrongName()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string processorName = "PropertyProcessorWrongName";

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            Type result = host.ResolveDirectiveProcessor(processorName);

            Assert.AreEqual(null, result);
        }

        // Get new AppDomain for the Microsoft.Practices.RecipeFramework.Library.TextTemplate
        [TestMethod]
        public void ProvideTemplatingAppDomainWithNull()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string content = null;

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            AppDomain appDomain = host.ProvideTemplatingAppDomain(content);

            object test = appDomain.CreateInstanceAndUnwrap(
                typeof(MockTransformationRunner).Assembly.FullName,
                typeof(MockTransformationRunner).FullName);

            Assert.IsNotNull(test);
            Assert.AreEqual(typeof(MockTransformationRunner).FullName, test.GetType().FullName);
        }

        // Get new AppDomain for the Microsoft.Practices.RecipeFramework.Library.TextTemplate
        [TestMethod]
        public void ProvideTemplatingAppDomain()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string content = string.Empty;

            // Create the Host
			ITextTemplatingEngineHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            AppDomain appDomain = host.ProvideTemplatingAppDomain(content);

            object test = appDomain.CreateInstanceAndUnwrap(
                typeof(MockTransformationRunner).Assembly.FullName,
                typeof(MockTransformationRunner).FullName);

            Assert.IsNotNull(test);
            Assert.AreEqual(typeof(MockTransformationRunner).FullName, test.GetType().FullName);
        }

        // Get new AppDomain for the Microsoft.Practices.RecipeFramework.Library.TextTemplate and check if the 
        // new AppDomain Static Host have the sended argumants values.
        [TestMethod]
        public void ProvideTemplatingAppDomainWithStaticHostValues()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            string content = string.Empty;
            int accountValue = 111;
            string clientName = "ClientName";

            // Create Arguments object
            IDictionary<string, PropertyData> arguments = new Dictionary<string, PropertyData>();
            arguments.Add("Account", new PropertyData(accountValue, typeof(int)));
            arguments.Add("Client", new PropertyData(clientName, typeof(string)));

            // Create the Host
            ITextTemplatingEngineHost host = new TemplateHost(path, arguments);
            
            AppDomain appDomain = host.ProvideTemplatingAppDomain(content);

            object test = appDomain.CreateInstanceAndUnwrap(
                typeof(MockTransformationRunner).Assembly.FullName,
                typeof(MockTransformationRunner).FullName);
            MockTransformationRunner mock = (MockTransformationRunner)test;

            Assert.IsNotNull(test);
            Assert.AreEqual(typeof(MockTransformationRunner).FullName, test.GetType().FullName);

            int accountResult = (int)((PropertyData)mock.GetHostArgumentValue("Account")).Value;
            string clientResult = (string)((PropertyData)mock.GetHostArgumentValue("Client")).Value;
            Assert.AreEqual(accountValue, accountResult);
            Assert.AreEqual(clientName, clientResult);
        }

        [TestMethod]
        public void TemplateHostIsITextTemplatingEngineHost()
        {
            // fields
            string path = Directory.GetCurrentDirectory();

            // Create the Host
			TemplateHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            Assert.IsTrue(host is ITextTemplatingEngineHost);
        }

        [TestMethod]
        public void TemplateHostIsMarshalByRefObject()
        {
            // fields
            string path = Directory.GetCurrentDirectory();

            // Create the Host
			TemplateHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());

            Assert.IsTrue(host is MarshalByRefObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IncludeTemplateThrowsIfTemplatePathNotUnderBinPath()
        {
            string path = Directory.GetCurrentDirectory();
            
            // Create the Host
            string a;
            string b;
            TemplateHost host = new TemplateHost(path, new Dictionary<string, PropertyData>());
            bool include = host.LoadIncludeText("C:\\Temp\\Outside.ipe",  out a, out b);
        }


        private class MockTransformationRunner : MarshalByRefObject
        {
            public MockTransformationRunner()
            {
            }

            public object GetHostArgumentValue(string key)
            {
                return TemplateHost.CurrentHost.Arguments[key];
            }
        }
    }
}

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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests
{
    /// <summary>
    /// Test Microsoft.Practices.RecipeFramework.Library.TextTemplateHostHelper class.
    /// </summary>
    [TestClass]
    public class TemplateHostHelperFixture
    {
        // Null Arguments passed in the constructor
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TemplateHostHelperConstructorWithNullArguments()
        {
            // fields
            MockTemplateHost host = null;

            // Create the ServiceContainer
            TemplateHostInitializer hostHelper = new TemplateHostInitializer(host); 
        }

        // Test Microsoft.Practices.RecipeFramework.Library.TextTemplateHostHelper with a valid argument
        [TestMethod]
        public void TemplateHostHelperWithValidArguments()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost host = new MockTemplateHost(path);

            // Create the ServiceContainer
            TemplateHostInitializer hostHelper = new TemplateHostInitializer(host); 
        }

        // Get the argument value fron the static Microsoft.Practices.RecipeFramework.Library.TextTemplateHostHelper object
        [TestMethod]
        public void TemplateHostHelperGetValueFromStaticObject()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost host = new MockTemplateHost(path);

            host.Arguments.Add("TestKey", new PropertyData("TestValue", typeof(string)));

            // Create the ServiceContainer
            TemplateHostInitializer hostHelper = new TemplateHostInitializer(host);

            string result = (string)MockTemplateHost.CurrentHost.Arguments["TestKey"].Value;
            Assert.AreEqual("TestValue", result);
        }

        // Get the argument value fron the static Microsoft.Practices.RecipeFramework.Library.TextTemplateHostHelper object
        //  using a custom object.
        [TestMethod]
        public void TemplateHostHelperWithCustomObject()
        {
            // fields
            string path = Directory.GetCurrentDirectory();
            MockTemplateHost host = new MockTemplateHost(path);
            MockTemplateHostHelperValue value = new MockTemplateHostHelperValue();
            host.Arguments.Add("TestKey", new PropertyData(value, typeof(MockTemplateHostHelperValue)));

            // Create the ServiceContainer
            TemplateHostInitializer hostHelper = new TemplateHostInitializer(host);

            MockTemplateHostHelperValue result = (MockTemplateHostHelperValue)MockTemplateHost.CurrentHost.Arguments["TestKey"].Value;

            Assert.AreEqual("MockTemplateHostHelperValueName", result.Name);
        }

        #region MockObjects
        private class MockTemplateHost : TemplateHost
        {
            public MockTemplateHost(string path)
				: base(path, new Dictionary<string, PropertyData>())
            {
            }
        }

        public class MockTemplateHostHelperValue
        {
            private string name = "MockTemplateHostHelperValueName";

            public MockTemplateHostHelperValue()
            {
            }

            public string Name
            {
                get { return this.name; }
            }
        }
        #endregion
    }
}

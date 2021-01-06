//#region Using directives

//using System;
//using System.Configuration;
//using System.Diagnostics;
//using System.IO;
//using System.Xml;
//using System.Xml.XPath;
//using Microsoft.Practices.RecipeFramework.Internal;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.Practices.Common;
//using Mvp.Xml.Common.XPath;

//#endregion Using directives

//namespace Microsoft.Practices.RecipeFramework
//{
//    [TestClass]
//    public class ManifestInstallerTest
//    {
//        static readonly string InstallUtil = Path.Combine(Utils.GetClrInstallationDirectory(), "InstallUtil.exe");

//        [TestMethod]
//        public void TestBadInstaller()
//        {
//            //UninstallHost("TestingHost");

//            ProcessStartInfo info = new ProcessStartInfo(InstallUtil, "/i /ShowCallStack /Host=TestingHost /Installer=\"" +
//                this.GetType().AssemblyQualifiedName + "\" " +
//                ReflectionHelper.GetAssemblyPath(typeof(ManifestInstallerTest).Assembly));
//            info.CreateNoWindow = true;
//            info.RedirectStandardOutput = true;
//            info.UseShellExecute = false;
//            Process install = Process.Start(info);
//            string output = install.StandardOutput.ReadToEnd();
//            install.WaitForExit();

//            if ((output.IndexOf("System.ArgumentException") == -1) && (output.IndexOf("No public installers with the RunInstallerAttribute.Yes attribute could be found") == -1))
//            {
//                Console.WriteLine(output);
//                Assert.Fail("Installation didn't throw an ArgumentException");
//            }

//            XPathDocument doc = new XPathDocument(RecipeManager.GetMainManifest().BaseURI);
//            string path = "gax:RecipeFramework/gax:Hosts/gax:Host[@Name=$name]";
//            XmlNamespaceManager context = new XmlNamespaceManager(doc.CreateNavigator().NameTable);
//            context.AddNamespace(Configuration.SchemaInfo.Prefix, Configuration.SchemaInfo.ManifestNamespace);
//            XPathNodeIterator it = XPathCache.Select(path, doc.CreateNavigator(), context,
//                new XPathVariable[] { new XPathVariable("name", "TestingHost") });

//            Assert.IsFalse(it.MoveNext());
//        }

//        [TestMethod]
//        public void TestGoodInstaller()
//        {
//            try
//            {
//                InstallHost("TestGoodInstaller", typeof(TestInstaller), false);

//                XPathDocument doc = new XPathDocument(RecipeManager.GetMainManifest().BaseURI);
//                string path = "gax:RecipeFramework/gax:Hosts/gax:Host[@Name=$name]";
//                XmlNamespaceManager context = new XmlNamespaceManager(doc.CreateNavigator().NameTable);
//                context.AddNamespace(Configuration.SchemaInfo.Prefix, Configuration.SchemaInfo.ManifestNamespace);
//                XPathNodeIterator it = XPathCache.Select(path, doc.CreateNavigator(), context,
//                    new XPathVariable[] { new XPathVariable("name", "TestGoodInstaller") });

//                Assert.IsTrue(it.MoveNext(), "Didn't find the node that should have been added for the host.");
//                Assert.AreEqual("TestGoodInstaller", it.Current.GetAttribute(Configuration.AttributeNames.Name, String.Empty));
//                Assert.AreEqual(typeof(TestInstaller).AssemblyQualifiedName, it.Current.GetAttribute(Configuration.AttributeNames.InstallerType, String.Empty));
//            }
//            finally
//            {
//                UninstallHost("TestGoodInstaller");
//            }
//        }

//        /// <summary>
//        /// Uninstalls a host by name, and returns the output of the InstallUtil.exe tool.
//        /// </summary>
//        /// <param name="host"></param>
//        /// <returns></returns>
//        internal static string UninstallHost(string host)
//        {
//            string str = "/u /DesignMode /Host=" + host + " " +
//                ReflectionHelper.GetAssemblyPath(typeof(RecipeManager).Assembly);
//            Console.Write("\nUninstall host with the following parameters:[{0}]\n", str);

//            ProcessStartInfo uninstinfo = new ProcessStartInfo(InstallUtil, str);

//            uninstinfo.CreateNoWindow = true;
//            uninstinfo.RedirectStandardOutput = true;
//            uninstinfo.UseShellExecute = false;
//            Process uninstall = Process.Start(uninstinfo);
//            string output = uninstall.StandardOutput.ReadToEnd();
//            uninstall.WaitForExit();

//            if (output.IndexOf("An exception occurred while uninstalling.") != -1)
//            {
//                Console.Write(output);
//                Assert.Fail("The uninstallation didn't succeed.");
//            }

//            return output;
//        }

//        /// <summary>
//        /// Uninstalls and then installs the host. Returns the output of the InstallUtil.exe tool.
//        /// </summary>
//        /// <param name="host"></param>
//        /// <param name="installer"></param>
//        /// <param name="debug">Whether to launch a debugger from inside the installer class.</param>
//        internal static string InstallHost(string host, Type installer, bool debug)
//        {
//            //UninstallHost(host);

//            ProcessStartInfo info = new ProcessStartInfo(InstallUtil, String.Format(
//                "/i /DesignMode /ShowCallStack {0} /Host={1} /Installer=\"{2}\" {3}",
//                debug ? "/DebugInstaller=true" : "",
//                host, installer.AssemblyQualifiedName,
//                ReflectionHelper.GetAssemblyPath(typeof(RecipeManager).Assembly)));
//            info.CreateNoWindow = true;
//            info.RedirectStandardOutput = true;
//            info.UseShellExecute = false;
//            Process install = Process.Start(info);
//            string output = install.StandardOutput.ReadToEnd();
//            install.WaitForExit();

//            // Test that no rollback happened.
//            if (output.IndexOf("The Rollback phase of the installation is beginning.") != -1)
//            {
//                Console.Write(output);
//                Assert.Fail("The installation didn't succeed, as there's a rollback message in InstallUtil output.");
//            }

//            return output;
//        }

//        /// <summary>
//        /// Installs a package and returns the output of the InstallUtil.exe tool.
//        /// </summary>
//        internal static string InstallPackage(string configuration, bool debugInstall)
//        {
//            ProcessStartInfo info = new ProcessStartInfo(InstallUtil, String.Format(
//                "/i /DesignMode /ShowCallStack {0} /Configuration=\"{1}\" {2}",
//                debugInstall ? "/DebugInstaller=true" : "",
//                configuration,
//                ReflectionHelper.GetAssemblyPath(typeof(RecipeManager).Assembly)));
//            info.CreateNoWindow = true;
//            info.RedirectStandardOutput = true;
//            info.UseShellExecute = false;
//            Process install = Process.Start(info);
//            string output = install.StandardOutput.ReadToEnd();
//            install.WaitForExit();

//            // Test that no rollback happened.
//            if (output.IndexOf("The Rollback phase of the installation is beginning.") != -1)
//            {
//                Console.Write(output);
//                Assert.Fail("The installation didn't succeed, as there's a rollback message in InstallUtil output.");
//            }

//            return output;
//        }

//        /// <summary>
//        /// Uninstalls a package and returns the output of the InstallUtil.exe tool.
//        /// </summary>
//        internal static string UninstallPackage(string configuration, bool debugInstall)
//        {
//            string str = String.Format(
//                "/u /DesignMode /ShowCallStack {0} /Configuration=\"{1}\" {2}",
//                debugInstall ? "/DebugInstaller=true" : "",
//                configuration,
//                ReflectionHelper.GetAssemblyPath(typeof(RecipeManager).Assembly));

//            Console.Write("\nUninstall Package with the following parameters:[{0}]\n", str);
//            // Uninstalling a non-existing one should not cause errors.
//            ProcessStartInfo info = new ProcessStartInfo(InstallUtil, str);
//            info.CreateNoWindow = true;
//            info.RedirectStandardOutput = true;
//            info.UseShellExecute = false;
//            Process install = Process.Start(info);
//            string output = install.StandardOutput.ReadToEnd();
//            install.WaitForExit();

//            // Test that no rollback happened.
//            if (output.IndexOf("The Rollback phase of the installation is beginning.") != -1)
//            {
//                Console.Write(output);
//                Assert.Fail("The uninstallation didn't succeed, as there's a rollback message in InstallUtil output.");
//            }

//            return output;
//        }

//        [TestMethod]
//        public void InstallPackageTest()
//        {
//            Configuration.GuidancePackage package = GuidancePackage.ReadConfiguration(Utils.MakeTestRelativePath("RecipeTest.xml"));
//            bool debug = false;

//            // Ensure host
//            InstallHost(package.Host, typeof(TestInstaller), false);
//            try
//            {
//                UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);

//                // Now install it.
//                string output = InstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);

//                // Check that the installer class was called.
//                if (output.IndexOf(typeof(TestInstaller).AssemblyQualifiedName) == -1)
//                {
//                    Console.Write(output);
//                    Assert.Fail("Installer for package was not run.");
//                }

//                XPathDocument doc = new XPathDocument(RecipeManager.GetMainManifest().BaseURI);
//                string path = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name]";
//                XmlNamespaceManager context = new XmlNamespaceManager(doc.CreateNavigator().NameTable);
//                context.AddNamespace(Configuration.SchemaInfo.Prefix, Configuration.SchemaInfo.ManifestNamespace);
//                XPathNodeIterator it = XPathCache.Select(path, doc.CreateNavigator(), context,
//                    new XPathVariable[] { new XPathVariable("name", package.Name) });
//                Assert.IsTrue(it.MoveNext(), "Didn't find the package node that should have been added.");
//                Assert.AreEqual(package.Caption, it.Current.GetAttribute(Configuration.AttributeNames.Caption, String.Empty));
//            }
//            finally
//            {
//                ManifestInstallerTest.UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), false);
//                UninstallHost(package.Host);
//            }
//        }

//        [TestMethod]
//        public void UninstallHostAndPackages()
//        {
//            Configuration.GuidancePackage package = GuidancePackage.ReadConfiguration(
//                Utils.MakeTestRelativePath("RecipeTest.xml"));
//            bool debug = false;

//            // Install host
//            InstallHost(package.Host, typeof(TestInstaller), false);
//            try
//            {
//                UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);

//                // Now install it.
//                string output = InstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);

//                // Check that the installer class was called.
//                if (output.IndexOf(typeof(TestInstaller).AssemblyQualifiedName) == -1)
//                {
//                    Console.Write(output);
//                    Assert.Fail("Installer for package was not run.");
//                }

//                UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);
//                UninstallHost(package.Host);

//                XPathDocument doc = new XPathDocument(RecipeManager.GetMainManifest().BaseURI);
//                string path = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name]";
//                XmlNamespaceManager context = new XmlNamespaceManager(doc.CreateNavigator().NameTable);
//                context.AddNamespace(Configuration.SchemaInfo.Prefix, Configuration.SchemaInfo.ManifestNamespace);
//                XPathNodeIterator it = XPathCache.Select(path, doc.CreateNavigator(), context,
//                    new XPathVariable[] { new XPathVariable("name", package.Name) });

//                Assert.IsFalse(it.MoveNext(), "Package node was not removed by uninstalling the host.");
//            }
//            finally
//            {
//                UninstallHost(package.Host);
//            }
//        }

//        [TestMethod]
//        public void UninstallPackageTest()
//        {
//            Configuration.GuidancePackage package = GuidancePackage.ReadConfiguration(Utils.MakeTestRelativePath("RecipeTest.xml"));
//            bool debug = false;

//            // Ensure host
//            InstallHost(package.Host, typeof(TestInstaller), false);
//            try
//            {
//                UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);

//                // Now install it.
//                string output = InstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);
//                // Check that the installer was called.
//                if (output.IndexOf(typeof(TestInstaller).AssemblyQualifiedName) == -1)
//                {
//                    Console.Write(output);
//                    Assert.Fail("Installer for package was not run.");
//                }

//                // Now uninstall again.
//                output = UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), debug);
//                // Check that the uninstaller was called.
//                if (output.IndexOf(typeof(TestInstaller).AssemblyQualifiedName) == -1)
//                {
//                    Console.Write(output);
//                    Assert.Fail("Installer for package was not run.");
//                }

//                XPathDocument doc = new XPathDocument(RecipeManager.GetMainManifest().BaseURI);
//                string path = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name]";
//                XmlNamespaceManager context = new XmlNamespaceManager(doc.CreateNavigator().NameTable);
//                context.AddNamespace(Configuration.SchemaInfo.Prefix, Configuration.SchemaInfo.ManifestNamespace);
//                XPathNodeIterator it = XPathCache.Select(path, doc.CreateNavigator(), context,
//                    new XPathVariable[] { new XPathVariable("name", package.Name) });

//                Assert.IsFalse(it.MoveNext(), "Package was not removed from manifest.");
//            }
//            finally
//            {
//                UninstallHost(package.Host);
//            }
//        }

//        public class TestInstaller : IHostInstaller
//        {
//            #region IHostInstaller Members

//            public void InstallPackage(System.Configuration.Install.InstallContext context, Configuration.GuidancePackage packageConfig)
//            {
//                context.LogMessage(this.GetType().AssemblyQualifiedName);
//            }

//            public void UninstallPackage(System.Configuration.Install.InstallContext context, Configuration.GuidancePackage packageConfig)
//            {
//                context.LogMessage(this.GetType().AssemblyQualifiedName);
//            }

//            public void InstallHost(System.Configuration.Install.InstallContext context)
//            {
//            }

//            public void UninstallHost(System.Configuration.Install.InstallContext context)
//            {
//            }

//            #endregion
//        }
//    }
//}

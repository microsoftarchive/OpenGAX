#region Using directives

using System;
using System.Collections;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.Practices.Common.Services;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
	[TestClass]
	public class TypeResolutionServiceTests
	{
		#region Setup & Tear down
		RecipeManager Manager;

		ArrayList CodeFiles = new ArrayList();
		ArrayList AsmFiles = new ArrayList();

		[TestInitialize]
		public void SetUp()
		{
			CodeFiles = new ArrayList();
			AsmFiles = new ArrayList();

			Manager = new RecipeManager();
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
			Manager.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
		}

		[TestCleanup]
		public void TearDown()
		{
			foreach (string file in CodeFiles)
			{
				try
				{
					File.Delete(file);
				}
				catch { }
			}
			// Assemblies may not be deleted as they may have been loaded.
			foreach (string file in AsmFiles)
			{
				try
				{
					File.Delete(file);
				}
				catch { }
			}
			Manager = null;
		}

		private void AddTempFiles(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CodeFiles.Add(Path.GetTempFileName());
				AsmFiles.Add(Path.GetTempFileName() + ".dll");
			}
		}

		private void Compile(int index, params string[] references)
		{
			CodeDomProvider provider = new Microsoft.CSharp.CSharpCodeProvider();

			string file = (string)CodeFiles[index];
			string asm = (string)AsmFiles[index];

			CompilerParameters prms = new CompilerParameters(references, asm);
			CompilerResults results = provider.CompileAssemblyFromFile(prms, file);

			foreach (string output in results.Output)
			{
				Console.WriteLine(output);
			}

			Assert.IsFalse(results.Errors.HasErrors);
		}

		#endregion Setup & Tear down

		[TestMethod]
		public void TestAssemblyFullName()
		{
			Assembly asm = new TypeResolutionService(
				new Uri(GetType().Assembly.CodeBase).LocalPath).GetAssembly(
				Common.ReflectionHelper.ParseAssemblyName(
					typeof(string).Assembly.GetName().FullName));
			Assert.AreEqual(typeof(string).Assembly, asm);
		}

		[TestMethod]
		public void TestAssemblyString()
		{
			Assert.AreEqual(
				typeof(string).Assembly.GetName().FullName,
				Common.ReflectionHelper.GetAssemblyString(typeof(string).AssemblyQualifiedName));
		}

		[TestMethod]
		public void TestAssemblyName()
		{
			Assert.AreEqual(
				typeof(string).Assembly.GetName().FullName,
				Common.ReflectionHelper.ParseAssemblyName(
					Common.ReflectionHelper.GetAssemblyString(typeof(string).AssemblyQualifiedName)).FullName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAssemblyLoad()
		{
			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			trs.GetAssembly(null);
		}

		[TestMethod]
		public void TestAssemblyLoad1()
		{
			AddTempFiles(1);

			using (StreamWriter sw = new StreamWriter((string)CodeFiles[0]))
			{
				sw.WriteLine(@"
	public class Test
	{
	}");
			}

			Compile(0);

			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			Assembly asm = trs.GetAssembly(Common.ReflectionHelper.ParseAssemblyName(
				Path.GetFileNameWithoutExtension((string)AsmFiles[0])));

			Assert.IsNotNull(asm);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TestGetAssemblyName()
		{
			Common.ReflectionHelper.GetAssemblyName(Path.GetTempFileName());
		}

		[TestMethod]
		public void TestAssemblyLoad3()
		{
			AddTempFiles(2);

			using (StreamWriter sw = new StreamWriter((string)CodeFiles[0]))
			{
				sw.WriteLine(@"
namespace TestLoad1
{
	public class First
	{
	}
}");
			}

			Compile(0);

			using (StreamWriter sw = new StreamWriter((string)CodeFiles[1]))
			{
				sw.WriteLine(@"
namespace TestLoad2
{
	public class Second : TestLoad1.First
	{
	}
}");
			}

			Compile(1, (string)AsmFiles[0]);

			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());

			Assembly asm = trs.GetAssembly(Common.ReflectionHelper.ParseAssemblyName(
				Path.GetFileNameWithoutExtension((string)AsmFiles[1])));

			Assert.IsNotNull(asm);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTypeLoad()
		{
			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			Type t = trs.GetType(null);
		}

		[TestMethod]
		public void TestTypeLoad1()
		{
			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			Type t = trs.GetType("Kzu, Clarius");
			Assert.IsNull(t);
		}

		[TestMethod]
		[ExpectedException(typeof(TypeLoadException))]
		public void TestTypeLoad2()
		{
			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			Type t = trs.GetType("Kzu, Clarius", true);
		}

		[TestMethod]
		public void TestTypeLoad3()
		{
			AddTempFiles(1);

			using (StreamWriter sw = new StreamWriter((string)CodeFiles[0]))
			{
				sw.WriteLine(@"
namespace TestLoad1
{
	public class First
	{
	}
}");
			}

			Compile(0);

			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			Type t = trs.GetType("TestLoad1.First, " + 
				Path.GetFileNameWithoutExtension((string)AsmFiles[0]));

			Assert.IsNotNull(t);
		}

		[TestMethod]
		public void TestTypeLoad4()
		{
			AddTempFiles(2);

			using (StreamWriter sw = new StreamWriter((string)CodeFiles[0]))
			{
				sw.WriteLine(@"
namespace TestLoad1
{
	public class First
	{
		public void Hello()
		{
			System.Console.WriteLine(""Hello!"");
		}
	}
}");
			}

			Compile(0);

			using (StreamWriter sw = new StreamWriter((string)CodeFiles[1]))
			{
				sw.WriteLine(@"
namespace TestLoad2
{
	public class Second : TestLoad1.First
	{
	}
}");
			}

			Compile(1, (string)AsmFiles[0]);

			TypeResolutionService trs = new TypeResolutionService(Path.GetTempPath());
			Type t = trs.GetType("TestLoad2.Second, " +
				Path.GetFileNameWithoutExtension((string)AsmFiles[1]));

			Assert.IsNotNull(t);

			object second = Activator.CreateInstance(t);
			second.GetType().InvokeMember("Hello", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null, second, new object[0]);

		}

		[TestMethod]
		public void TestProvider()
		{
			string folder = Path.GetTempPath() + "MyTestPackage";
			if (Directory.Exists(folder))
			{
				Directory.Delete(folder, true);
			}
			Directory.CreateDirectory(folder);
			string tempfile = Path.Combine(folder, "MyTestFile.dll");
			string code = "public class MyClass {}";
			System.CodeDom.Compiler.CompilerResults results =
				new Microsoft.CSharp.CSharpCodeProvider().CompileAssemblyFromSource(
				new CompilerParameters(new string[] { "System.dll" }, tempfile), code);
			if (results.Errors.HasErrors)
			{
				string errors = "";
				foreach (object error in results.Errors)
					errors += error + "\n";
				Assert.Fail(errors);
			}

			// Copy the package config to the temp location and install from there.
			//string config = Utils.MakeTestRelativePath(@"Services
		}
	}
}

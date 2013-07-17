#region Using directives

using System;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockDte : DTE
	{
		#region _DTE Members

		public Document ActiveDocument
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public object ActiveSolutionProjects
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Window ActiveWindow
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public AddIns AddIns
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public DTE Application
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public object CommandBars
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public string CommandLineArguments
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Commands Commands
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public ContextAttributes ContextAttributes
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public DTE DTE
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Debugger Debugger
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public vsDisplay DisplayMode
		{
			get
			{
				throw new global::System.NotImplementedException();
			}

			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public Documents Documents
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public string Edition
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Events Events
		{
			get { throw new global::System.NotImplementedException(); }
		}


		public void ExecuteCommand(string CommandName, string CommandArgs)
		{
			throw new NotImplementedException();
		}
		public string FileName
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Find Find
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public string FullName
		{
			get { throw new global::System.NotImplementedException(); }
		}


		public object GetObject(string Name)
		{
			throw new NotImplementedException();
		}
		public Globals Globals
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public ItemOperations ItemOperations
		{
			get { throw new global::System.NotImplementedException(); }
		}


		public wizardResult LaunchWizard(string VSZFile, ref object[] ContextParams)
		{
			throw new NotImplementedException();
		}
		public int LocaleID
		{
			//The Wizard framework quesries this value
			get { return 1033; }
		}

		public Macros Macros
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public DTE MacrosIDE
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Window MainWindow
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public vsIDEMode Mode
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public string Name
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public ObjectExtenders ObjectExtenders
		{
			get { throw new global::System.NotImplementedException(); }
		}


		public Window OpenFile(string ViewKind, string FileName)
		{
			throw new NotImplementedException();
		}

		public void Quit()
		{
			throw new NotImplementedException();
		}
		public string RegistryRoot
		{
			get { throw new global::System.NotImplementedException(); }
		}


		public string SatelliteDllPath(string Path, string Name)
		{
			throw new NotImplementedException();
		}
		public SelectedItems SelectedItems
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Solution Solution
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public SourceControl SourceControl
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public StatusBar StatusBar
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public bool SuppressUI
		{
			get
			{
				throw new global::System.NotImplementedException();
			}

			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public UndoContext UndoContext
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public bool UserControl
		{
			get
			{
				throw new global::System.NotImplementedException();
			}

			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public string Version
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public WindowConfigurations WindowConfigurations
		{
			get { throw new global::System.NotImplementedException(); }
		}

		public Windows Windows
		{
			get { throw new global::System.NotImplementedException(); }
		}


		public bool get_IsOpenFile(string ViewKind, string FileName)
		{
			throw new NotImplementedException();
		}

		public EnvDTE.Properties get_Properties(string Category, string Page)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

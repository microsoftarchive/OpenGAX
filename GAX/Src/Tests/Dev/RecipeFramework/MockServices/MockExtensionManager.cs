#region Using directives

using System;
using System.Linq;
using System.Collections;
using Microsoft.VisualStudio.ExtensionManager;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockExtensionManager : IVsExtensionManager, SVsExtensionManager
	{
		private List<string> guidancePackages = new List<string>();

		public void AddGuidancePackage(string guidancePackageConfigurationFile)
		{
			this.guidancePackages.Add(guidancePackageConfigurationFile);
		}

		public IExtension CreateExtension(string extensionPath)
		{
			throw new NotImplementedException();
		}

		public IInstallableExtension CreateInstallableExtension(string extensionPath)
		{
			throw new NotImplementedException();
		}

		public bool DidLoadUserExtensions
		{
			get { throw new NotImplementedException(); }
		}

		public RestartReason Disable(IInstalledExtension extension)
		{
			throw new NotImplementedException();
		}

		public RestartReason Enable(IInstalledExtension extension)
		{
			throw new NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<IExtensionReference> FindMissingReferences(IExtension extension)
		{
			throw new NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<string> GetEnabledExtensionContentLocations(string contentTypeName, System.Collections.Generic.IDictionary<string, string> attributes)
		{
			throw new NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<string> GetEnabledExtensionContentLocations(string contentTypeName)
		{
			throw new NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<IInstalledExtension> GetEnabledExtensions(string contentTypeName)
		{
			throw new NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<IInstalledExtension> GetEnabledExtensions()
		{
			return this.guidancePackages.Select(gp => new MockInstalledExtension(gp));
		}

		public System.Collections.Generic.IEnumerable<IInstalledExtension> GetImmediateDependants(IInstalledExtension extension)
		{
			throw new NotImplementedException();
		}

		public IInstalledExtension GetInstalledExtension(string identifier)
		{
			throw new NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<IInstalledExtension> GetInstalledExtensions()
		{
			throw new NotImplementedException();
		}

		public RestartReason Install(IInstallableExtension extension, bool perMachine)
		{
			throw new NotImplementedException();
		}

		public void InstallAsync(IInstallableExtension extension, bool perMachine, object userState)
		{
			throw new NotImplementedException();
		}

		public void InstallAsync(IInstallableExtension extension, bool perMachine)
		{
			throw new NotImplementedException();
		}

		public void InstallAsyncCancel(object userState)
		{
			throw new NotImplementedException();
		}


		public event EventHandler<InstallCompletedEventArgs> InstallCompleted
		{
			add { throw new NotSupportedException(); }
			remove { throw new NotSupportedException(); }
		}

		public event EventHandler<InstallProgressChangedEventArgs> InstallProgressChanged
		{
			add { throw new NotSupportedException(); }
			remove { throw new NotSupportedException(); }
		}

		public bool IsInstalled(IExtension extension)
		{
			throw new NotImplementedException();
		}

		public RestartReason RestartRequired
		{
			get { throw new NotImplementedException(); }
		}

		public void RevertUninstall(IInstalledExtension extension)
		{
			throw new NotImplementedException();
		}

		public bool TryGetInstalledExtension(string identifier, out IInstalledExtension result)
		{
			throw new NotImplementedException();
		}

		public void Uninstall(IInstalledExtension extension)
		{
			throw new NotImplementedException();
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
		{
			add { throw new NotSupportedException(); }
			remove { throw new NotSupportedException(); }
		}

		class MockInstalledExtension : IInstalledExtension
		{
			string guidancePackageConfigurationFilename;

			public MockInstalledExtension(string guidancePackageConfigurationFilename)
			{
				this.guidancePackageConfigurationFilename = guidancePackageConfigurationFilename;
			}

			public string InstallPath
			{
				get { return Path.GetDirectoryName(guidancePackageConfigurationFilename); }
			}

			public DateTimeOffset InstalledOn
			{
				get { throw new NotImplementedException(); }
			}

			public bool InstalledPerMachine
			{
				get { throw new NotImplementedException(); }
			}

			public ulong SizeInBytes
			{
				get { throw new NotImplementedException(); }
			}

			public EnabledState State
			{
				get { throw new NotImplementedException(); }
			}

			public IEnumerable<IExtensionContent> Content
			{
				get { yield return new MockCustomExtension(this.guidancePackageConfigurationFilename) ; }
			}

			public IExtensionHeader Header
			{
				get { return new MockExtensionHeader(); }
			}

			public IEnumerable<IExtensionReference> References
			{
				get { throw new NotImplementedException(); }
			}

			public string Type
			{
				get { throw new NotImplementedException(); }
			}

			class MockExtensionHeader : IExtensionHeader
			{
				public bool AllUsers
				{
					get { throw new NotImplementedException(); }
				}

				public string Author
				{
					get { throw new NotImplementedException(); }
				}

				public string Description
				{
					get { return "MockGuidancePackage"; }
				}

				public IEnumerable<string> GetSupportedIsolatedShells(Version shellVersion)
				{
					throw new NotImplementedException();
				}

				public IEnumerable<string> GetSupportedVSEditions(Version vsVersion)
				{
					throw new NotImplementedException();
				}

				public Uri GettingStartedGuide
				{
					get { throw new NotImplementedException(); }
				}

				public string Icon
				{
					get { throw new NotImplementedException(); }
				}

				public string Identifier
				{
					get { throw new NotImplementedException(); }
				}

				public bool InstalledByMsi
				{
					get { throw new NotImplementedException(); }
				}

				public bool IsIsolatedShellSupported(Version shellVersion, string appName)
				{
					throw new NotImplementedException();
				}

				public bool IsVSEditionSupported(Version vsVersion, string edition)
				{
					throw new NotImplementedException();
				}

				public string License
				{
					get { throw new NotImplementedException(); }
				}

				public bool LicenseClickThrough
				{
					get { throw new NotImplementedException(); }
				}

				public string LicenseFormat
				{
					get { throw new NotImplementedException(); }
				}

				public System.Globalization.CultureInfo Locale
				{
					get { throw new NotImplementedException(); }
				}

				public string LocalizedDescription
				{
					get { throw new NotImplementedException(); }
				}

				public string LocalizedName
				{
					get { throw new NotImplementedException(); }
				}

				public Uri MoreInfoUrl
				{
					get { throw new NotImplementedException(); }
				}

				public string Name
				{
					get { return "MockGuidancePackage"; }
				}

				public string PreviewImage
				{
					get { throw new NotImplementedException(); }
				}

				public Version SupportedFrameworkMaxVersion
				{
					get { throw new NotImplementedException(); }
				}

				public Version SupportedFrameworkMinVersion
				{
					get { throw new NotImplementedException(); }
				}

				public IEnumerable<Version> SupportedIsolatedShellVersions
				{
					get { throw new NotImplementedException(); }
				}

				public IEnumerable<Version> SupportedVSVersions
				{
					get { throw new NotImplementedException(); }
				}

				public bool SystemComponent
				{
					get { throw new NotImplementedException(); }
				}

				public Version Version
				{
					get { return new Version(); }
				}

                public IList<System.Xml.XmlElement> AdditionalElements
                {
                    get { throw new NotImplementedException(); }
                }

                public bool GlobalScope
                {
                    get { throw new NotImplementedException(); }
                }

                public IList<System.Xml.XmlElement> LocalizedAdditionalElements
                {
                    get { throw new NotImplementedException(); }
                }

                public Uri ReleaseNotes
                {
                    get { throw new NotImplementedException(); }
                }

                public byte[] ReleaseNotesContent
                {
                    get { throw new NotImplementedException(); }
                }

                public string ReleaseNotesFormat
                {
                    get { throw new NotImplementedException(); }
                }

                public VersionRange SupportedFrameworkVersionRange
                {
                    get { throw new NotImplementedException(); }
                }

                public IEnumerable<string> Tags
                {
                    get { throw new NotImplementedException(); }
                }

                public string ShortcutPath
                {
                    get { throw new NotImplementedException(); }
                }

                public bool IsExperimental
                {
                    get
                    {
                        throw new NotImplementedException();
                    }
                }
            }

			class MockCustomExtension : IExtensionContent
			{
				string guidancePackageConfigurationFilename;

				public MockCustomExtension(string guidancePackageConfigurationFilename)
				{
					this.guidancePackageConfigurationFilename = guidancePackageConfigurationFilename;
				}

				public IDictionary<string, string> Attributes
				{
					get { throw new NotImplementedException(); }
				}

				public string ContentTypeName
				{
					get { return "GuidancePackage"; }
				}

				public string RelativePath
				{
					get { return Path.GetFileName(guidancePackageConfigurationFilename); }
				}

                public IList<System.Xml.XmlElement> AdditionalElements
                {
                    get { throw new NotImplementedException(); }
                }
            }


            public Version SchemaVersion
            {
                get { throw new NotImplementedException(); }
            }


            DateTimeOffset? IInstalledExtension.InstalledOn
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsPackComponent
            {
                get { throw new NotImplementedException(); }
            }

            public IList<System.Xml.XmlElement> AdditionalElements
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsProductSupported(string productId, Version version)
            {
                throw new NotImplementedException();
            }

            public string GetContentLocation(IExtensionContent content)
            {
                throw new NotImplementedException();
            }

            public IList<System.Xml.XmlElement> LocalizedAdditionalElements
            {
                get { throw new NotImplementedException(); }
            }

            public IEnumerable<IExtensionRequirement> Targets
            {
                get { throw new NotImplementedException(); }
            }

            public IExtensionInstallerInformation InstallerInformation => throw new NotImplementedException();

            public VsixType PackageType => throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public long GetLastExtensionsChangedTimestamp()
        {
            throw new NotImplementedException();
        }
    }
}

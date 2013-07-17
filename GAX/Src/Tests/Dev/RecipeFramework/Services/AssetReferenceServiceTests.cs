#region Using directives

using System;
using System.Collections;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.RecipeFramework;
using Microsoft.VisualStudio.ExtensionManager;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
	[TestClass]
	[DeploymentItem(@"Services\", @"Services\")]
	public class AssetReferenceServiceTests
	{
		RecipeManager manager;
		GuidancePackage package;

		[TestInitialize]
		public void SetUp()
		{
			manager = new RecipeManager();

			var extensionManager = new MockServices.MockExtensionManager();
			extensionManager.AddGuidancePackage(Utils.MakeTestRelativePath(@"Services\AssetReferenceServiceTests.xml"));

			manager.AddService(typeof(IPersistenceService), new MockServices.MockPersistenceService());
			manager.AddService(typeof(SVsExtensionManager), extensionManager);

			package = new GuidancePackage(Utils.MakeTestRelativePath(@"Services\AssetReferenceServiceTests.xml"));
			manager.EnablePackage(package);
		}

		[TestMethod]
		public void LoadDuplicateReferences()
		{
			IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);
			service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"));
			service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"));
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadDuplicateReferencesThrow()
        {
            IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);
            service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"), true);
            service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"), true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadDuplicateReferencesRangeThrow()
        {
            IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);
            service.AddRange(new IAssetReference[] {
                new MockObjects.MockReference("AddService", "/Solution/Services"),
                new MockObjects.MockReference("AddService", "/Solution/Services") },
                true);
        }

        [TestMethod]
        public void LoadDuplicateReferencesRange()
        {
            IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);
            service.AddRange(new IAssetReference[] {
                new MockObjects.MockReference("AddService", "/Solution/Services"),
                new MockObjects.MockReference("AddService", "/Solution/Services") });
        }

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void LoadNonReferenceComponent()
		{
			IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);
			((ComponentModel.ContainerComponent)service).Add(new System.ComponentModel.Component());
		}

		[TestMethod]
		public void IndexedByItem()
		{
			IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);

			service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"));
			service.Add(new MockObjects.MockReference("AddService", "/Solution/Services/Accounting"));
			service.Add(new MockObjects.MockReference("AddService", "/Solution/BusinessLayer"));
			service.Add(new MockObjects.MockReference("AddBusinessAction", "/Solution/BusinessLayer"));

			Assert.AreEqual(service.Find(typeof(IndexerBoundTarget), "/Solution/Services/Accounting").Length, 1);
			Assert.AreEqual(service.Find(typeof(IndexerBoundTarget), "/Solution/BusinessLayer").Length, 2);
		}

		[TestMethod]
		public void IndexedByRecipe()
		{
			IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);

			service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"));
			service.Add(new MockObjects.MockReference("AddService", "/Solution/Services/Accounting"));
			service.Add(new MockObjects.MockReference("AddService", "/Solution/BusinessLayer"));
			service.Add(new MockObjects.MockReference("AddBusinessAction", "/Solution/BusinessLayer"));
			service.Add(new MockObjects.MockReference("AddBusinessAction", "/Solution/BusinessLayer/BusinessActions"));

			Assert.AreEqual(service.Find(typeof(IndexerBoundAsset), "AddService").Length, 3);
			Assert.AreEqual(service.Find(typeof(IndexerBoundAsset), "AddBusinessAction").Length, 2);
		}

        //[TestMethod]
        //public void IndexedByRecipeAndItem()
        //{
        //    IAssetReferenceService service = (IAssetReferenceService)package.GetService(typeof(IAssetReferenceService), true);

        //    service.Add(new MockObjects.MockReference("AddService", "/Solution/Services"));
        //    service.Add(new MockObjects.MockReference("AddService", "/Solution/Services/Accounting"));
        //    service.Add(new MockObjects.MockReference("AddService", "/Solution/BusinessLayer"));
        //    service.Add(new MockObjects.MockReference("AddBusinessAction", "/Solution/BusinessLayer"));

        //    MockObjects.MockReference reference = new MockObjects.MockReference("AddBusinessAction", "/Solution/BusinessLayer/BusinessActions");
        //    service.Add(reference);

        //    Assert.IsNotNull(service.FindOne(typeof(IndexerBoundRecipeTarget), "AddService", "/Solution/Services"));
        //    Assert.AreSame(reference, service.FindOne(typeof(IndexerBoundRecipeTarget), 
        //        "AddBusinessAction", "/Solution/BusinessLayer/BusinessActions"));
        //}
	}
}

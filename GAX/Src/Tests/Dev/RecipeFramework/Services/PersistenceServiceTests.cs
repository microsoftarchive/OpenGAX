#region Using directives

using System;
using System.Collections;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
	[TestClass]
	public class PersistenceServiceTests
	{
		IDictionary _state;

		[TestInitialize]
		public void SetUp()
		{
			_state = new System.Collections.Specialized.HybridDictionary();
			_state.Add("Hi", 2344);
			_state.Add("DriveType", DriveType.Network);
			_state.Add("AnyString", "Hi there");
			_state.Add("Today", DateTime.Now);
		}

		private void RunProvider(IPersistenceService service)
		{
			service.SaveState("ShadowFax", new MockObjects.MockReference("AddBusinessAction", "Services/Accounting/GetAccountBalance.cs"), _state);

			IDictionary dict = service.LoadState("ShadowFax", new MockObjects.MockReference("AddBusinessAction", "Services/Accounting/GetAccountBalance.cs"));
			foreach (DictionaryEntry entry in dict)
			{
				Assert.AreEqual(_state[entry.Key], entry.Value);
			}
		}
	}
}

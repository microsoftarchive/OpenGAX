#region Using directives

using System;
using System.Collections;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockPersistenceService : Services.IPersistenceService
	{
		#region IPersistenceService Members

        public void ClearState(string packageName)
        {
        }

		public IDictionary LoadState(string packageName, IAssetReference reference)
		{
			return null;
		}

		public IDictionary RemoveState(string packageName, IAssetReference reference)
		{
            return null;
		}

		public void SaveState(string packageName, IAssetReference reference, System.Collections.IDictionary state)
		{
		}

		public IAssetReference[] LoadReferences(string packageName)
		{
			return new IAssetReference[0];
		}

        public void RemoveReferences(string packageName)
        {
		}

		public void SaveReferences(string packageName, IAssetReference[] references)
		{
		}

		#endregion
	}
}

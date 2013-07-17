using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockDictionaryService : IDictionaryService
	{
		Hashtable values = new Hashtable();

		#region IDictionaryService Members

		public object GetKey(object value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object GetValue(object key)
		{
			return values[key];
		}

		public void SetValue(object key, object value)
		{
			values[key] = value;
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel;
using Microsoft.Practices.Common.Services;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests.Mocks
{
    /// <summary>
    /// Simulates DictionaryService used by Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.
    /// </summary>
	internal class MockDictionaryService : Hashtable, IDictionaryService, IComponentChangeService
	{
		public MockDictionaryService()
		{
		}

		#region IComponentChangeService members

#pragma warning disable 0067
		public event ComponentEventHandler ComponentAdded;
		public event ComponentEventHandler ComponentAdding;
		public event ComponentEventHandler ComponentRemoved;
		public event ComponentEventHandler ComponentRemoving;
		public event ComponentRenameEventHandler ComponentRename;
#pragma warning restore 0067

		public event ComponentChangedEventHandler ComponentChanged;
		public event ComponentChangingEventHandler ComponentChanging;

		public void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue)
		{
			if (ComponentChanged != null && component is ValueInfo)
				ComponentChanged(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
		}

		public void OnComponentChanging(object component, MemberDescriptor member)
		{
			if (ComponentChanging != null && component is ValueInfo)
				ComponentChanging(this, new ComponentChangingEventArgs(component, member));
		}

		#endregion

		#region IDictionaryService Members

		public object GetKey(object value)
		{
			foreach (DictionaryEntry entry in this)
			{
				if (entry.Value != null && entry.Value.Equals(value))
				{
					return entry.Key;
				}
			}
			return null;
		}

		public object GetValue(object key)
		{
			return this[key];
		}

		public void SetValue(ValueInfo recipeArgument, object value)
		{
			if (ComponentChanging != null)
			{
				ComponentChanging(this, new ComponentChangingEventArgs(
					recipeArgument, null));
			}
			object oldValue = this[recipeArgument.Name];
			this[recipeArgument.Name] = value;
			if (ComponentChanged != null)
			{
				ComponentChanged(this, new ComponentChangedEventArgs(
					recipeArgument, null, oldValue, value));
			}
		}

		public void SetValue(object key, object value)
		{
			this[key] = value;
		}

		#endregion
	}
}

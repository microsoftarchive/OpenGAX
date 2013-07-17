//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Extensions
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

#region Using directives

using System;
using System.Collections;
using System.ComponentModel.Design;

#endregion Using directives

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Adapts the <see cref="IDictionaryService"/> interface to the 
	/// <see cref="IDictionary"/> one.
	/// </summary>
	/// <remarks>
	/// This adapter only implements the read functionality of a dictionary, 
	/// and throws <see cref="NotImplementedException"/> for all other members, 
	/// including those that enumerate or retrieve all keys in the dictionary, 
	/// as that functionality is not supported by the underlying <see cref="IDictionaryService"/>.
	/// <para>
	/// Supported methods are: <see cref="IDictionary.Contains"/> and <see cref="IDictionary.this"/> (indexer 
	/// only to retrieve a value).
	/// </para>
	/// </remarks>
	public sealed class ServiceAdapterDictionary : IDictionary
	{
		IDictionaryService service;

		/// <summary>
		/// Creates the adapter for the specified <see cref="IDictionaryService"/> service.
		/// </summary>
		/// <param name="serviceToAdapt">The service to adapt.</param>
		public ServiceAdapterDictionary(IDictionaryService serviceToAdapt)
		{
			this.service = serviceToAdapt;
		}

		#region IDictionary Members

		void IDictionary.Add(object key, object value)
		{
			throw new NotImplementedException();
		}

		void IDictionary.Clear()
		{
			throw new NotImplementedException();
		}

		bool IDictionary.Contains(object key)
		{
			// Not complete what Contains mean... but it's all we can do.
			return service.GetValue(key) != null;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		bool IDictionary.IsFixedSize
		{
			get { return true; }
		}

		bool IDictionary.IsReadOnly
		{
			get { return true; }
		}

		ICollection IDictionary.Keys
		{
			get { throw new NotImplementedException(); }
		}

		void IDictionary.Remove(object key)
		{
			throw new NotImplementedException();
		}

		ICollection IDictionary.Values
		{
			get { throw new NotImplementedException(); }
		}

		object IDictionary.this[object key]
		{
			get
			{
				return service.GetValue(key);
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		int ICollection.Count
		{
			get { throw new NotImplementedException(); }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

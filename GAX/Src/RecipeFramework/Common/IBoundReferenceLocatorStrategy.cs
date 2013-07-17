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

using System;

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// An ojbect implementing this interface will serve a <see cref="IBoundAssetReference"/> 
	/// as locator of the <see cref="IBoundAssetReference.Target"/> based on the <see cref="IBoundAssetReference.SubPath"/>
	/// </summary>
	public interface IBoundReferenceLocatorStrategy
	{
		/// <summary>
		/// Returns a friendly string describing the kind of objects in <see cref="IBoundAssetReference.Target"/>
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		string GetAppliesTo(object target);
		/// <summary>
		/// Returns the string used to represent the <see cref="IBoundAssetReference.Target"/> during serialization
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		string GetSerializationData(object target);
		/// <summary>
		/// Finds a valid object based on <paramref name="serializedData"/>
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="serializedData"></param>
		/// <returns>Returns null if the object is not found</returns>
		object LocateTarget(IServiceProvider serviceProvider, string serializedData);
	}
}

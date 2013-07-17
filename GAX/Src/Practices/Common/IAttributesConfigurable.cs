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

namespace Microsoft.Practices.Common
{
	/// <summary>
	/// Interface implemented by elements that allow arbitrary attributes to be 
	/// specified in the configuration file.
	/// </summary>
	/// <remarks>
	/// Only those elements that include an xs:anyAttribute in the XSD will be 
	/// able to receive configuration data.
	/// </remarks>
    public interface IAttributesConfigurable
	{
		/// <summary>
		/// Configures the component using the dictionary of attributes specified 
		/// in the configuration file.
		/// </summary>
		/// <param name="attributes">The attributes in the configuration element.</param>
        void Configure(System.Collections.Specialized.StringDictionary attributes);
	}
}

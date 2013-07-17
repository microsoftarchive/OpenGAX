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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
	/// <summary>
	///	Wrapper that exposes the <see cref="IServiceProvider"/> interface for the 
	/// VS automation object (a.k.a. the DTE).
	/// </summary>
	public class VsServiceProvider : IServiceProvider
	{
		private static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
		private Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider;

		/// <summary>
		///	Initializes the provider using the specified automation object.
		/// </summary>
		/// <param name="automationObject">The VS automation object.</param>
		/// <exception cref="ArgumentException">The <paramref name="automationObject"/> is not a valid COM service provider.</exception>
		public VsServiceProvider(object automationObject)
		{
			serviceProvider = automationObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
			if (serviceProvider == null)
			{
				throw new ArgumentException(Properties.Resources.InvalidComServiceProvider);
			}
		}

		/// <summary>
		/// Remove reference to the automation object.
		/// </summary>
		public virtual void Dispose()
		{
			if (serviceProvider != null)
			{
				serviceProvider = null;
			}

		}

		/// <summary>
		/// See <see cref="IServiceProvider.GetService"/>.
		/// </summary>
		public virtual object GetService(Type serviceType)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");

			return GetService(serviceType.GUID, serviceType);
		}

		private object GetService(Guid guid)
		{
			return GetService(guid, null);
		}

		private object GetService(Guid guid, Type serviceClass)
		{
			object service = null;

			// No valid guid on the passed in class, so there is no service for it.
			if (guid.Equals(Guid.Empty))
			{
				return null;
			}

			IntPtr pUnk;
			int hr = serviceProvider.QueryService(ref guid, ref IID_IUnknown, out pUnk);

			if (Succeeded(hr) && (pUnk != IntPtr.Zero))
			{
				service = Marshal.GetObjectForIUnknown(pUnk);
				Marshal.Release(pUnk);
			}

			return service;
		}

		private static bool Succeeded(int hr)
		{
			return (hr >= 0);
		}
	}
}

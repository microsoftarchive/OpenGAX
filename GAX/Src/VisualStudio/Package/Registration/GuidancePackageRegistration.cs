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
using System.Text;
using Microsoft.VisualStudio.Shell;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Registration
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	internal abstract class GuidancePackageRegistrationAttribute : RegistrationAttribute, IComparable
	{
		bool allowStandardRegistration;

		protected GuidancePackageRegistrationAttribute(bool allowStandardRegistration)
		{
			this.allowStandardRegistration = allowStandardRegistration;
		}

		internal GuidancePackageRegistrationAttribute()
		{
			allowStandardRegistration = false;
		}

		protected RegistrationAttribute.RegistrationContext Context
		{
			get { return context; }
		} RegistrationAttribute.RegistrationContext context = null;

		protected abstract void Register();

		protected abstract void Unregister();

		public override void Register(RegistrationAttribute.RegistrationContext context)
		{
			this.context = context;
			this.Register();
		}

		public override void Unregister(RegistrationAttribute.RegistrationContext context)
		{
			this.context = context;
			this.Unregister();
		}

		private int order;

		public int Order
		{
			get { return order; }
			set { order = value; }
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			GuidancePackageRegistrationAttribute attribute = obj as GuidancePackageRegistrationAttribute;

			if (attribute != null)
			{
				return this.Order.CompareTo(attribute.Order);
			}

			return 0;
		}

		#endregion
	}
}

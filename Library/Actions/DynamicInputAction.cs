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

#region Using Directives

using System;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using System.Collections;
using Microsoft.Practices.Common;
using System.Reflection;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// An Action that have a dynamic set of input properties
    /// </summary>
    public abstract class DynamicInputAction : ConfigurableAction, ICustomTypeDescriptor
    {
        #region Fields and Constructor

        /// <summary>
        /// Collection storing the set of custom input properties for this action
        /// </summary>
        protected IDictionary additionalArguments = new System.Collections.Hashtable();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicInputAction()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Wrapper container to store a <see cref="IDictionaryService"/>
        /// This dictionary adds the set of custom input properties
        /// </summary>
        protected Microsoft.Practices.ComponentModel.ServiceContainer WrappedContainer
        {
            get
            {
                if (wrappedContainer == null)
                {
                    // Build a wrapper container for the template.
                    Microsoft.Practices.ComponentModel.ServiceContainer container =
                        new Microsoft.Practices.ComponentModel.ServiceContainer();
                    // Site the container so it will ask services from the parent container.
                    container.Site = new Site((IServiceProvider)this.Site.Container, container, "DynamicInputAction");
                    // Add the wrapper service to new container.
                    container.AddService(typeof(IDictionaryService), new DictionaryWrapper(
                        this,
                        (IDictionaryService)GetService(typeof(IDictionaryService)),
                        additionalArguments));
                    wrappedContainer = container;
                }
                return wrappedContainer;
            }
        } Microsoft.Practices.ComponentModel.ServiceContainer wrappedContainer = null;

        #endregion

        #region ICustomTypeDescriptor Members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, attributes, true);
            PropertyDescriptor[] proparray = new PropertyDescriptor[properties.Count];
            properties.CopyTo(proparray, 0);
            // Wrap the properties in our custom collection.
            return new CustomProperties(proparray);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        #region Private implementation of ICustomTypeDescriptor

        private class CustomProperties : PropertyDescriptorCollection
        {
            public CustomProperties(PropertyDescriptor[] properties)
                : base(properties)
            {
            }

            public override PropertyDescriptor Find(string name, bool ignoreCase)
            {
                PropertyDescriptor prop = base.Find(name, ignoreCase);
                if (prop != null)
                {
                    return prop;
                }
                return new CustomProperty(name);
            }
        }

        private class CustomProperty : PropertyDescriptor
        {
            public CustomProperty(string name)
                : base(name, new Attribute[] { new InputAttribute() })
            {
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get { return this.GetType(); }
            }

            public override object GetValue(object component)
            {
                if (!(component is DynamicInputAction))
                {
                    throw new ArgumentException("component");
                }
                return ((DynamicInputAction)component).additionalArguments[base.Name];
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return typeof(string); }
            }

            public override void ResetValue(object component)
            {
            }

            public override void SetValue(object component, object value)
            {
                if (!(component is DynamicInputAction))
                {
                    throw new ArgumentException("component");
                }
                ((DynamicInputAction)component).additionalArguments[base.Name] = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }

        #endregion Private implementation of ICustomTypeDescriptor

        #region IDictionaryService wrapper

        private class DictionaryWrapper : IDictionaryService
        {
            IDictionaryService wrapped;
            IDictionary additional;
            DynamicInputAction parent;

            public DictionaryWrapper(DynamicInputAction parent, IDictionaryService wrapped, IDictionary additional)
            {
                this.parent = parent;
                this.wrapped = wrapped;
                this.additional = additional;
            }

            #region IDictionaryService Members

            public object GetKey(object value)
            {
                object key = wrapped.GetKey(value);
                if (key != null)
                {
                    return key;
                }
                foreach (DictionaryEntry entry in additional)
                {
                    if (entry.Value == value)
                    {
                        return entry.Key;
                    }
                }
                return null;
            }

            public object GetValue(object key)
            {
                object value = wrapped.GetValue(key);
                if (value != null)
                {
                    return value;
                }
                value = additional[key];
                if (value == null)
                {
                    BindingFlags bindingFlags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
                    PropertyInfo propInfo = parent.GetType().GetProperty((string)key, bindingFlags);
                    if (propInfo != null)
                    {
                        value = propInfo.GetValue(parent, null);
                    }
                }
                return value;
            }

            public void SetValue(object key, object value)
            {
                // Dictionary for actions is readonly, so we can only set values on 
                // the additional dictionary only.
                additional[key] = value;
            }

            #endregion
        }

        #endregion IDictionaryService wrapper
    }
}

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
using System.Text;
using System.ComponentModel;

namespace Microsoft.Practices.Common.Services
{
    /// <summary>
    /// Holds metadata about a value used.
    /// </summary>
    public class ValueInfo
    {
        string name;

        /// <summary>
        /// The name of the value.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        Type type;

        /// <summary>
        /// The Type of the value.
        /// </summary>
        public Type Type
        {
            get { return type; }
        } 

        bool isRequired;

        /// <summary>
        /// If <see langword="true"/>, the value is required and can't be null.
        /// </summary>
        public bool IsRequired
        {
            get { return isRequired; }
        }

        TypeConverter typeConverter;

        /// <summary>
        /// The TypeConverter for the value.
        /// </summary>
        /// <remarks>
        /// .NET types usually have type converters associated by means of the 
        /// <see cref="TypeConverterAttribute"/>. This property allows to override 
        /// the default converter for a type (i.e. provide more meaningful converter 
        /// for a string value that holds a file name).
        /// </remarks>
        public TypeConverter Converter
        {
            get { return typeConverter; }
        } 

        /// <summary>
        /// <see cref="ValueInfo"/> constructor.
        /// </summary>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="required">If <see langword="true"/>, the value is required and can't be null.</param>
        /// <param name="type">The Type of the value.</param>
        /// <param name="converter">The TypeConverter for the value.</param>
        public ValueInfo(string valueName, bool required, Type type, TypeConverter converter)
        {
            this.name = valueName;
            this.isRequired = required;
            this.type = type;
            this.typeConverter = converter;
        }

        /// <summary>
        /// Returns the name of the value.
        /// </summary>
        /// <example>
        /// <code>AddService.FolderLocation</code>
        /// </example>
        public override string ToString()
        {
            return name;
        }
    }
}

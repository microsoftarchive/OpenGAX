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
using System.Collections;

namespace Microsoft.Practices.Common
{
    /// <summary>
    /// Dictionary of strings that has the exact same behavior of the 
    /// built-in <see cref="System.Collections.Specialized.StringDictionary"/> 
    /// (case-insensitive keys), but which doesn't modify the keys.
    /// </summary>
    public class ImmutableKeyStringDictionary : System.Collections.Specialized.StringDictionary
    {
        // Fields
        internal Hashtable contents = new Hashtable(
            new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());

        #region Dummy overrides

        /// <summary>
        /// Adds an element to the dictionary.
        /// </summary>
        public override void Add(string key, string value) { contents.Add(key, value); }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.Clear"/>.
        /// </summary>
        public override void Clear() { contents.Clear(); }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.ContainsKey"/>.
        /// </summary>
        public override bool ContainsKey(string key) { return contents.ContainsKey(key); }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.ContainsValue"/>.
        /// </summary>
        public override bool ContainsValue(string value) { return contents.ContainsValue(value); }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.CopyTo"/>.
        /// </summary>
        public override void CopyTo(Array array, int index) { contents.CopyTo(array, index); }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.GetEnumerator"/>.
        /// </summary>
        public override IEnumerator GetEnumerator() { return contents.GetEnumerator(); }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.Remove"/>.
        /// </summary>
        public override void Remove(string key) { contents.Remove(key); }

        // Properties
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.Count"/>.
        /// </summary>
        public override int Count { get { return contents.Count; } }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.IsSynchronized"/>.
        /// </summary>
        public override bool IsSynchronized { get { return contents.IsSynchronized; } }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.this[string]"/>.
        /// </summary>
        public override string this[string key]
        {
            get { return (string)contents[key]; }
            set { contents[key] = value; }
        }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.Keys"/>.
        /// </summary>
        public override ICollection Keys { get { return contents.Keys; } }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.SyncRoot"/>.
        /// </summary>
        public override object SyncRoot { get { return contents.SyncRoot; } }
        /// <summary>
        /// See <see cref="System.Collections.Specialized.StringDictionary.Values"/>.
        /// </summary>
        public override ICollection Values { get { return contents.Values; } }

        #endregion Dummy overrides
    }
}

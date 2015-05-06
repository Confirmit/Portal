// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.UI;

namespace Controls
{
    /// <summary>
    /// The SimpleTabHeaderCollection is used to wrap the SimpleTabContainer.Controls collection
    /// and provide an AccordionPane only view.
    /// </summary>
    public sealed class SimpleTabHeaderCollection: IList, IEnumerable<SimpleTabHeader>
    {
        /// <summary>
        /// Parent SimpleTabContainer whose Controls collection we are filtering
        /// </summary>
        private readonly SimpleTabContainer _parent;

        /// <summary>
        /// Counter used to prevent modification of the collection during enumeration
        /// </summary>
        private int _version;

        /// <summary>
        /// Constructor to associate the collection with an Accordion
        /// </summary>
        /// <param name="parent">Parent Accordion</param>
        internal SimpleTabHeaderCollection(SimpleTabContainer parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "Parent SimpleTabContainer cannot be null.");

            _parent = parent;
        }

        /// <summary>
        /// Number of SimpleTabHeader in the parent SimpleTabContainer's Controls collection
        /// </summary>
        public int Count
        {
            get
            {
                int headers = 0;
                foreach (Control control in _parent.Controls)
                {
                    if (control is SimpleTabHeader)
                        headers++;
                }
                return headers;
            }
        }

        /// <summary>
        /// The collection is not read-only so this always returns false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Index the SimpleTabHeader, or raise an ArgumentException if
        /// the index is invalid
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>AccordionPane</returns>
        public SimpleTabHeader this[int index]
        {
            get { return _parent.Controls[ToRawIndex(index)] as SimpleTabHeader; }
        }

        /// <summary>
        /// Index the SimpleTabHeader by their Control.IDs.  We will return null
        /// if the desired header is not found.
        /// </summary>
        /// <param name="id">SimpleTabHeader Control ID</param>
        /// <returns>SimpleTabHeader, or null if not found</returns>
        public SimpleTabHeader this[string id]
        {
            get
            {
                for (int i = 0; i < _parent.Controls.Count; i++)
                {
                    SimpleTabHeader header = _parent.Controls[i] as SimpleTabHeader;
                    if (header!= null && header.ID == id)
                        return header;
                }

                return null;
            }
        }

        /// <summary>
        /// Since the SimpleTabContainer.Controls collection may contain other controls than
        /// just SimpleTabHeader, we need to adjust the index for these additional controls.
        /// </summary>
        /// <param name="headerIndex">Index of the desired SimpleTabHeader</param>
        /// <returns>Raw index in the SimpleTabContainer.Controls collection</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Assembly is not localized")]
        private int ToRawIndex(int headerIndex)
        {
            if (headerIndex < 0)
                return -1;

            int headerCount = -1;
            for (int i = 0; i < _parent.Controls.Count; i++)
            {
                if (_parent.Controls[i] is SimpleTabHeader && ++headerCount == headerIndex)
                    return i;
            }

            throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "No SimpleTabHeader at position {0}", headerIndex));
        }

        /// <summary>
        /// Given an index in the parent SimpleTabContainer.Controls collection, we determine
        /// its index in the collection of SimpleTabHeader
        /// </summary>
        /// <param name="index">Index in the Controls collection</param>
        /// <returns>Index in the SimpleTabHeaderCollection</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Assembly is not localized")]
        private int FromRawIndex(int index)
        {
            if (index < 0)
                return -1;

            int headerCount = -1;
            for (int i = 0; i < _parent.Controls.Count; i++)
            {
                if (_parent.Controls[i] is SimpleTabHeader)
                    headerCount++;

                if (index == i)
                    return headerCount;
            }
            throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "No SimpleTabHeader at position {0}", index));
        }

        /// <summary>
        /// Add a new AccordionPane to the collection
        /// </summary>
        /// <param name="item">AccordionPane</param>
        public void Add(SimpleTabHeader item)
        {
            item.SetOwner(_parent);
            _parent.Controls.Add(item);
            _version++;
        }

        /// <summary>
        /// Clear the AccordionPanes in the collection
        /// </summary>
        public void Clear()
        {
            _parent.ClearHeaders();
            _version++;
        }

        /// <summary>
        /// Check if the collection contains the desired SimpleTabHeader
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(SimpleTabHeader item)
        {
            return _parent.Controls.Contains(item);
        }

        /// <summary>
        /// Copy the collection into an array
        /// </summary>
        /// <param name="array">Array (of AccordionPanes)</param>
        /// <param name="index">Index to begin copying</param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Assembly is not localized")]
        public void CopyTo(Array array, int index)
        {
            SimpleTabHeader[] panes = array as SimpleTabHeader[];
            if (panes == null)
                throw new ArgumentException("Expected an array of SimpleTabHeader.");

            CopyTo(panes, index);
        }

        /// <summary>
        /// Copy the collection into an array
        /// </summary>
        /// <param name="array">Arrray</param>
        /// <param name="index">Index to begin copying</param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Assembly is not localized")]
        public void CopyTo(SimpleTabHeader[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array", "Cannot copy into a null array.");

            int offset = 0;
            for (int i = 0; i < _parent.Controls.Count; i++)
            {
                SimpleTabHeader header = _parent.Controls[i] as SimpleTabHeader;
                if (header != null)
                {
                    if (offset + index == array.Length)
                        throw new ArgumentException("Array is not large enough for the AccordionPanes");

                    array[offset++ + index] = header;
                }
            }
        }

        /// <summary>
        /// Get the index of the SimpleTabHeader in the list
        /// </summary>
        /// <param name="item">AccordionPane</param>
        /// <returns>Index of the AccordionPane</returns>
        public int IndexOf(SimpleTabHeader item)
        {
            return FromRawIndex(_parent.Controls.IndexOf(item));
        }

        /// <summary>
        /// Insert a new SimpleTabHeader at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">AccordionPane to insert</param>
        public void Insert(int index, SimpleTabHeader item)
        {
            _parent.Controls.AddAt(ToRawIndex(index), item);
            _version++;
        }

        /// <summary>
        /// Remove an SimpleTabHeader from the collection
        /// </summary>
        /// <param name="item">SimpleTabHeader</param>
        /// <returns>True if we were able to remove, false otherwise</returns>
        public void Remove(SimpleTabHeader item)
        {
            _parent.Controls.Remove(item);
            _version++;
        }

        /// <summary>
        /// Remove the SimpleTabHeader at the given index from the collection
        /// </summary>
        /// <param name="index">Index of the SimpleTabHeader to remove</param>
        public void RemoveAt(int index)
        {
            _parent.Controls.RemoveAt(ToRawIndex(index));
            _version++;
        }

        /// <summary>
        /// Add an SimpleTabHeader to the list
        /// </summary>
        /// <param name="value">SimpleTabHeader</param>
        /// <returns>Always returns 0</returns>
        int IList.Add(object value)
        {
            Add(value as SimpleTabHeader);
            return 0;
        }

        /// <summary>
        /// Check if the list contains the AccordionPane
        /// </summary>
        /// <param name="value">AccordionPane</param>
        /// <returns>True if it contains the pane, false otherwise</returns>
        bool IList.Contains(object value)
        {
            return Contains(value as SimpleTabHeader);
        }

        /// <summary>
        /// Get the inded of the provided SimpleTabHeader
        /// </summary>
        /// <param name="value">SimpleTabHeader</param>
        /// <returns>Index of the SimpleTabHeader</returns>
        int IList.IndexOf(object value)
        {
            return IndexOf(value as SimpleTabHeader);
        }

        /// <summary>
        /// Insert an SimpleTabHeader at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">SimpleTabHeader</param>
        void IList.Insert(int index, object value)
        {
            Insert(index, value as SimpleTabHeader);
        }

        /// <summary>
        /// The collection is not a fixed size, so this
        /// always returns false
        /// </summary>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Remove an SimpleTabHeader from the list
        /// </summary>
        /// <param name="value">AccordionPane</param>
        void IList.Remove(object value)
        {
            Remove(value as SimpleTabHeader);
        }

        /// <summary>
        /// Get an SimpleTabHeader given its index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>SimpleTabHeader</returns>
        object IList.this[int index]
        {
            get { return this[index]; }
            set { }
        }

        /// <summary>
        /// This collection is not synchronized, so it always returns false
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// This collection is not synchronized, so this always throws a
        /// NotImplementedException
        /// </summary>
        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Get an enumerator for the collection
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SimpleTabHeaderEnumerator(this);
        }

        /// <summary>
        /// Get an enumerator for the collection
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<SimpleTabHeader> GetEnumerator()
        {
            return new SimpleTabHeaderEnumerator(this);
        }

        /// <summary>
        /// Enumerator for the AccordionPaneCollection
        /// </summary>
        private class SimpleTabHeaderEnumerator : IEnumerator<SimpleTabHeader>
        {
            /// <summary>
            /// Reference to the collection
            /// </summary>
            private SimpleTabHeaderCollection _collection;

            /// <summary>
            /// Enumerator for the parent SimpleTabContainer.Controls collection
            /// </summary>
            private IEnumerator _parentEnumerator;

            /// <summary>
            /// Version of the collection when we began enumeration
            /// (used to check for modifications of the collection)
            /// </summary>
            private int _version;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="parent">SimpleTabHeaderCollection</param>
            public SimpleTabHeaderEnumerator(SimpleTabHeaderCollection parent)
            {
                _collection = parent;
                _parentEnumerator = parent._parent.Controls.GetEnumerator();
                _version = parent._version;
            }

            /// <summary>
            /// Ensure the collection has not been modified while enumerating
            /// </summary>
            [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Assembly is not localized")]
            private void CheckVersion()
            {
                if (_version != _collection._version)
                    throw new InvalidOperationException("Enumeration can't continue because the collection has been modified.");
            }

            /// <summary>
            /// Dispose of the enumerator
            /// </summary>
            public void Dispose()
            {
                _parentEnumerator = null;
                _collection = null;
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Current AccordionPane
            /// </summary>
            public SimpleTabHeader Current
            {
                get
                {
                    CheckVersion();
                    return _parentEnumerator.Current as SimpleTabHeader;
                }
            }

            /// <summary>
            /// Current AccordionPane
            /// </summary>
            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Move to the next SimpleTabHeader
            /// </summary>
            /// <returns>True if we were able to move, false otherwise</returns>
            public bool MoveNext()
            {
                CheckVersion();
                bool result = _parentEnumerator.MoveNext();
                if (result && !(_parentEnumerator.Current is SimpleTabHeader))
                    result = MoveNext();
                return result;
            }

            /// <summary>
            /// Reset the enumerator to the beginning of the list
            /// </summary>
            public void Reset()
            {
                CheckVersion();
                _parentEnumerator.Reset();
            }
        }
    }
}
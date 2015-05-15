using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace UlterSystems.PortalLib
{
	/// <summary>
	/// Class of cache supporting timings.
	/// </summary>
	public class Cache
	{
		#region Classes
		/// <summary>
		/// Class of item in cache.
		/// </summary>
		public class CacheItem
		{
			#region Fields
			private object m_Value;
			private DateTime m_InsertDate;
			private DateTime m_LastGetDate;
			#endregion

			#region Properties
			/// <summary>
			/// Object in cache.
			/// </summary>
			public object Value
			{
				get 
				{
					m_LastGetDate = DateTime.Now;
					return m_Value; 
				}
				set
				{
					m_Value = value;
					m_InsertDate = DateTime.Now;
				}
			}

			/// <summary>
			/// Time when object was inserted into cache.
			/// </summary>
			public DateTime InsertDate
			{
				get { return m_InsertDate; }
				set { m_InsertDate = value; }
			}

			/// <summary>
			/// Time when object was last obtained from cache.
			/// </summary>
			public DateTime LastGetDate
			{
				get { return m_LastGetDate; }
				set { m_LastGetDate = value; }
			}
			#endregion
		}
		#endregion

		#region Fields
		private static Dictionary<object, CacheItem> m_Cache = new Dictionary<object, CacheItem>();
		#endregion

		#region Properties
		#endregion

		#region Constructors
		#endregion

		#region Methods
		/// <summary>
		/// Clears cache.
		/// </summary>
		public static void Clear()
		{
			Debug.Assert(m_Cache != null);
			m_Cache.Clear();
		}

		/// <summary>
		/// Returns object from cache.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <returns>Object from cache.</returns>
		public static object GetObject(object key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			Debug.Assert(m_Cache != null);
			if (!m_Cache.ContainsKey(key))
				throw new Exception();

			return m_Cache[key].Value;
		}

		/// <summary>
		/// Adds object into cache.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public static void Add(object key, object value)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			CacheItem item = new CacheItem();
			item.Value = value;

			Debug.Assert(m_Cache != null);

			m_Cache[key] = item;
		}

		/// <summary>
		/// Removes object from cache.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <returns>Was object removed.</returns>
		public static bool Remove(object key)
		{
			if( key == null )
				throw new ArgumentNullException("key");

			Debug.Assert(m_Cache != null);
			return m_Cache.Remove(key);
		}

		/// <summary>
		/// Does the cache contain key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <returns>Does the cache contain key.</returns>
		public static bool Contains(object key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			Debug.Assert(m_Cache != null);
			return m_Cache.ContainsKey(key);
		}

		/// <summary>
		/// Return insertion time of object.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <returns>Insertion time of object.</returns>
		public static DateTime? InsertDate(object key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			Debug.Assert(m_Cache != null);
			if (!m_Cache.ContainsKey(key))
			{ return null; }
			else
			{ return m_Cache[key].InsertDate; }
		}

		/// <summary>
		/// Returns last get time of object.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <returns>Last get time of object.</returns>
		public static DateTime? LastGetDate(object key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			Debug.Assert(m_Cache != null);
			if (!m_Cache.ContainsKey(key))
			{ return null; }
			else
			{ return m_Cache[key].LastGetDate; }
		}
		#endregion
	}

    /// <summary>
    /// Class of cache.
    /// </summary>
    /// <typeparam name="TKey">Type of cache keys.</typeparam>
    /// <typeparam name="TValue">Type of cache items.</typeparam>
    public class Cache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields
        private readonly Dictionary<TKey, TValue> m_InternalCache;
        /// <summary>
        /// Function for determinatoin if cache is enabled.
        /// </summary>
        private readonly Func<bool> m_IsEnabledFunc;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="isCacheEnabledFunc">Function for determinatoin if cache is enabled.</param>
        /// <remarks>If isCacheEnabledFunc is null cache will be enabled.</remarks>
        public Cache(Func<bool> isCacheEnabledFunc)
        {
            m_IsEnabledFunc = isCacheEnabledFunc;
            if (m_IsEnabledFunc == null)
                m_IsEnabledFunc = delegate { return true; };

            m_InternalCache = new Dictionary<TKey,TValue>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="isCacheEnabledFunc">Function for determinatoin if cache is enabled.</param>
        /// <param name="comparer">Equality comparer.</param>
        /// <remarks>If isCacheEnabledFunc is null cache will be enabled.</remarks>
        public Cache(Func<bool> isCacheEnabledFunc, IEqualityComparer<TKey> comparer)
        {
            m_IsEnabledFunc = isCacheEnabledFunc;
            if (m_IsEnabledFunc == null)
                m_IsEnabledFunc = delegate { return true; };

            m_InternalCache = new Dictionary<TKey, TValue>(comparer);
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds item to cache.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (m_IsEnabledFunc())
            {
                m_InternalCache.Add(key, value);
            }
        }

        /// <summary>
        /// Does cache contain key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>True if cache contains key; false, otherwise.</returns>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (m_IsEnabledFunc())
            { return m_InternalCache.ContainsKey(key); }
            else
            { return false; }
        }

        /// <summary>
        /// Keys of cache.
        /// </summary>
        public ICollection<TKey> Keys
        {
            [DebuggerStepThrough]
            get { return m_InternalCache.Keys; }
        }

        /// <summary>
        /// Removes from cache item by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Is item removed.</returns>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return m_InternalCache.Remove(key);
        }

        /// <summary>
        /// Tryes to get item from cache.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Return value.</param>
        /// <returns>True if value was returned; false, otherwise.</returns>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (m_IsEnabledFunc())
            { return m_InternalCache.TryGetValue(key, out value); }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Collection of values.
        /// </summary>
        public ICollection<TValue> Values
        {
            [DebuggerStepThrough]
            get { return m_InternalCache.Values; }
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Value with key.</returns>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        /// <exception cref="NotImplementedException">Cache is not enabled.</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                if (m_IsEnabledFunc())
                { return m_InternalCache[key]; }
                else
                { throw new NotImplementedException(); }
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                if (m_IsEnabledFunc())
                { m_InternalCache[key] = value; }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds item to cache.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <summary>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            m_InternalCache.Clear();
        }

        /// <summary>
        /// Does cache contain item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>True if cache contains item; false, otherwise.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (m_IsEnabledFunc())
            { return ((ICollection<KeyValuePair<TKey, TValue>>)m_InternalCache).Contains(item); }
            else
            { return false; }
        }

        /// <summary>
        /// Copies content of cache to array.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="arrayIndex">Starting index of arra.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)m_InternalCache).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Count of items.
        /// </summary>
        public int Count
        {
            [DebuggerStepThrough]
            get { return m_InternalCache.Count; }
        }

        /// <summary>
        /// Is cache readonly.
        /// </summary>
        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)m_InternalCache).IsReadOnly; }
        }

        /// <summary>
        /// Removes item from cache.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if item was removed; false, otherwise.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)m_InternalCache).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        /// <returns>Enumerator of cache items.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_InternalCache.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        /// <returns>Enumerator of cache items.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_InternalCache.GetEnumerator();
        }

        #endregion
    }
}

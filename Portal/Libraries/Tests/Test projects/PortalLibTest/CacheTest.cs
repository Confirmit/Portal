using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UlterSystems.PortalLib;

namespace Confirmit.Portal.PortalLib.Test
{
    [TestClass]
    public class CacheEnabledTest
    {
        private Cache<string, string> m_Cache;

        [TestInitialize()]
        public void TestInitialize()
        {
            m_Cache = new Cache<string, string>(delegate { return true; });
        }

        [TestMethod]
        public void Cache_Constructor_NullComparer_Ok()
        {
            m_Cache = new Cache<string, string>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Add_NullKey_ArgumentNullException()
        {
            m_Cache.Add(null, "A");
        }

        [TestMethod]
        public void Cache_Add_NotNullKey_Ok()
        {
            m_Cache.Add("A", "A");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ContainsKey_NullKey_ArgumentNullException()
        {
            bool res = m_Cache.ContainsKey(null);
        }

        [TestMethod]
        public void Cache_ContainsKey_NoItem_False()
        {
            Assert.IsFalse(m_Cache.ContainsKey("A"));
        }

        [TestMethod]
        public void Cache_ContainsKey_HasItem_True()
        {
            m_Cache.Add("A", "A");
            Assert.IsTrue(m_Cache.ContainsKey("A"));
        }

        [TestMethod]
        public void Cache_Keys_NotNull()
        {
            Assert.IsNotNull(m_Cache.Keys);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Remove_NullKey_ArgumentNullException()
        {
            bool res = m_Cache.Remove(null);
        }

        [TestMethod]
        public void Cache_Remove_NoItem_False()
        {
            Assert.IsFalse(m_Cache.Remove("A"));
        }

        [TestMethod]
        public void Cache_Remove_HasItem_True()
        {
            m_Cache.Add("A", "A");
            Assert.IsTrue(m_Cache.Remove("A"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_TryGetValue_NullKey_ArgumentNullException()
        {
            string val;
            bool res = m_Cache.TryGetValue(null, out val);
        }

        [TestMethod]
        public void Cache_TryGetValue_NoItem_False()
        {
            string val;
            Assert.IsFalse(m_Cache.TryGetValue("A", out val));
            Assert.IsNull(val);
        }

        [TestMethod]
        public void Cache_TryGetValue_HasItem_True()
        {
            string val;
            m_Cache.Add("A", "A");
            Assert.IsTrue(m_Cache.TryGetValue("A", out val));
            Assert.AreEqual("A", val);
        }

        [TestMethod]
        public void Cache_Values_NotNull()
        {
            Assert.IsNotNull(m_Cache.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Indexer_Get_NullKey_ArgumentNullException()
        {
            string val = m_Cache[null];
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Cache_Indexer_Get_NoItem_KeyNotFoundException()
        {
            string val = m_Cache["A"];
        }

        [TestMethod]
        public void Cache_Indexer_Get_HasItem_True()
        {
            m_Cache.Add("A", "A");
            Assert.AreEqual("A", m_Cache["A"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Indexer_Set_NullKey_ArgumentNullException()
        {
            m_Cache[null] = "A";
        }

        [TestMethod]
        public void Cache_Indexer_Set_Ok()
        {
            m_Cache["A"] = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ICollection_Add_NullKey_ArgumentNullException()
        {
            ((ICollection<KeyValuePair<string, string>>)m_Cache).Add(new KeyValuePair<string, string>(null, "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Add_NotNullKey_Ok()
        {
            ((ICollection<KeyValuePair<string, string>>)m_Cache).Add(new KeyValuePair<string, string>("A", "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Clear_Ok()
        {
            m_Cache.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ICollection_Contains_NullKey_ArgumentNullException()
        {
            bool res = ((ICollection<KeyValuePair<string, string>>)m_Cache).Contains(new KeyValuePair<string, string>(null, "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Contains_NoItem_False()
        {
            Assert.IsFalse(((ICollection<KeyValuePair<string, string>>)m_Cache).Contains(new KeyValuePair<string, string>("A", "A")));
        }

        [TestMethod]
        public void Cache_ICollection_Contains_HasItem_True()
        {
            m_Cache.Add("A", "A");
            Assert.IsTrue(((ICollection<KeyValuePair<string, string>>)m_Cache).Contains(new KeyValuePair<string, string>("A", "A")));
        }

        [TestMethod]
        public void Cache_ICollection_CopyTo_Ok()
        {
            KeyValuePair<string, string>[] arr = new KeyValuePair<string, string>[m_Cache.Count];
            m_Cache.CopyTo(arr, 0);
        }

        [TestMethod]
        public void Cache_ICollection_Count_NoItems_Zero()
        {
            Assert.AreEqual(0, m_Cache.Count);
        }

        [TestMethod]
        public void Cache_ICollection_Count_HasOneItems_One()
        {
            m_Cache.Add("A", "A");
            Assert.AreEqual(1, m_Cache.Count);
        }

        [TestMethod]
        public void Cache_ICollection_IsReadOnly_False()
        {
            Assert.IsFalse(m_Cache.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ICollection_Remove_NullKey_ArgumentNullException()
        {
            bool res = ((ICollection<KeyValuePair<string, string>>)m_Cache).Remove(new KeyValuePair<string, string>(null, "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Remove_NoItem_False()
        {
            Assert.IsFalse(((ICollection<KeyValuePair<string, string>>)m_Cache).Remove(new KeyValuePair<string, string>("A", "A")));
        }

        [TestMethod]
        public void Cache_ICollection_Remove_HasItem_True()
        {
            m_Cache.Add("A", "A");
            Assert.IsTrue(((ICollection<KeyValuePair<string, string>>)m_Cache).Remove(new KeyValuePair<string, string>("A", "A")));
        }

        [TestMethod]
        public void Cache_GetEnumerator_NotNull()
        {
            Assert.IsNotNull(m_Cache.GetEnumerator());
        }
    }

    [TestClass]
    public class CacheDisabledTest
    {
        private Cache<string, string> m_Cache;

        [TestInitialize()]
        public void TestInitialize()
        {
            m_Cache = new Cache<string, string>(delegate { return false; });
        }
        [TestMethod]
        public void Cache_Constructor_NullComparer_Ok()
        {
            m_Cache = new Cache<string, string>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Add_NullKey_ArgumentNullException()
        {
            m_Cache.Add(null, "A");
        }

        [TestMethod]
        public void Cache_Add_NotNullKey_Ok()
        {
            m_Cache.Add("A", "A");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ContainsKey_NullKey_ArgumentNullException()
        {
            bool res = m_Cache.ContainsKey(null);
        }

        [TestMethod]
        public void Cache_ContainsKey_False()
        {
            m_Cache.Add("A", "A");
            Assert.IsFalse(m_Cache.ContainsKey("A"));
        }

        [TestMethod]
        public void Cache_Keys_ZeroLength()
        {
            Assert.IsNotNull(m_Cache.Keys);
            Assert.AreEqual(0, m_Cache.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Remove_NullKey_ArgumentNullException()
        {
            bool res = m_Cache.Remove(null);
        }

        [TestMethod]
        public void Cache_Remove_False()
        {
            m_Cache.Add("A", "A");
            Assert.IsFalse(m_Cache.Remove("A"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_TryGetValue_NullKey_ArgumentNullException()
        {
            string val;
            bool res = m_Cache.TryGetValue(null, out val);
        }

        [TestMethod]
        public void Cache_TryGetValue_False()
        {
            m_Cache.Add("A", "A");
            string val;
            Assert.IsFalse(m_Cache.TryGetValue("A", out val));
            Assert.IsNull(val);
        }

        [TestMethod]
        public void Cache_Values_ZeroLength()
        {
            Assert.IsNotNull(m_Cache.Values);
            Assert.AreEqual(0, m_Cache.Values.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Indexer_Get_NullKey_ArgumentNullException()
        {
            string val = m_Cache[null];
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Cache_Indexer_Get_NotImplementedException()
        {
            string val = m_Cache["A"];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_Indexer_Set_NullKey_ArgumentNullException()
        {
            m_Cache[null] = "A";
        }

        [TestMethod]
        public void Cache_Indexer_Set_Ok()
        {
            m_Cache["A"] = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ICollection_Add_NullKey_ArgumentNullException()
        {
            ((ICollection<KeyValuePair<string, string>>)m_Cache).Add(new KeyValuePair<string, string>(null, "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Add_NotNullKey_Ok()
        {
            ((ICollection<KeyValuePair<string, string>>)m_Cache).Add(new KeyValuePair<string, string>("A", "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Clear_Ok()
        {
            m_Cache.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ICollection_Contains_NullKey_False()
        {
            Assert.IsFalse( ((ICollection<KeyValuePair<string, string>>)m_Cache).Contains(new KeyValuePair<string, string>(null, "A")) );
        }

        [TestMethod]
        public void Cache_ICollection_Contains_False()
        {
            m_Cache.Add("A", "A");
            Assert.IsFalse(((ICollection<KeyValuePair<string, string>>)m_Cache).Contains(new KeyValuePair<string, string>("A", "A")));
        }

        [TestMethod]
        public void Cache_ICollection_CopyTo_Ok()
        {
            KeyValuePair<string, string>[] arr = new KeyValuePair<string, string>[m_Cache.Count];
            m_Cache.CopyTo(arr, 0);
        }

        [TestMethod]
        public void Cache_ICollection_Count_Zero()
        {
            m_Cache.Add("A", "A");
            Assert.AreEqual(0, m_Cache.Count);
        }

        [TestMethod]
        public void Cache_ICollection_IsReadOnly_False()
        {
            Assert.IsFalse(m_Cache.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cache_ICollection_Remove_NullKey_ArgumentNullException()
        {
            bool res = ((ICollection<KeyValuePair<string, string>>)m_Cache).Remove(new KeyValuePair<string, string>(null, "A"));
        }

        [TestMethod]
        public void Cache_ICollection_Remove_False()
        {
            m_Cache.Add("A", "A");
            Assert.IsFalse(((ICollection<KeyValuePair<string, string>>)m_Cache).Remove(new KeyValuePair<string, string>("A", "A")));
        }

        [TestMethod]
        public void Cache_GetEnumerator_NotNull_Empty()
        {
            Assert.IsNotNull(m_Cache.GetEnumerator());
            Assert.IsFalse(m_Cache.GetEnumerator().MoveNext());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections;

namespace Core
{
    /// <summary>
    /// Класс, отвечающий за кэширование данных в пределах библиотеки
    /// </summary>
    public class CacheManager
    {
        public delegate System.Collections.IDictionary RequestCacheCallback();

        public static RequestCacheCallback RequestCache;

        private static Hashtable m_staticHash = new Hashtable();

        public static System.Collections.IDictionary Cache
        {
            get
            {
                if (RequestCache != null)
                {
                    IDictionary requestedCache = RequestCache();
                    return (requestedCache == null) ? m_staticHash : requestedCache;
                }
                else
                {
                    return m_staticHash;
                }
            }
        }
    }
}
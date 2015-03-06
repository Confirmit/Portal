using System;
using System.Reflection;

namespace Helpers.Reflection
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Return value of property.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Object value.</returns>
        public static Object GetPropertyValue(Object source, String propertyName)
        {
            if (source == null || String.IsNullOrEmpty(propertyName))
                return null;

            MemberInfo[] mInfo = source.GetType().FindMembers(MemberTypes.Property,
                                                           BindingFlags.Public | BindingFlags.Instance,
                                                           DelegateToSearchCriteria,
                                                           propertyName);

            return (mInfo.Length == 0)
                       ? null
                       : ((PropertyInfo) mInfo[0]).GetValue(source, null);
        }

        private static bool DelegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
        {
            return objMemberInfo.Name.ToString() == objSearch.ToString();
        }
    }
}

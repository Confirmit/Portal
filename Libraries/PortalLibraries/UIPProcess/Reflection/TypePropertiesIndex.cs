using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace UIPProcess.Reflection
{
    public class TypePropertiesIndex<TPropertyType>
    {
        public void FillIndex(Object entity)
        {
            Type clazz = entity.GetType();
            MemberInfo[] arrTypeProps = clazz.FindMembers(
                MemberTypes.Property,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                new MemberFilter(delegateToSearchCriteria),
                typeof(TPropertyType));

            foreach (MemberInfo member in arrTypeProps)
            {
                PropertyInfo property = member as PropertyInfo;
                Debug.Assert(property != null);

                if (!mapProperties.ContainsKey(property.Name))
                {
                    mapProperties.Add(property.Name, property);
                    continue;
                }

                if (property.DeclaringType.IsSubclassOf(
                    ((PropertyInfo)mapProperties[property.Name]).DeclaringType))
                    mapProperties[property.Name] = property;
            }
        }

        private static bool delegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
        {
            PropertyInfo infoProp = objMemberInfo as PropertyInfo;
            if (infoProp == null)
                return false;

            if (infoProp.PropertyType == (Type)objSearch)
                return true;
            else 
                return false;
        }

        private IDictionary<String, PropertyInfo> mapProperties = new Dictionary<String, PropertyInfo>();
        public IDictionary<String, PropertyInfo> Properties
        {
            get { return mapProperties; }
            set { mapProperties = value; }
        }
    }
}
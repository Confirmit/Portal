using System;
using System.Collections.Generic;
using System.Reflection;

using UIProcess;

namespace UIPProcess.UIP.Views
{
    internal class ViewKeysMapper
    {        
        public static void MapKeysToView(ViewSettings viewSettings, Object view
                                         , State state)
        {
            IDictionary<StateKeySettings, String> mapKeysProps = viewSettings.MapKeysProperties;
            foreach (KeyValuePair<StateKeySettings, String> key_property in mapKeysProps)
            {
                String keyName = key_property.Key.Name;
                String propertyName = key_property.Value;

                PropertyInfo propViewData = FindPropertyByName(view, propertyName);

                if (propViewData == null || propViewData.CanWrite == false)
                    throw new Exception(string.Format("ExceptionIncorrectViewPropertyMapping: {0} - {1} - {2}."
                                                      , viewSettings.Name, propertyName, keyName));

                try
                {
                    propViewData.SetValue(view, state[keyName], null);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }
        
        private static PropertyInfo FindPropertyByName(Object entity, String strName)
        {
            Type clazz = entity.GetType();
            MemberInfo[] arrTypeMembers = clazz.FindMembers(MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance
                                                            , delegateToSearchCriteria, strName);

            if (arrTypeMembers == null || arrTypeMembers.Length == 0)
                return null;

            return arrTypeMembers[0] as PropertyInfo;
        }

        private static bool delegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
        {
            return objMemberInfo.Name.Equals(objSearch.ToString());
        }
    }
}
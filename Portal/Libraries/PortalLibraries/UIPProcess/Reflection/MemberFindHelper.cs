using System;
using System.Reflection;

namespace UIPProcess.Reflection
{
    public class MemberFindHelper
    {        
        static public MemberInfo FindPropertyByName(Object entity, String strName)
        {
            Type clazz = entity.GetType();
            MemberInfo[] arrTypeMembers = clazz.FindMembers(
                MemberTypes.Property,
                BindingFlags.Public | BindingFlags.Instance,
                delegateToSearchCriteria, strName);

            if (arrTypeMembers == null || arrTypeMembers.Length == 0)
                return null;

            return arrTypeMembers[0];
        }

        private static bool delegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
        {
            return objMemberInfo.Name.ToString().Equals(objSearch.ToString());
        }
    }
}
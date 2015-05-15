using System;
using System.Reflection;

namespace UIPProcess.Reflection
{
    public class InterfaceFindHelper
    {
        static public Type FindInterfaceByName(Type type, Type interfaceToFind)
        {
            try {
                TypeFilter myFilter = new TypeFilter(delegateToSearchByName);

                Type[] interfaces = type.FindInterfaces(myFilter
                                                        , interfaceToFind.ToString());
                if (interfaces.Length > 0) 
                    return interfaces[0];

                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        private static Boolean delegateToSearchByName(Type typeObj, Object criteriaObj)
        {
            if(typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }
    }
}
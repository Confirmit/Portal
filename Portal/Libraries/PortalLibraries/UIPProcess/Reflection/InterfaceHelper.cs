using System;
using System.Collections.Generic;
using System.Reflection;

namespace UIPProcess.Reflection
{
    public class InterfaceHelper
    {
        /// <summary>
        /// получить список названий properties
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String[] GetStringsProperties(Type type)
        {
            IList<PropertyInfo> listP = type.GetProperties();

            String[] listS = new String[listP.Count];

            foreach (PropertyInfo infoProperty in listP)
                listS.SetValue(infoProperty.Name, listP.IndexOf(infoProperty));

            Array.Sort(listS);

            return listS;
        }

        /// <summary>
        /// получить список названий properties
        /// </summary>
        /// <param name="nameInterface"></param>
        /// <param name="nameAssembly"></param>
        /// <returns></returns>
        public static String[] GetStringsProperties(String nameInterface, String nameAssembly)
        {
            Type type = Assembly.Load(nameAssembly).GetType(nameInterface, false);

            if (type == null)
                return null;

            return GetStringsProperties(type);
        }

        public static PropertyInfo[] GetProperties(Type IType, Type clazz)
        {
            PropertyInfo[] pi = IType.GetProperties();
            Type[] interfaces = clazz.FindInterfaces(new TypeFilter(MyInterfaceFilter), IType);

            if (interfaces.Length == 0)
                return pi;

            PropertyInfo[] piChild = interfaces[0].GetProperties();
            if (piChild.Length == 0)
                return pi;

            PropertyInfo[] res = new PropertyInfo[pi.Length + piChild.Length];
            pi.CopyTo(res, 0);
            piChild.CopyTo(res, pi.Length);
            return res;
        }

        public static bool MyInterfaceFilter(Type typeObj, Object criteriaObj)
        {
            Type[] pi = typeObj.GetInterfaces();
            String str = ((Type)criteriaObj).Name.ToString();

            foreach (Type interf in pi)
                if (interf.Name.ToString().Equals(str))
                    return true;

            return false;
        }
    }
}
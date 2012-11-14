using System;
using System.Reflection;
using Core.Exceptions;

namespace Core
{
	/// <summary>
	/// ��������������� ������ ��� Reflection'�.
	/// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// �������� �������� ����������� ����� ����.
        /// ���������� �� Type.InvokeMethod ���, ��� ����� ���� ����� �� ���� �������� ������.
        /// </summary>
        /// <param name="type">���.</param>
        /// <param name="name">��� ������.</param>
        /// <param name="args">��������� ������.</param>
        /// <returns>������������ �������� ������.</returns>
        public static object InvokeStaticMethod(Type type, string name, object[] args)
        {
            // �������� ����� ����� ��������������� � ����. ���� �� �������, 
            // �� ��������� � �������� ����.
            MethodInfo method = null;
            for (Type currentType = type; currentType != null; currentType = currentType.BaseType)
            {
                method = currentType.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
                if (method != null)
                    break;
            }

            // �������� �����.
            if (method != null)
                return method.Invoke(null, args);

            // ����� �� ������.
            throw new CoreMissingMethodException(Resources.ResourceManager.GetString("MethodException", method));
        }

        /// <summary>
        /// Get property value  of object.
        /// </summary>
        /// <param name="obj">Object to searcg property.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Value of property.</returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (propInfo.Name.Equals(propertyName))
                    return propInfo.GetValue(obj, null);
            }

            return null;
        }
    }
}

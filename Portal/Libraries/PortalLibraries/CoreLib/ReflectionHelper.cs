using System;
using System.Reflection;
using Core.Exceptions;

namespace Core
{
	/// <summary>
	/// Вспомогательные методы для Reflection'а.
	/// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// Вызывает открытый статический метод типа.
        /// Отличается от Type.InvokeMethod тем, что метод ищет вверх по всей иерархии класса.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <param name="name">Имя метода.</param>
        /// <param name="args">Аргументы вызова.</param>
        /// <returns>Возвращаемое значение метода.</returns>
        public static object InvokeStaticMethod(Type type, string name, object[] args)
        {
            // Пытаемся найти метод непосредственно у типа. Если не находим, 
            // то переходим к базовому типу.
            MethodInfo method = null;
            for (Type currentType = type; currentType != null; currentType = currentType.BaseType)
            {
                method = currentType.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
                if (method != null)
                    break;
            }

            // Вызываем метод.
            if (method != null)
                return method.Invoke(null, args);

            // Метод не найден.
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

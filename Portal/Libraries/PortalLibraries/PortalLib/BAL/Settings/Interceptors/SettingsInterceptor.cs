using System;
using System.Collections.Generic;
using System.Reflection;

using Castle.Core.Interceptor;

namespace ConfirmIt.PortalLib.BAL.Settings.Interceptors
{
    /// <summary>
    /// Общий перехватчик для любых классов настроек. Хранит кеш для известных настроек. 
    /// Если требуется прочитать новую настройку, которой нет в кеше, или сохранить настройку в БД, отдает управление наследнику
    /// </summary>
    public abstract class SettingsInterceptor : IInterceptor
    {
        #region Fields

        /// <summary>
        /// Кеш для хранения уже известных настроек, чтобы не обращаться лишний раз к БД
        /// </summary>
        protected IDictionary<string, ISetting> m_Cache = new Dictionary<string, ISetting>();

        #endregion

        public void Intercept(IInvocation invocation)
        {
            MethodInfo method = invocation.Method;
            string propertyName = method.Name.Substring(4, method.Name.Length - 4);

            if (method.Name.Equals("Load"))
            {
                SettingAttribute loadAttr = (SettingAttribute)invocation.Arguments[0];
                invocation.ReturnValue = LoadSetting(loadAttr, true);
                return;
            }

            if (method.Name.Equals("GetSettingValue"))
            {
                SettingAttribute loadAttr = (SettingAttribute)invocation.Arguments[0];
                invocation.ReturnValue = LoadSetting(loadAttr, true).Value;
                return;
            }

            if (method.Name.Equals("Save"))
            {
                SettingAttribute loadAttr = (SettingAttribute)invocation.Arguments[0];
                saveAndCacheSetting(loadAttr, invocation.Arguments[1]);
                return;
            }

            //Извлекаем атрибут перехваченного свойства
            SettingAttribute attribute = null;
            PropertyInfo propInfo = invocation.Method.DeclaringType.GetProperty(propertyName);
            if (propInfo != null)
            {
                object[] attributes = propInfo.GetCustomAttributes(typeof(SettingAttribute), false);
                if (attributes.Length != 0)
                    attribute = attributes[0] as SettingAttribute;
            }

            if (string.IsNullOrEmpty(propertyName) || attribute == null)
            {
                invocation.Proceed();
                return;
            }

            if (method.Name.StartsWith("get_"))
            {
                ISetting result = LoadSetting(attribute, false);
                if (result == null)
                {
                    if (attribute is PersonalSettingAttribute && ((PersonalSettingAttribute)attribute).HasGlobalAnalogue)
                    {
                        invocation.ReturnValue = convertStringToMethodType(method, (PersonalSettings.GetGlobalAnalogueOfPersonalSetting(attribute)).ToString());
                        return;
                    }
                    else
                    {
                        invocation.Proceed();
                        return;
                    }
                }

                result.Value = convertStringToMethodType(method, result.Value.ToString());
                m_Cache[attribute.SettingName] = result;
                invocation.ReturnValue = convertStringToMethodType(method, result.Value.ToString());

                return;
            }

            if (method.Name.StartsWith("set_"))
            {
                saveAndCacheSetting(attribute, invocation.Arguments[0]);
                return;
            }
        }

        #region Load setting

        protected ISetting LoadSetting(SettingAttribute attribute)
        {
            return LoadSetting(attribute, true);
        }

        protected ISetting LoadSetting(SettingAttribute attribute, bool storeInCache)
        {
            ISetting setting = null;
            if (!m_Cache.TryGetValue(attribute.SettingName, out setting))
            {
                setting = GetSetting(attribute);
                if (storeInCache && setting != null)
                    m_Cache[attribute.SettingName] = setting;
            }
            return setting;
        }

        protected abstract ISetting GetSetting(SettingAttribute attribute);

        #endregion

        #region Save setting

        private void saveAndCacheSetting(SettingAttribute attribute, object value)
        {
            ISetting saved_setting = SaveSetting(attribute, value);
            if (saved_setting != null)
                m_Cache[attribute.SettingName] = saved_setting;
        }

        protected abstract ISetting SaveSetting(SettingAttribute attribute, object value);

        #endregion

        private object convertStringToMethodType(MethodInfo method, string value)
        {
            switch (Type.GetTypeCode(method.ReturnType))
            {
                case TypeCode.String:
                    return value;

                case TypeCode.Int32:
                    return Int32.Parse(value);

                case TypeCode.Boolean:
                    return Boolean.Parse(value);

                case TypeCode.DateTime:
                    return DateTime.Parse(value);

                default:
                    return TimeSpan.Parse(value);
            }
        }
    }
}
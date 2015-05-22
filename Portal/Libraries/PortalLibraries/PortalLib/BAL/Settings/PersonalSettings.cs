using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    /// <summary>
    /// Персональные (пользовательские) настройки
    /// </summary>
    /// В этом классе нужно лишь объявить пустые свойства-настройки с атрибутом PersonalSetting.
    /// Всю работу выполняет перехватчик PersonSettingsInterceptor.
    /// Свойства получат управление только если перехватчик не справится со своей задачей
    public class PersonalSettings : BaseSettingCollection
    {
        #region List of Personal Settings

        /// <summary>
        /// Язык по умолчанию
        /// </summary>
        [PersonalSetting("DefaultCulture")]
        public virtual string DefaultCulture
        {
            get
            {
                //Если язык по умолчанию для пользователя не указан, ориентируемся на его версию Windows
                string currentCultureID;
                if (System.Web.HttpContext.Current != null)
                {
                    currentCultureID = System.Web.HttpContext.Current.Request.UserLanguages[0];
                    if (currentCultureID.Length > 2)
                        currentCultureID = currentCultureID.Substring(0, 2);
                }
                else
                    currentCultureID = "en";

                return currentCultureID;
            }
            set { }
        }

        /// <summary>
        /// Размер строк в таблице по умолчанию
        /// </summary>
        [PersonalSetting("DefaultPageSize")]
        public virtual int DefaultPageSize
        {
            get { return 5; }
            set { }
        }

        /// <summary>
        /// Вид преобразования даты в строку
        /// </summary>
        [PersonalSetting("DefaultDateFormat", GlobalAnalogue = "DefaultDateFormat")]
        public virtual string DefaultDateFormat
        {
            get
            {                
                return string.Empty;
                //Если эта настройка для пользователя не указана, вернём значение соответствующей глобальной настройки
                //return GlobalSettings.Instance.DefaultDateFormat;
            }
            set { }
        }

        /// <summary>
        /// Вид преобразования времени в строку
        /// </summary>
        [PersonalSetting("DefaultTimeFormat", GlobalAnalogue = "DefaultTimeFormat")]
        public virtual string DefaultTimeFormat
        {
            get
            {
                return string.Empty;
                //Если эта настройка для пользователя не указана, вернём значение соответствующей глобальной настройки
                //return GlobalSettings.Instance.DefaultTimeFormat;
            }
            set { }
        }

        #endregion

        #region StaticMethods

        /// <summary>
        /// Проверить, совпадает ли значение персональной (пользовательской) настройки со значением аналогичной глобальной настройки
        /// </summary>
        /// <param name="attribute">Атрибут персональной настройки</param>
        /// <param name="value">Значение персональной настройки</param>
        /// <returns>true - если значение персональной и глобальной настроек совпадают, false - в противном случае</returns>
        public static bool IsGlobalEqual(SettingAttribute attribute, object value)
        {
            try
            {
                object global_value = GetGlobalAnalogueOfPersonalSetting(attribute);
                if (string.Equals(global_value.ToString(), value.ToString()))
                    return true;
            }
            catch
            {}

            return false;
        }

        /// <summary>
        /// Получить атрибут персональной настройки ( PersonalSettingAttribute ) по ключу
        /// </summary>
        /// <param name="attribute">Атрибут персональной настройки</param>
        /// <returns>PersonalSettingAttribute - если настройка была найдена среди свойств класса PersonalSettings, null - в противном случае</returns>
        public static PersonalSettingAttribute GetPersonalSettingAttribute(SettingAttribute attribute)
        {
            PersonalSettingAttribute person_attribute = attribute as PersonalSettingAttribute;
            if (person_attribute != null)
            {
                PersonalSettingAttribute result = new PersonalSettingAttribute(person_attribute.SettingName);
                result.GlobalAnalogue = person_attribute.GlobalAnalogue;
                return result;
            }
            else
                return attribute.SettingType == SettingType.Personal
                        ? GetPersonalSettingAttribute(attribute.SettingName)
                        : null;
        }

        /// <summary>
        /// Получить атрибут персональной настройки ( PersonalSettingAttribute ) по ключу
        /// </summary>
        /// <param name="setting_name">Имя настройки</param>
        /// <returns>PersonalSettingAttribute - если настройка была найдена среди свойств класса PersonalSettings, null - в противном случае</returns>
        public static PersonalSettingAttribute GetPersonalSettingAttribute(string setting_name)
        {
            PropertyInfo property = GetPropertyBySettingAttributeName(typeof(PersonalSettings), setting_name);
            if (property != null)
            {
                object[] prop_attr = property.GetCustomAttributes(typeof(PersonalSettingAttribute), false);
                if (prop_attr.Length > 0)
                {
                    PersonalSettingAttribute result = new PersonalSettingAttribute(((PersonalSettingAttribute)prop_attr[0]).SettingName);
                    result.GlobalAnalogue = ((PersonalSettingAttribute)prop_attr[0]).GlobalAnalogue;
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Получить значение глобального аналога персональной настройки
        /// </summary>
        /// <param name="attribute">Атрибут персональной настройки</param>
        /// <returns>Значение глобального аналога персональной настройки</returns>
        public static object GetGlobalAnalogueOfPersonalSetting(SettingAttribute attribute)
        {
            try
            {
                PersonalSettingAttribute person_attr = GetPersonalSettingAttribute(attribute);
                if (person_attr != null && person_attr.HasGlobalAnalogue)
                {
                    PropertyInfo global_property = GetPropertyBySettingAttributeName(typeof(GlobalSettings), person_attr.GlobalAnalogue);
                    if (global_property != null)
                        return global_property.GetValue(GlobalSettings.Instance, null);
                }
            }
            catch
            { }

            return new object();
        }

        /// <summary>
        /// Получить свойство-настройку по имени
        /// </summary>
        /// <param name="type">Тип обьекта, свойство которого необходимо получить</param>
        /// <param name="name">Имя искомой настройки ( которое должно быть указано в SettingAttribute.Name )</param>
        /// <returns>Свойство ( PropertyInfo ), у которого есть арибут SettingAttribute и SettingAttribute.Name = name</returns>
        private static PropertyInfo GetPropertyBySettingAttributeName(Type type, string name)
        {
            BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
            foreach (PropertyInfo property in type.GetProperties(flags))
            {
                object[] prop_attr = property.GetCustomAttributes(typeof(SettingAttribute), false);
                if (prop_attr.Length > 0 && ((SettingAttribute)prop_attr[0]).SettingName == name)
                    return property;
            }
            return null;
        }

        #endregion

        #region ToList method

        //public override IList<ISetting> ToList(SettingType settingType)
        //{
        //    return (IList<ISetting>)ToList<PersonalSettings, PersonalSettingEntity>(SettingType.Personal);
        //}

        public override IList<ISetting> ToList(SettingType type)
        {
            if (type != SettingType.Personal)
                throw new Exception("PersonSettingsInterceptor can't handle non-personal settings");

            List<PersonalSettingEntity> setings_col = new List<PersonalSettingEntity>();
            BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

            try
            {
                foreach (PropertyInfo property in typeof(PersonalSettings).GetProperties(flags))
                {
                    object[] attr_obj = property.GetCustomAttributes(typeof(PersonalSettingAttribute), false);
                    if (attr_obj.Length == 0)
                        continue;

                    object set_value = property.GetValue(this, null);
                    ISetting setting = Load((PersonalSettingAttribute)attr_obj[0]);
                    if (setting == null)
                        setings_col.Add(new PersonalSettingEntity((PersonalSettingAttribute)attr_obj[0], set_value));
                    else
                        setings_col.Add((PersonalSettingEntity)setting);

                    //if (!m_Cache.TryGetValue(((PersonalSettingAttribute)attr_obj[0]).SettingName, out setting))
                    //    setings_col.Add(new PersonalSettingEntity((PersonalSettingAttribute)attr_obj[0], set_value));
                    //else
                    //    setings_col.Add((PersonalSettingEntity)setting);
                }
            }
            catch
            {
                return new List<ISetting>();
            }

            return setings_col.ToArray();
        }

        #endregion
    }
}
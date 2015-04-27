using System;
using System.Reflection;
using System.Collections.Generic;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BAL.Settings.Interceptors
{
    /// <summary>
    /// Перехватчик для класса PersonSettings.
    /// Обеспечивает функционал для работы с локальными (пользовательскими) настройками
    /// </summary>
    public class PersonalSettingsInterceptor : SettingsInterceptor
    {
        #region Fields

        /// <summary>
        /// Пользователь, которому принадлежат настройки
        /// </summary>
        private Person m_Person = null;

        #endregion

        #region Constructor

        public PersonalSettingsInterceptor(Person person)
        {
            m_Person = person;
        }

        #endregion

        #region GetSetting

        protected override ISetting GetSetting(SettingAttribute person_attribute)
        {
            if (person_attribute.SettingType != SettingType.Personal)
                throw new Exception("PersonSettingsInterceptor can't handle non-personal settings");
            
            IList<PersonAttribute> person_attrs = 
                                PersonAttributes.GetPersonAttributesByKeyword((int)m_Person.ID, person_attribute.SettingName);

            return person_attrs.Count != 0 ? new PersonalSettingEntity(person_attribute, person_attrs[0]) : null;
        }

        #endregion

        #region SaveSetting

        protected override ISetting SaveSetting(SettingAttribute person_attribute, object value)
        {
            if (person_attribute.SettingType != SettingType.Personal)
                throw new Exception("PersonSettingsInterceptor can't handle non-personal settings");

            //Возможно, к настройке уже обращались, тогда она есть в кеше
            ISetting setting = null;
            if (!m_Cache.TryGetValue(person_attribute.SettingName, out setting))
                setting = GetSetting(person_attribute);//Если в кеше записи нет, то попробуем взять из таблицы

            if (setting == null)//Если такой записи в таблице нет, создадим её                
            {
                //Если значение пользовательской настройки совпадает со значением аналогичной глобальной настройки, 
                //то хранить ее для пользователя не будем 
                if (PersonalSettings.IsGlobalEqual(person_attribute, value))
                    return null;

                PersonAttribute attr = m_Person.AddStandardStringAttribute(person_attribute.SettingName, value.ToString());
                return new PersonalSettingEntity(person_attribute, attr);
            }
            else
            {
                //Если новое значение пользовательской настройки совпадает со значением аналогичной глобальной настройки, 
                //то просто удалим для этого пользователя эту настройку 
                if (PersonalSettings.IsGlobalEqual(person_attribute, value))
                    m_Person.RemoveStandardAttributes(PersonAttributeType.GetAttributeType(person_attribute.SettingName));

                ((PersonalSettingEntity)setting).Value = value.ToString();
                (((PersonalSettingEntity)setting).PersonAttribute).Save();
                return setting;
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    /// <summary>
    /// Возможные типы настроек
    /// </summary>
    public enum SettingType
    {
        /// <summary>
        /// Глобальная настройка, значение хранится в таблице Settings БД Portal
        /// </summary>
        Global = 1,

        /// <summary>
        /// Глобальная форумная настройка, значение хранится в таблице Settings БД Portal
        /// </summary>
        Forum,

        /// <summary>
        /// URL сервиса к офису, значение хранится в таблице Offices БД Portal
        /// </summary>
        Office,

        /// <summary>
        /// Персональная (пользовательская) настройка, значение хранится в таблице PersonAttributes БД Portal
        /// </summary>
        Personal,
        None
    }

    /// <summary>
    /// Атрибут любой настройки (свойства класса настроек)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingAttribute : Attribute
    {
        /// <summary>
        /// Создает атрибут настройки заданного типа и имени
        /// </summary>
        /// <param name="type">Тип настройки</param>
        /// <param name="name">Имя соответствующей настройки в таблице настроек</param>
        public SettingAttribute(SettingType settingType, string settingName)
        {
            m_SettingType = settingType;
            m_SettingName = settingName;
        }

        #region Fields

        private string m_SettingName = string.Empty;
        private SettingType m_SettingType;

        #endregion

        /// <summary>
        /// Тип настройки
        /// </summary>
        public SettingType SettingType
        {
            get { return m_SettingType; }
        }

        /// <summary>
        /// Имя соответствующей настройки в таблице настроек
        /// </summary>
        public string SettingName
        {
            get { return m_SettingName; }
        }
    }

    /// <summary>
    /// Атрибут пользовательской настройки (свойства класса пользовательских настроек)
    /// Отличается от SettingAttribute тем, что имеет поле GlobalAnalogue 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PersonalSettingAttribute : SettingAttribute
    {
        /// <summary>
        /// Создает атрибут персональной настройки заданного имени
        /// </summary>
        /// <param name="name">Имя соответствующей настройки в таблице настроек</param>
        public PersonalSettingAttribute(string name)
            : base(SettingType.Personal, name)
        { }

        #region Fields

        private string m_globalAnalogue = string.Empty;

        #endregion

        /// <summary>
        /// Имя глобальной настройки в таблице настроек, которую перекрывает данная пользовательская настройка.
        /// Является именованным параметром, т.к. пользовательская настройка может не иметь глобального аналога
        /// </summary>
        public string GlobalAnalogue
        {
            get { return m_globalAnalogue; }
            set { m_globalAnalogue = value; }
        }

        /// <summary>
        /// Есть ли у настройки глобальный аналог
        /// </summary>
        public bool HasGlobalAnalogue
        {
            get { return !string.IsNullOrEmpty(GlobalAnalogue); }
        }
    }
}

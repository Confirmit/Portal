using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    /// <summary>
    /// Персональная (пользовательская) настройка
    /// Хранит в себе экземпляр PersonAttribute 
    /// </summary>
    internal class PersonalSettingEntity : ISetting
    {
        #region Fields

        private PersonAttribute m_PersonAttribute = new PersonAttribute();
        private PersonalSettingAttribute m_PersonalSettingAttribute = null;

        #endregion

        #region Constructors

        private PersonalSettingEntity()
        {}

        public PersonalSettingEntity(SettingAttribute setting_attr, object value)
        {
            m_PersonAttribute.StringField = value.ToString();
            if (!setPersonSettingAttributeField(setting_attr))
                throw new Exception("Invalid setting type of personal setting.");
        }

        public PersonalSettingEntity(SettingAttribute setting_attr, PersonAttribute person_attr)
        {
            m_PersonAttribute = person_attr;
            if (!setPersonSettingAttributeField(setting_attr))
                throw new Exception("Invalid setting type of personal setting.");
        }

        #endregion

        #region Properties

        public PersonAttribute PersonAttribute
        {
            get { return m_PersonAttribute; }
        }

        public bool IsSaved
        {
            get { return PersonAttribute.IsSaved; }
        }

        public bool IsGlobalEqual(object value)
        {
            return PersonalSettings.IsGlobalEqual(SettingAttribute, value);
        }

        #region ISetting Members

        public string KeyColumnName
        {
            get { return string.Empty; }
        }

        public SettingAttribute SettingAttribute
        {
            get { return m_PersonalSettingAttribute; }
        }

        public object Value
        {
            get { return PersonAttribute.StringField; }
            set { PersonAttribute.StringField = value.ToString(); }
        }

        #endregion

        #endregion

        #region Methods

        private bool setPersonSettingAttributeField(SettingAttribute attr)
        {
            PersonalSettingAttribute person_attr = PersonalSettings.GetPersonalSettingAttribute(attr);
            if (person_attr == null)
                return false;

            m_PersonalSettingAttribute = person_attr;
            return true;
        }

        #endregion
    }
}

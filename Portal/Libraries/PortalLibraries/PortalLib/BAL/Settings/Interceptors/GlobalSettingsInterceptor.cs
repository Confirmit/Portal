using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Core;
using office = UlterSystems.PortalLib.BusinessObjects.Office;

namespace ConfirmIt.PortalLib.BAL.Settings.Interceptors
{
    /// <summary>
    /// Перехватчик для класса GlobalSettings.
    /// Обеспечивает функционал для работы с глобальными настройками
    /// </summary>
    public class GlobalSettingsInterceptor : SettingsInterceptor
    {
        #region Get Setting

        protected override ISetting GetSetting(SettingAttribute attribute)
        {
            validateSettingType(attribute.SettingType);
            return loadSetting(attribute);
        }

        private ISetting loadSetting(SettingAttribute attribute)
        {
            ISetting setting = null;
            switch (attribute.SettingType)
            {
                case SettingType.Global:
                case SettingType.Forum:
                    {
                        setting = new GlobalSettingEntity(attribute);
                        break;
                    }

                case SettingType.Office:
                    {
                        setting = new office();
                        break;
                    }
            }

            // TODO: 
            ((BasePlainObject)setting).LoadByReference(setting.KeyColumnName, attribute.SettingName);
            return setting;
        }

        #endregion

        #region Save setting

        protected override ISetting SaveSetting(SettingAttribute attribute, object value)
        {
            validateSettingType(attribute.SettingType);

            ISetting setting = null;
            if (!m_Cache.TryGetValue(attribute.SettingName, out setting))
                setting = loadSetting(attribute);

            setting.Value = value.ToString();
            ((BasePlainObject) setting).Save();
            
            return setting;
        }

        #endregion

        private void validateSettingType(SettingType settingType)
        {
            if (settingType != SettingType.Global && settingType != SettingType.Forum && settingType != SettingType.Office)
                throw new Exception("GlobalSettingsInterceptor can't handle " + settingType + " settings");
        }
    }
}

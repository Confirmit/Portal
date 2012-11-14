using System;
using System.Collections;
using System.Collections.Generic;

using Core;
using ConfirmIt.PortalLib.BAL.Settings;
using UlterSystems.PortalLib.BusinessObjects;

public partial class PersonSettingsControl : BaseSettingUserControl
{
    private PersonalSettings m_Settings = new PersonalSettings();
 
    public PersonSettingsControl()
    {
        m_Settings = Person.Current.PersonSettings;
        SettingCollection = m_Settings.ToList(SettingType.Personal);
    }

    public void Save()
    {
        Save(m_Settings);
        ChangeCulture();
    }

    public void Cancel()
    {
        Page.RedirectToMySelf();
    }

    protected void ChangeCulture()
    {
        try
        {
            MLText.CurrentCultureID = m_Settings.DefaultCulture;
            Page.RedirectToMySelf();
        }
        catch (Exception ex)
        {
            ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
        }
    }
}
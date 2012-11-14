using Core;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BAL.Settings;

public partial class OfficesSettings : BaseSettingUserControl
{
    public OfficesSettings()
    {
        SettingCollection = GlobalSettings.Instance.ToList(SettingType.Office);
    }
}
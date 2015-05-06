using ConfirmIt.PortalLib.BAL.Settings;
using Core;

public partial class MainSettings : BaseSettingUserControl
{
    public MainSettings()
    {
        SettingCollection = GlobalSettings.Instance.ToList(SettingType.Global);
    }
}

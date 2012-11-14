using ConfirmIt.PortalLib.BAL.Settings;
using Core;

public partial class ForumSettings : BaseSettingUserControl
{
    public ForumSettings()
    {
        SettingCollection = GlobalSettings.Instance.ToList(SettingType.Forum);
    }
}

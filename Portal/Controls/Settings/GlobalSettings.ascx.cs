using System;

using ConfirmIt.PortalLib.BAL.Settings;

public partial class Admin_GlobalSettings : BaseUserControl
{
    private GlobalSettings settings
    {
        get { return GlobalSettings.Instance; }
    }

    /// <summary>
    /// Обработчик нажатия на кнопку Apply.
    /// </summary>
    protected void btnApply_Click(object sender, EventArgs e)
    {
        mainCommonSettings.Save(settings);
        forumSettings.Save(settings);
        officesSettings.Save(settings);
    }
}

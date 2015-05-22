public partial class PersonSettingsPage : BaseWebPage
{
    protected void btnApply_Click(object sender, System.EventArgs e)
    {
        personSettings.Save();
    }
    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        personSettings.Cancel();
    }
}

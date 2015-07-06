using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Controls.GroupsControls
{
    public partial class GroupSettingsControl : UserControl
    {
        public TextBox GroupName
        {
            get { return GroupNameTextBox; }
        }

        public TextBox GroupDescription
        {
            get { return GroupDescriptionTextBox; }
        }
    }
}
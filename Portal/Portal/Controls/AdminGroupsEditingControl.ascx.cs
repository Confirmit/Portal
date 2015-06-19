using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.Rules;

namespace Portal.Controls
{
    public partial class AdminGroupsEditingControl : UserControl
    {
        public event EventHandler<SelectedObjectEventArgs> EventHandler;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //var groupRepository = new GroupRepository();
                //var currentListOfGroups = groupRepository.GetAllGroups();

                //GroupsEditingGridView.DataSource = currentListOfGroups;
                //GroupsEditingGridView.DataBind();
            }
        }

        protected virtual void OnUserChanging()
        {
            if (SelectedGroupID == -1)
                throw new Exception("Selected user id equals -1.");

            if (EventHandler != null && SelectedGroupID != -1)
                EventHandler(this, new SelectedObjectEventArgs { ObjectID = SelectedGroupID });
        }

        private int SelectedGroupID
        {
            get
            {
                return GroupsEditingGridView.SelectedDataKey == null
                           ? -1
                           : (int)GroupsEditingGridView.SelectedDataKey.Value;
            }
        }

        protected void GroupsEditingGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OnUserChanging();
        }
    }
}
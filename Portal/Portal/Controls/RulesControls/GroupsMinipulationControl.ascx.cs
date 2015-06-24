using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class GroupsMinipulationControl : UserControl
    {
        private int CurrentRuleId
        {
            get { return ViewState["CurrentRuleId"] is int ? (int)ViewState["CurrentRuleId"] : -1; }
            set { ViewState["CurrentRuleId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Visible = false;
            }
            AddGroupsFromRuleButton.Click += AddUsersFromGroupButtonOnClick;
            RemoveGroupsFromRuleButton.Click += RemoveUsersFromGroupButtonOnClick;

            GroupsListContainingInCurrentInRuleControl.GetGroupsForBindingFunction += GetPersonsContainingInGroup;
            GroupsListNotContainingInCurrentInRuleControl.GetGroupsForBindingFunction += GetPersonsotNotContainingInGroup;
        }

        private void AddUsersFromGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var idsSelectedGroups = GroupsListNotContainingInCurrentInRuleControl.GetIdsSelectedGroups();
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.AddGroupIdsToRule(CurrentRuleId, idsSelectedGroups.ToArray());

            GroupsListContainingInCurrentInRuleControl.BindGroupsInRule();
            GroupsListNotContainingInCurrentInRuleControl.BindGroupsInRule();
        }

        private void RemoveUsersFromGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var idsSelectedGroups = GroupsListContainingInCurrentInRuleControl.GetIdsSelectedGroups();
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.DeleteGroupIdsFromRule(CurrentRuleId, idsSelectedGroups.ToArray());

            GroupsListContainingInCurrentInRuleControl.BindGroupsInRule();
            GroupsListNotContainingInCurrentInRuleControl.BindGroupsInRule();
        }

        public void OnRuleChanging(SelectedObjectEventArgs e)
        {
            Visible = true;
            CurrentRuleId = e.ObjectID;
            GroupsListContainingInCurrentInRuleControl.OnRuleChanging(this, e);
            GroupsListNotContainingInCurrentInRuleControl.OnRuleChanging(this, e);
        }

        public IList<UserGroup> GetPersonsContainingInGroup()
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(CurrentRuleId);
            return allGroupsByRule;
        }

        public IList<UserGroup> GetPersonsotNotContainingInGroup()
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(CurrentRuleId);
            var allGroups = new GroupRepository().GetAllGroups();
            var userGroupsNotContainingInCurrentRule = allGroups
                .Where(userGroupFromAllGroups => !allGroupsByRule.Any(userGroupByRule => userGroupByRule.ID.Value == userGroupFromAllGroups.ID.Value)).ToList();
            return userGroupsNotContainingInCurrentRule;
        }
    }
}
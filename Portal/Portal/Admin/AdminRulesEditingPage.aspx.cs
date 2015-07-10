using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Portal.Controls.RulesControls;

namespace Portal.Admin
{
    public partial class AdminRulesEditingPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isShowRuleCreatorControl = string.IsNullOrEmpty(Request.QueryString["RuleID"]);
            if (isShowRuleCreatorControl)
            {
                var ruleCreatorControl = (RuleCreatorControl)
                             LoadControl("~/Controls/RulesControls/RuleCreatorControl.ascx");
                RuleConfigurationPlaceHolder.Controls.Add(ruleCreatorControl);
                GroupsManipulationControlID.Visible = false;
            }
            else
            {
                var ruleId = int.Parse(Request.QueryString["RuleID"]);
                var ruleConfigurationControl = (RuleConfigurationControl)
                             LoadControl("~/Controls/RulesControls/RuleConfigurationControl.ascx");
                ruleConfigurationControl.RuleId = ruleId;

                var groupRepository = new GroupRepository();
                var ruleRepository = new RuleRepository(groupRepository);

                var parsedRuleKind = ruleRepository.GetRuleById(ruleId).RuleType;

                Rule editingRule;
                switch (parsedRuleKind)
                {
                    case RuleKind.AddWorkTime:
                        editingRule = ruleRepository.GetRuleById<InsertTimeOffRule>(ruleId);
                        break;
                    case RuleKind.NotReportToMoscow:
                        editingRule = ruleRepository.GetRuleById<NotReportToMoscowRule>(ruleId);
                        break;
                    case RuleKind.NotifyByTime:
                        editingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                        break;
                    case RuleKind.NotifyLastUser:
                        editingRule = ruleRepository.GetRuleById<NotifyLastUserRule>(ruleId);
                        break;
                    default:
                        throw new ArgumentException();
                }

                ruleConfigurationControl.SetRuleProperty(editingRule, parsedRuleKind);
                RuleConfigurationPlaceHolder.Controls.Add(ruleConfigurationControl);

                if (!Page.IsPostBack)
                {
                    GroupsManipulationControlID.AddCommonColumnsToEntitiesGridView("IDColumn", "ID");
                    GroupsManipulationControlID.AddCommonColumnsToEntitiesGridView("NameColumn", "Name");
                    GroupsManipulationControlID.AddCommonColumnsToEntitiesGridView("DescriptionColumn", "Description");
                }
                
                GroupsManipulationControlID.Visible = true;
                GroupsManipulationControlID.CurrentWrapperEntityId = ruleId;
                GroupsManipulationControlID.AddEntitiesToWrapperEntityAction += AddEntitiesToWrapperEntity;
                GroupsManipulationControlID.RemoveEntitiesToWrapperEntityAction += RemoveEntitiesToWrapperEntity;
                GroupsManipulationControlID.GetIncludedEntities = GetIncludedEntitiesForBinding;
                GroupsManipulationControlID.GetNotIncludedEntities = GetNotIncludedEntitiesForBinding;
            }
        }

        public IList<Object> GetIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(wrapperEntityId);

            return allGroupsByRule.Select(group => (object)group).ToArray();
        }

        public IList<Object> GetNotIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(wrapperEntityId);
            var allGroups = new GroupRepository().GetAllGroups();
            var userGroupsNotContainingInCurrentRule = allGroups
                .Where(userGroupFromAllGroups => !allGroupsByRule.Any(userGroupByRule => userGroupByRule.ID.Value == userGroupFromAllGroups.ID.Value)).ToList();

            return userGroupsNotContainingInCurrentRule.Select(
                    group => (object)group).ToArray();
        }

        public void AddEntitiesToWrapperEntity(object sender, EntitiesManipulationEventArgs entitiesManipulationEventArgs)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.AddGroupIdsToRule(entitiesManipulationEventArgs.WrapperEntityId, entitiesManipulationEventArgs.IdsSelectedEntities.ToArray());
        }

        public void RemoveEntitiesToWrapperEntity(object sender, EntitiesManipulationEventArgs entitiesManipulationEventArgs)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.DeleteGroupIdsFromRule(entitiesManipulationEventArgs.WrapperEntityId, entitiesManipulationEventArgs.IdsSelectedEntities.ToArray());
        }
    }
}
using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    public class RuleProvider
    {
        public Rule GetRuleByIdAndRuleKind(int ruleId, RuleKind ruleKind)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            Rule rule;
            switch (ruleKind)
            {
                case RuleKind.AddWorkTime:
                    rule = ruleRepository.GetRuleById<InsertTimeOffRule>(ruleId);
                    break;
                case RuleKind.NotReportToMoscow:
                    rule = ruleRepository.GetRuleById<NotReportToMoscowRule>(ruleId);
                    break;
                case RuleKind.NotifyByTime:
                    rule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                    break;
                case RuleKind.NotifyLastUser:
                    rule = ruleRepository.GetRuleById<NotifyLastUserRule>(ruleId);
                    break;
                default:
                    throw new ArgumentException();
            }
            return rule;
        }
    }
}

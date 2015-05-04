using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public class NotReportingToMoscowProvider : RuleProvider
    {
        private List<NotReportingRuleToMoscow> _rules; 
        public override RuleKind TypeOfRule
        {
            get { return RuleKind.NotReportingToMoscow; }
        }

        public NotReportingToMoscowProvider()
        {
            _rules = new List<NotReportingRuleToMoscow>();
            FillRulesId();
        }

        public List<NotReportingRuleToMoscow> GetRules()
        {
            if (_rules.Count != 0) return _rules;

            foreach (var id in RulesId)
            {
                var rule = new NotReportingRuleToMoscow();
                if (rule.Load(id))
                {
                    _rules.Add(rule);
                }
            }
            return _rules;
        }
    }
}

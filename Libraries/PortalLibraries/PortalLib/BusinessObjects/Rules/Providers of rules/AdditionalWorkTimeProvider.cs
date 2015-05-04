using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public class AdditionalWorkTimeProvider : RuleProvider, IAdditionWorkTimeProvider
    {
        private List<IAdditionWork> _rules;
        public override RuleKind TypeOfRule
        {
            get { return RuleKind.AdditionalWorkTime; }
        }

        public AdditionalWorkTimeProvider()
        {
            _rules = new List<IAdditionWork>();
            FillRulesId();
        }

        public List<IAdditionWork> GetRules()
        {
            if (_rules.Count != 0) return _rules;

            foreach (var id in RulesId)
            {
                var rule = new AdditionRuleWorkTime();
                if (rule.Load(id))
                {
                    _rules.Add(rule);
                }
            }
            return _rules;
        }
    }
}

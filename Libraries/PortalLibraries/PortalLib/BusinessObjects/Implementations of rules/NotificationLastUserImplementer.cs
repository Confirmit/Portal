using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotificationLastUserImplementer
    {
        private List<INotificationLastUser> _rules;
        private IActivityRuleChecking _checkingRule;
        private StringBuilder _scriptText = new StringBuilder("<script type='text/javascript'> alert('Вы последний. Не забудьте : \n _Info_')</script>");

        public int UserId { get; private set; }

        public NotificationLastUserImplementer(INotificationLastUserProvider providerRules, IActivityRuleChecking ruleChecking, int userId)
        {
            _rules = providerRules.GetRules();
            _checkingRule = ruleChecking;
            UserId = userId;
        }

        private bool BuildScript()
        {
            var activeRules = new List<INotificationLastUser>();

            foreach (var rule in _rules)
            {
                if (_checkingRule.IsActive(rule) && rule.Contains(UserId) && IsLastUser(rule))
                    activeRules.Add(rule);
            }
            var builderScript = new StringBuilder();

            if (activeRules.Count == 0) return false;

            for (int i = 0; i < activeRules.Count; i++)
            {
                builderScript.AppendLine(string.Format("{0}) {1}", (i + 1), activeRules[i].Subject));
            }
            _scriptText = _scriptText.Replace("_Info_", builderScript.ToString());

            return true;
        }

        private bool IsLastUser(IRule rule)
        {
            var users = GetAllUsers(rule);
            var countUsers = GetCountActiveUsers(users);
            if (countUsers == 1) return true;

            return false;
        }

        private IList<int> GetAllUsers(IRule rule)
        {
            var groups = rule.GetUserGroups();
            var allUsers = new HashSet<int>();
            foreach (var group in groups)
            {
                allUsers.UnionWith(group.GetUsersId());
            }
            return allUsers.ToList();
        }

        private int GetCountActiveUsers(IList<int> users)
        {
            int countActiveUser = 0;
            foreach (var user in users)
            {
                var timeYesterday = DateTime.Now.AddDays(-1);
                var timeTommorow = DateTime.Now.AddDays(1);
                if (WorkEvent.GetEventsOfRange(user, timeYesterday, timeTommorow).Last().EventType == WorkEventType.TimeOff)
                    countActiveUser++;
            }
            return countActiveUser;
        }

    }
}

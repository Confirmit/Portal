using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;
using ConfirmIt.PortalLib.Notification;
using TestConsoleExecutorRules.Factory;
using TestConsoleExecutorRules.TestImplementation;
using UlterSystems.PortalLib.Notification;

namespace TestConsoleExecutorRules
{
    public class MainFactory
    {
        private string _mailAddress;
        public string MailAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_mailAddress))
                    return "TestAddress@confirmit.ru";
                return _mailAddress;
            }
            set { _mailAddress = value; }
        }
        public IMailStorage GetMailStorage()
        {
            return new TestMailStorage();
        }

        public MailProvider GetMailProvider()
        {
            return new MailProvider(MailAddress, MailTypes.NRNotification, new TestMailStorage());
        }

        public TimeExecutedRulesInspector<NotifyByTimeRule> GeTimeExecutedRulesInspector()
        {
            return new TimeExecutedRulesInspector<NotifyByTimeRule>(new ExecutedRuleRepository());
        }

        public ExecutedRuleRepository GetExecutedRuleRepository()
        {
            return new ExecutedRuleRepository();
        }

        public RuleFactory GetRuleFactory()
        {
            return new RuleFactory
              {
                  Time = new DateTime(2015, 6, 3),
                  Days = new HashSet<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday }
              };
        }

        public GroupFactory GetGroupFactory()
        {
            return new GroupFactory();
        }

        public UserFactory GetUserFactory()
        {
            return new UserFactory();
        }
    }
}

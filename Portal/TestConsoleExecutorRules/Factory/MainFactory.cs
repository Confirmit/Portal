using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
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
            return new DataBaseMailStorage();
        }

        public MailProvider GetMailProvider()
        {
            return new MailProvider(MailAddress, MailTypes.NRNotification, new DataBaseMailStorage());
        }
        

        public ExecutedRuleRepository GetExecutedRuleRepository()
        {
            return new ExecutedRuleRepository();
        }

        public RuleFactory GetRuleFactory()
        {
            return new RuleFactory
              {
                  Time = DateTime.Now,
                  Days = new HashSet<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday }
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

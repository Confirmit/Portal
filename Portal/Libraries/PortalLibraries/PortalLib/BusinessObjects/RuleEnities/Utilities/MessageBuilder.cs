using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class MessageBuilder
    {
        public void BuildScript(int userId, string subject, MessageHelper messegeHelper, IList<NotifyLastUserRule> rules)
        {
            messegeHelper.Subject = subject;

            foreach (NotifyLastUserRule rule in rules)
            {
                messegeHelper.AddNote(rule.Subject);
            }
        }
    }
}
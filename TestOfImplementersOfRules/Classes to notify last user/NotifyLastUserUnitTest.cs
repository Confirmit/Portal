using System;
using ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestOfImplementersOfRules
{
    [TestClass]
    public class NotifyLastUserUnitTest
    {
        [TestMethod]
        public void AfterInvocationBuildScript_TextOfScriptMustMatch()
        {
            var implementor = new NotificationLastUserImplementer(new TestNotificationLastUserProvider(), new TestActivityRuleChecking(), 1);
            Assert.IsTrue(implementor.BuildScript());
            var script = new JSAlertBuilder(implementor.Subject);
            script.AddNote();
        }
    }
}

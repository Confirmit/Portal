using System;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.Common_test_classes;

namespace TestOfImplementersOfRules
{
    [TestClass]
    public class NotifyLastUserUnitTest
    {
        [TestMethod]
        public void AfterInvocationBuildScript_TextOfScriptMustMatch()
        {
            var implementor = new NotificationLastUserImplementer(new TestNotificationLastUserProvider(), new TestActivityRuleChecking(), new TestWorkEventTypeRecognizer(WorkEventType.TimeOff), 1);

            Assert.IsTrue(implementor.BuildScript());
            var script = new JSAlertBuilder(implementor.Subject);
            //script.AddNote();
        }
    }
}

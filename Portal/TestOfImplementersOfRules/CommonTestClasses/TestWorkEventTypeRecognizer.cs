using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;

namespace TestOfImplementersOfRules.CommonTestClasses
{
    public class TestWorkEventTypeRecognizer :IWorkEventTypeRecognizer
    {
        private readonly WorkEventType _eventType;
        public TestWorkEventTypeRecognizer(WorkEventType expectedEventType)
        {
            _eventType = expectedEventType;
        }
        public WorkEventType GetType(int userId)
        {
            return _eventType;
        }
    }
}
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace TestOfImplementersOfRules.Common_test_classes
{
    public class TestWorkEventTypeRecognizer :IWorkEventTypeRecognizer
    {
        private WorkEventType _eventType;
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
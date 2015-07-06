using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using Core.Dictionaries;

namespace TestOfImplementersOfRules.CommonTestClasses
{
    public class TestWorkEventTypeRecognizer :IWorkEventTypeRecognizer
    {
        public readonly WorkEventType _eventType;

        public IDictionary<int, WorkEventType> DictionaryUserIdWorkEvent { get; set; }

        public TestWorkEventTypeRecognizer(WorkEventType expectedEventType)
        {
            _eventType = expectedEventType;
            DictionaryUserIdWorkEvent = new Dictionary<int, WorkEventType>();
        }

        public TestWorkEventTypeRecognizer(IDictionary<int, WorkEventType> dictionaryUserIdWorkEvent)
        {
            DictionaryUserIdWorkEvent = dictionaryUserIdWorkEvent;
        }

        public WorkEventType GetType(int userId)
        {
            if (DictionaryUserIdWorkEvent.ContainsKey(userId))
                return DictionaryUserIdWorkEvent[userId];

            return _eventType;
        }
    }
}
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using Core.Dictionaries;

namespace TestOfImplementersOfRules.CommonTestClasses
{
    public class TestActiveStateUserRecognizer :IActiveStateUserRecognizer
    {
        public readonly bool _state;

        public IDictionary<int, bool> DictionaryUserIdActiveState { get; set; }

        public TestActiveStateUserRecognizer(bool state)
        {
            _state = state;
            DictionaryUserIdActiveState = new Dictionary<int, bool>();
        }

        public TestActiveStateUserRecognizer(IDictionary<int, bool> dictionaryUserIdActiveState)
        {
            DictionaryUserIdActiveState = dictionaryUserIdActiveState;
        }

        public bool IsActive(int userId)
        {
            if (DictionaryUserIdActiveState.ContainsKey(userId))
                return DictionaryUserIdActiveState[userId];

            return _state;
        }
    }
}
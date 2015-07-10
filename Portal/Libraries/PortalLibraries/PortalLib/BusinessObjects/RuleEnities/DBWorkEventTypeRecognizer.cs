using System;
using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public class DbActiveStateUserRecognizer : IActiveStateUserRecognizer
    {
        public bool IsActive(int userId)
        {
            var mainWorkEvent = WorkEvent.GetMainWorkEvent(userId, DateTime.Now);
            if (mainWorkEvent != null && mainWorkEvent.IsOpen)
                return true;

            return false;
        }
    }
}
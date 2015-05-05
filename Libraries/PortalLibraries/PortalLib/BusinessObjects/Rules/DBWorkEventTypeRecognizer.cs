using System;
using System.Linq;
using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public class DBWorkEventTypeRecognizer : IWorkEventTypeRecognizer
    {
        public WorkEventType GetType(int userId)
        {
            var timeYesterday = DateTime.Now.AddDays(-1);
            var timeTommorow = DateTime.Now.AddDays(1);
            return WorkEvent.GetEventsOfRange(userId, timeYesterday, timeTommorow).Last().EventType;
        }
    }
}
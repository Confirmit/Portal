using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    [Serializable]
    public enum RuleKind
    {
        NotifyByTime = 1,
        NotifyLastUser = 2,
        AddWorkTime = 3,
        NotReportToMoscow = 4
    }
}

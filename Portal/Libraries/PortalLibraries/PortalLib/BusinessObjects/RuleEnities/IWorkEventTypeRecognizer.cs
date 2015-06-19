using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public interface IWorkEventTypeRecognizer
    {
        WorkEventType GetType(int userId);
    }
}
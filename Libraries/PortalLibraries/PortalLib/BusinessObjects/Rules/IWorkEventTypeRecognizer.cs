using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public interface IWorkEventTypeRecognizer
    {
        WorkEventType GetType(int userId);
    }
}
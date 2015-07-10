namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public interface IActiveStateUserRecognizer
    {
        bool IsActive(int userId);
    }
}
namespace ConfirmIt.PortalLib.FiltersSupport
{
    public interface IFilter
    {
        bool IsChanged(object filter);
    }
}

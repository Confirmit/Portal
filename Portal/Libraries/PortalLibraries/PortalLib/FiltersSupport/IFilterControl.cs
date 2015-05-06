namespace ConfirmIt.PortalLib.FiltersSupport
{
    public interface IFilterControl
    {
        IFilter Filter { get; }
        bool FilterChanged { get; set; }
    }
}

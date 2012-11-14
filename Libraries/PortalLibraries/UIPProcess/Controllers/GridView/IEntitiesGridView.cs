using UIPProcess.UIP.Views;

namespace UIPProcess.Controllers.GridView
{
    public interface IEntitiesGridView : IWebControl
    {
        IObjectGridView ObjectGridView { get;}
    }
}
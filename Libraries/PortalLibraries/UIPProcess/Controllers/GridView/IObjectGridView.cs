namespace UIPProcess.Controllers.GridView
{
    public interface IObjectGridView
    {
        string ClientID { get; }
        string ClientJSObjectName { get; }

        string NavigateOnAdd { get; }
        int SelectedEntityId { get;}
        string SelectedEntityType { get;}
        string NavigateOnSelect {get;}

        int[] SelectedRows { get; }
        int[] SelectedRowsIds { get; }        
        int PageIndex { get; }
        int PageSize { get; }
    }
}
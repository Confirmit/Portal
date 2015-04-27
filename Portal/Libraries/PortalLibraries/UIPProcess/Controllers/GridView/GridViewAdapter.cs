namespace UIPProcess.Controllers.GridView
{
    /// <summary>
    /// Adapter class to access to GridView controls on the server.
    /// </summary>
    public class GridViewAdapter : IObjectGridView
    {
        public GridViewAdapter(Controls.HotGridView.GridView gridView)
        {
            _gridView = gridView;
        }
        private Controls.HotGridView.GridView _gridView = null;

        public string ClientID
        {
            get { return _gridView.ClientID; }
        }

        public string ClientJSObjectName
        {
            get { return _gridView.ClientJSObjectName; }
        }

        public string NavigateOnAdd
        {
            get { return _gridView.NavigateOnAdd; }
        }
        
        public int SelectedEntityId 
        {
            get { return _gridView.SelectedEntityId; }
        }

        public string SelectedEntityType
        {
            get { return _gridView.SelectedEntityType; }
        }
        
        public string NavigateOnSelect 
        {
            get { return _gridView.NavigateOnSelect; }
        }

        public int[] SelectedRows
        {
            get { return _gridView.GetCheckBoxesSelectedIndices(); }
        }

        public int[] SelectedRowsIds
        {
            get { return _gridView.SelectedRowsIds; }
        }

        public int PageIndex
        {
            get { return _gridView.PageIndex; }
        }

        public int PageSize
        {
            get { return _gridView.PageSize; }
        }
    }
}
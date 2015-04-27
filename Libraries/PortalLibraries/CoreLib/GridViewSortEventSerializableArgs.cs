using System;
using System.Web.UI.WebControls;

namespace Core
{
    [Serializable]
    public class GridViewSortEventSerializableArgs : EventArgs
    {
        public string SortExpression { get; set; }
        public SortDirection CurrentDirection { get; set; }

        public GridViewSortEventSerializableArgs(string sortExpression, SortDirection currentDirection)
        {
            SortExpression = sortExpression;
            CurrentDirection = currentDirection;
        }
    }
}
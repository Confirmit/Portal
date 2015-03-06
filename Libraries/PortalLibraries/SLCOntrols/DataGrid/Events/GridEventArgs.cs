using System.Windows;
using System.Windows.Controls;

namespace SLControls.DataGrid.Events
{
    public class GridEventArgs : RoutedEventArgs
    {
        public GridEventArgs(DataGridRow row, DataGridColumn column) : base()
        {
            Row = row;
            Column = column;
        }

        #region Properties

        public DataGridRow Row { get; set; }
        public DataGridColumn Column { get; set; }

        #endregion
    }
}

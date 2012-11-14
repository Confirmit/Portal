using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using SilverlightMenu;
using SLControls.DataGrid.Events;

namespace SLControls.DataGrid
{
    public class MenuButton : MenuItem
    {
        #region Constructor

        public MenuButton()
            : base()
        {
            ParentMenuItem = this;
            DefaultStyleKey = typeof(Button);
        }

        #endregion

        #region Events

        public new event GridButtonEventHandler Click;
        public new event GridButtonEventHandler BeforeButtonClick;
        public delegate void GridButtonEventHandler(object sender, GridEventArgs e);

        #endregion

        #region Properties

        public bool OnClickSubscribe
        {
            get { return Click != null; }
        }

        #region Auto Properties

        public DataGridRow Row { get; set; }
        public DataGridColumn Column { get; set; }

        #endregion

        #endregion

        public new Popup Popup
        {
            set { base.Popup = value; }
        }

        #region Methods

        protected override void OnClick()
        {
            if (BeforeButtonClick != null)
                BeforeButtonClick(this, new GridEventArgs(Row, Column));

            base.OnClick();

            if (OnClickSubscribe)
                Click(this, new GridEventArgs(Row, Column));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Controls.HotGridView
{
    /// <summary>
    /// This class represents the field with the ability to 'select' the
    /// entity from it.
    /// </summary>
    public class BoundSelectionField : BoundField
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public BoundSelectionField()
        { }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public String ActionName
        {
            set { ViewState[keyActionNameProperty] = value; }
            get
            {
                return ViewState[keyActionNameProperty] == null ? String.Empty
                                                                : (String)ViewState[keyBusinessKeyProperty];
            }
        }
        private readonly String keyActionNameProperty = "ActionNameProperty";

        /// <summary>
        /// 
        /// </summary>
        public String BusinessKeyProperty
        {
            set { ViewState[keyBusinessKeyProperty] = value; }
            get
            {
                return ViewState[keyBusinessKeyProperty] == null ? String.Empty 
                                                                : (String)ViewState[keyBusinessKeyProperty];
            }
        }
        private readonly String keyBusinessKeyProperty = "BusinessKeyProperty";

        /// <summary>
        /// The type of the object which is related to this particular field.
        /// </summary>
        public String ColumnType
        {
            set { ViewState[keyColumnType] = value; }
            get 
            {
                return ViewState[keyColumnType] == null ? String.Empty : (String) ViewState[keyColumnType]; 
            }
        }        
        private readonly String keyColumnType = "ColumnType";

        /// <summary>
        /// The name of the view (from the Application configuration) where the 
        /// System should navigate when User click to this field in a grid.
        /// </summary>
        public String NavigateOnSelect
        {
            set { ViewState[keyNavigateOnSelect] = value; }
            get
            {
                return ViewState[keyNavigateOnSelect] == null ? String.Empty : (String)ViewState[keyNavigateOnSelect];
            }
        }
        private readonly String keyNavigateOnSelect = "NavigateOnSelect";

        /// <summary>
        /// The name of the property of data object, bound to row, from which to load the identifier
        /// of the object, bound to this field.
        /// </summary>
        public String IdPropertyName
        {
            set { ViewState[keyIdPropertyName] = value; }
            get { return ViewState[keyIdPropertyName] == null ? String.Empty : (String)ViewState[keyIdPropertyName]; }
        }
        private readonly String keyIdPropertyName = "IdPropertyName";

        /// <summary>
        /// The URL of the image to show in this field.
        /// </summary>
        public String ImageUrl
        {
            set { ViewState[keyImageUrl] = value; }
            get { return ViewState[keyImageUrl] == null ? String.Empty : (String)ViewState[keyImageUrl]; }
        }
        private readonly String keyImageUrl = "ImageUrl";

        /// <summary>
        /// The Alt Text of the image to show in this field.
        /// </summary>
        public String AltText
        {
            set { ViewState[keyAltText] = value; }
            get { return ViewState[keyAltText] == null ? String.Empty : (String)ViewState[keyAltText]; }
        }
        private readonly String keyAltText = "AltText";

        public Int32 ColumnIndex
        {
            set { ViewState[keyColumnIndex] = value; }
            get { return ViewState[keyColumnIndex] == null ? 0 : (Int32)ViewState[keyColumnIndex]; }
        }
        private readonly String keyColumnIndex = "ColumnIndex";

        /// <summary>
        /// If ControllerJSObjectName is specified for the grid this should be the call to a JS function, provided
        /// by the Controller class. Otherwise this could be any JS code which should be executed
        /// when User click to the field.
        /// </summary>
        public String OnClickClientEvent
        {
            set { ViewState[keyClickClientEvent] = value; }
            get { return ViewState[keyClickClientEvent] == null ? String.Empty : (String) ViewState[keyClickClientEvent]; }
        }
        private readonly String keyClickClientEvent = "OnClickClientEvent";

        public String ControllerObjectName
        {
            set { ViewState[keyControllerObjectName] = value; }
            get 
            { 
                return ViewState[keyControllerObjectName] == null 
                    ? String.Empty : (String)ViewState[keyControllerObjectName];
            }
        }
        private readonly String keyControllerObjectName = "ControllerObjectNameBSF";

        #endregion

        protected override void OnDataBindField(object sender, EventArgs e)
        {
            base.OnDataBindField(sender, e);
            
            DataControlFieldCell cell = (DataControlFieldCell) sender;

            BoundSelectionFieldData dataCell = new BoundSelectionFieldData();
            HyperLink link = dataCell.CreateControlLink(cell
                , IdPropertyName, ControllerObjectName, OnClickClientEvent
                , ImageUrl, AltText, ColumnIndex, BusinessKeyProperty
                );

            if (!_clearData)
            {
                _data.Clear();
                _clearData = true;
            }

            _data.Add(dataCell);

            if (link != null)
            {
                cell.Text = String.Empty;
                cell.Controls.Add(link);
            }
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            base.InitializeDataCell(cell, rowState);
            
            if (_data != null && _countRows < _data.Count) {
                BoundSelectionFieldData dataCell = _data[_countRows];

                if (!dataCell.IsEmpty())
                    cell.Controls.Add(dataCell.CreateControlLinkFromCachedData());
            }

            _countRows++;
        }

        #region View State support

        protected override void LoadViewState(object savedState)
        {
            _data = savedState as IList<BoundSelectionFieldData>;
        }

        protected override object SaveViewState()
        {
            return _data;
        }

        #endregion

        #region Fields

        private IList<BoundSelectionFieldData> _data = new List<BoundSelectionFieldData>();
        private Int32 _countRows = 0;
        private bool _clearData = false;

        #endregion
    }
}
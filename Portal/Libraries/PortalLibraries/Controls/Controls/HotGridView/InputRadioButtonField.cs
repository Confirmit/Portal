using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Controls.HotGridView
{
    internal class InputRadioButtonField : TemplateField
    {
        public const String RadioButtonID = "SpecialRadioButton";

        public InputRadioButtonField(
              String rowIdPropertyName
            , String controllerName
            , String businessKeyPropertyName
            , Int32 columnIndex
        )
        {
            ItemTemplate = new InputRadioButton(rowIdPropertyName
                , controllerName, businessKeyPropertyName, columnIndex);     
        }

        public Boolean AutoSubmitOnSelectRow
        {
            set { ((InputRadioButton)ItemTemplate).AutoSubmitOnSelectRow = value; }
        }

        public String SelectedRadioButtonValue
        {
            set { ((InputRadioButton)ItemTemplate).SelectedRadioButtonValue = value; }
        }
    }

    internal sealed class InputRadioButton : ITemplate
    {
        public InputRadioButton(
              String rowIdPropertyName
            , String controllerName
            , String businessKeyPropertyName
            , Int32 columnIndex
        )
        {
            _rowIdPropertyName = rowIdPropertyName;
            _controllerName = controllerName;
            _businessKeyPropertyName = businessKeyPropertyName;
            _columnIndex = columnIndex;
        }

        public Boolean AutoSubmitOnSelectRow
        {
            get { return _autoSubmitOnSelectRow; }
            set { _autoSubmitOnSelectRow = value; }
        }
        private Boolean _autoSubmitOnSelectRow = false;

        public String SelectedRadioButtonValue
        {
            get { return _selectedRadioButtonValue; }
            set { _selectedRadioButtonValue = value; }
        }
        private String _selectedRadioButtonValue = null;

        private String _rowIdPropertyName = String.Empty;
        private String _controllerName = String.Empty;
        private String _businessKeyPropertyName = String.Empty;
        private Int32 _columnIndex = 0;

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            Literal l = new Literal();
            l.DataBinding += new EventHandler(BindData);
            container.Controls.Add(l);
        }

        #endregion

        private Boolean _fFirstRow = true;

        public void BindData(object sender, EventArgs e)
        {
            Literal l = (Literal)sender;
            GridViewRow rowViewRow = (GridViewRow)l.NamingContainer;
            GridView grid = (GridView)rowViewRow.NamingContainer;

            String databaseId = BoundSelectionFieldData.GetObjectValue(rowViewRow.DataItem, _rowIdPropertyName);
            String businessId = String.IsNullOrEmpty(_businessKeyPropertyName)
                ? String.Empty
                : BoundSelectionFieldData.GetObjectValue(rowViewRow.DataItem, _businessKeyPropertyName);

            Boolean _isChecked = databaseId.Equals(SelectedRadioButtonValue) ? true
                    : businessId.Equals(SelectedRadioButtonValue);

            if (_fFirstRow)
                _isChecked = true;

            l.Text = String.Format(
                "<input name=\"{0}\" id=\"{0}\" type=\"radio\" {1} onclick=\"{2}\" value=\"{3}\"/>"
                , grid.UniqueID + InputRadioButtonField.RadioButtonID
                , (_isChecked) ? "checked" : ""
                , getClickHandlerString(databaseId, businessId)
                , databaseId
            );

            if (_isChecked)
            {
                grid.RegisterInitScript(String.Format(@"
                    {0}._gridViewClient.setSelectedDatabaseId('{1}');
                    {0}._gridViewClient.setSelectedBusinessKey('{2}');
                    ", _controllerName, databaseId, businessId));
            }
            
            _fFirstRow = false;
        }

        private String getClickHandlerString(String databaseId, String businessId)
        {
            if (!AutoSubmitOnSelectRow)
                return String.Format("javascript:{0}.OnClick('{1}','{2}','{3}', false);"
                        , _controllerName
                        , _columnIndex, databaseId, businessId);
            else
                return String.Format("javascript:{0}.OnSelectRowSubmit('{1}','{2}','{3}');"
                        , _controllerName
                        , _columnIndex, databaseId, businessId);
        }
    }
}
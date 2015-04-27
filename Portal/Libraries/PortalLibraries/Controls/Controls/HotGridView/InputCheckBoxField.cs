using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls.HotGridView
{
    internal class InputCheckBoxField : CheckBoxField
    {
        public const String CheckBoxID = "SpecialCheckBoxButton";
        public const String RowIdFieldID = "RowIdHiddenField";

        public InputCheckBoxField(String rowIdPropertyName)
        {
            DataField = rowIdPropertyName;
        }

        protected override void InitializeDataCell(DataControlFieldCell cell
                                                 , DataControlRowState rowState)
        {
            CheckBox chk = new CheckBox();
            chk.Checked = ((GridView)Control).IsTitleCheckBoxChecked;
            chk.ID = InputCheckBoxField.CheckBoxID;
            cell.Controls.Add(chk);

            if (DataField.Equals(""))
                return;

            HiddenField fieldRowID = new HiddenField();
            fieldRowID.ID = RowIdFieldID;

            fieldRowID.DataBinding += new EventHandler(OnIdFieldDataBind);
            cell.Controls.Add(fieldRowID);
        }

        protected void OnIdFieldDataBind(object sender, EventArgs e)
        {
            HiddenField fieldRowID = (HiddenField)sender;
            Object objData = this.GetValue(fieldRowID.NamingContainer);            
            fieldRowID.Value = objData.ToString();
        }
    }
}

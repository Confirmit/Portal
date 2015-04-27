using System;
using System.Collections;
using System.Web.UI.WebControls;


namespace Controls.HotGridView
{
    public partial class GridView : System.Web.UI.WebControls.GridView
    {
        #region Constants
        private const string CheckBoxColumHeaderTemplate = "<input type='checkbox' hidefocus='hidefocus' id='{0}' name='{1}' {2} onclick='{3}.CheckAll(this)'/>";
        private const string CheckBoxColumHeaderID = "{0}_HeaderButton";
        // HeaderButton должен быть такой же как в js коде!!!
        private const string CheckBoxColumHeaderName = "{0}$HeaderButton"; 
        #endregion

        // METHOD:: add a brand new radio button column
        protected virtual ArrayList AddRadioButtonsColumn(ICollection columnList)
        {
            ArrayList list = new ArrayList(columnList);

            InputRadioButtonField field = new InputRadioButtonField(RowIdPropertyName
                , ControllerJSObjectName, RowBusinessKeyPropertyName
                , RadioButtonColumnIndex);
            field.ItemStyle.Width = new Unit("16px");
            field.HeaderText = "";

            field.AutoSubmitOnSelectRow = AutoSubmitOnSelectRow;
            field.SelectedRadioButtonValue = RadioButtonSelectedIndexValue;

            if (RadioButtonColumnIndex > list.Count)
            {
                list.Add(field);
                RadioButtonColumnIndex = list.Count - 1;
            }
            else
                list.Insert(RadioButtonColumnIndex, field);

            return list;
        }

        protected virtual ArrayList AddCheckBoxColumn(ICollection columnList)
        {
            ArrayList list = new ArrayList(columnList);

            // Determine the check state for the header checkbox
            string shouldCheck = "";
            string checkBoxID = String.Format(CheckBoxColumHeaderID, ClientID);
            string checkBoxName = String.Format(CheckBoxColumHeaderName, UniqueID);

            if (IsTitleCheckBoxChecked)
                shouldCheck = "checked=\"checked\"";
            
            // Create a new custom CheckBoxField object 
            InputCheckBoxField field = new InputCheckBoxField(RowIdPropertyName);
            field.ItemStyle.Width = new Unit("16px");
            field.HeaderText = String.Format(CheckBoxColumHeaderTemplate, checkBoxID, checkBoxName, shouldCheck, ClientJSObjectName);
            field.ReadOnly = true;

            // Insert the checkbox field into the list at the specified position
            if (CheckBoxColumnIndex > list.Count)
            {
                // If the desired position exceeds the number of columns 
                // add the checkbox field to the right. Note that this check
                // can only be made here because only now we know exactly HOW 
                // MANY columns we're going to have. Checking Columns.Count in the 
                // property setter doesn't work if columns are auto-generated
                list.Add(field);
                CheckBoxColumnIndex = list.Count - 1;
            }
            else
                list.Insert(CheckBoxColumnIndex, field);

            // Return the new list
            return list;
        }


    
        // METHOD:: retrieve the style object based on the row state
        protected virtual TableItemStyle GetRowStyleFromState(DataControlRowState state)
        {
            switch (state)
            {
                case DataControlRowState.Alternate:
                    return AlternatingRowStyle;
                case DataControlRowState.Edit:
                    return EditRowStyle;
                case DataControlRowState.Selected:
                    return SelectedRowStyle;
                default:
                    return RowStyle;

                // DataControlRowState.Insert is not relevant here
            }
        }
    }
}
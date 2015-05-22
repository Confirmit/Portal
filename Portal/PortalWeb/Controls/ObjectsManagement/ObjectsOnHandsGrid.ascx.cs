using System.Data;
using System.Web.UI.WebControls;

using Core;

public partial class ObjectsOnHandsGrid : BaseUserControl
{
    public string ControlID { get; set; }

    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);

        gridViewOnHands.RowDataBound += OnRowDataBound;

        foreach (Parameter parameter in dsObjectsOnHand.SelectParameters)
        {
            if (parameter is ControlParameter)
                ((ControlParameter)parameter).ControlID = ControlID;
        }
    }

    private void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DataRowView rowView = (DataRowView)e.Row.DataItem;
        MLText firstname = new MLText();
        MLText lastname = new MLText();

        firstname.LoadFromXML(rowView["OwnerFirstName"].ToString());
        lastname.LoadFromXML(rowView["OwnerLastName"].ToString());
        e.Row.Cells[2].Text = string.Format("{0} {1}", firstname, lastname);

        Image image = e.Row.Cells[0].FindControl("imgType") as Image;
        if (image == null)
            return;

        image.ImageUrl = string.Format("~/Images/RequestObject/reqobj_{0}.png", rowView["ObjType"].ToString());
    }
}
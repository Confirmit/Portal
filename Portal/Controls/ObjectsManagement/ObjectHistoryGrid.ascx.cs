using System;
using System.Data;
using System.Web.UI.WebControls;

using Core;
using ConfirmIt.PortalLib.BusinessObjects.RequestObjects;

public partial class ObjectHistoryGrid : BaseUserControl
{
    private int? ObjectID 
    {
        get 
        {
            if (ViewState["ObjectID"] == null)
                return -1;

            return (int)ViewState["ObjectID"];
        } 
        set { ViewState["ObjectID"] = value;}
    }

    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);

        if (!IsPostBack)
            ObjectID = null;

        dsObjectHistory.ObjectCreated += new ObjectDataSourceObjectEventHandler(OnDataSourceObjectCreated);
        gridViewObjectHistory.RowDataBound += new GridViewRowEventHandler(OnRowDataBound);
    }

    public void DataBind(int reqObjectID)
    {
        ObjectID = reqObjectID;

        gridViewObjectHistory.PageIndex = 0;
        gridViewObjectHistory.DataBind();
    }

    private void OnDataSourceObjectCreated(object sender, ObjectDataSourceEventArgs e)
    {
        RequestObjectHistoryDataSource dataSource = e.ObjectInstance as RequestObjectHistoryDataSource;
        if (dataSource == null)
            return;

        dataSource.ObjectID = ObjectID.Value;
    }

    private void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DataRowView rowView = (DataRowView)e.Row.DataItem;
        MLText firstname = new MLText();
        MLText lastname = new MLText();

        firstname.LoadFromXML(rowView["FirstName"].ToString());
        lastname.LoadFromXML(rowView["LastName"].ToString());
        e.Row.Cells[0].Text = string.Format("{0} {1}", firstname, lastname);

        e.Row.Cells[2].Text = (bool)rowView["IsTaken"]
                            ? (string)GetLocalResourceObject("Taken")
                            : (string)GetLocalResourceObject("Granted");
    }
}
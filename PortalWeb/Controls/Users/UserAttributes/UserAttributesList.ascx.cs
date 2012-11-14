using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_AdminPersonAttributesView : BaseUserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        gridViewAttributes.PageIndexChanging += gvAttributes_PageIndexChanging;
        gridViewAttributes.DataBound += gvAttributes_DataBound;
        gridViewAttributes.RowDeleting += gvAttributes_RowDeleting;
        gridViewAttributes.RowEditing += gvAttributes_RowEditing;
        gridViewAttributes.RowDataBound += gridViewAttributes_RowDataBound;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        bindAttributesGridView();
    }

    private void bindAttributesGridView()
    {
        try
        {
            PersonAttributeType[] attrtype =
                PersonAttributeType.GetAllTypesAttributes();

            gridViewAttributes.DataSource = attrtype;
            gridViewAttributes.DataBind();
        }
        catch (Exception ex)
        {
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
        }
    }

    private void gvAttributes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridViewAttributes.PageIndex = e.NewPageIndex;
        //bindAttributesGridView();
    }

    private void gvAttributes_RowEditing(object sender, GridViewEditEventArgs e)
    {
        userAttributeInfo.UserAttributeId = (int)gridViewAttributes.DataKeys[e.NewEditIndex].Value;
    }

    private void gvAttributes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = (int)gridViewAttributes.DataKeys[e.RowIndex].Value;
        PersonAttributeType attr = new PersonAttributeType();
        if (!attr.Load(id))
            return;

        attr.Delete();
        //bindAttributesGridView();
    }

    private void gridViewAttributes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        ImageButton btn = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
        if (btn == null)
            return;

        btn.OnClientClick = string.Format("if (confirm('Are you sure you want to delete this person attribute?') == false) return false; ");
    }

    #region Pager Support

    protected virtual void OnPageIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        gridViewAttributes.PageIndex = Convert.ToInt32(ddl.SelectedValue) - 1;
        //bindAttributesGridView();
    }

    private void ShowPagerData(Control pagerRow)
    {
        if (pagerRow == null)
            return;

        DropDownList ddlPages = pagerRow.FindControl("ddlPage") as DropDownList;
        Literal lbl = pagerRow.FindControl("lblPageCount") as Literal;

        if ((ddlPages != null) && (lbl != null))
        {
            ddlPages.Items.Clear();
            for (int pageIndex = 1; pageIndex <= gridViewAttributes.PageCount; pageIndex++)
            {
                ListItem item = new ListItem(pageIndex.ToString());
                if (pageIndex == gridViewAttributes.PageIndex + 1)
                    item.Selected = true;
                ddlPages.Items.Add(item);
            }
            lbl.Text = gridViewAttributes.PageCount.ToString();
        }
    }

    private void gvAttributes_DataBound(object sender, EventArgs e)
    {
        GridViewRow topPagerRow = gridViewAttributes.TopPagerRow;
        GridViewRow bottomPagerRow = gridViewAttributes.BottomPagerRow;

        ShowPagerData(topPagerRow);
        ShowPagerData(bottomPagerRow);
    }

    #endregion
}
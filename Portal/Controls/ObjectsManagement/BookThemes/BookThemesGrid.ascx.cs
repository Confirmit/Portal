using System;
using System.Diagnostics;
using System.Web.UI.WebControls;

using UIPProcess.Controllers.GridView;

public partial class BookThemesGrid : BaseUserControl, IEntitiesGridView
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menuActions.ProcessedGridObject = gridViewBookThemes.ClientJSObjectName;
        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;

        ActionsMenuClientId = menuActions.ClientID;

        dsBookThemes.ObjectCreated += Controller.OnDataSourceCreated;
        gridViewBookThemes.Sorting += Controller.OnGridViewSorting;
    }

    public new IGridViewDataSourceController Controller
    {
        get { return (IGridViewDataSourceController)base.Controller; }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public Boolean DataChanged
    {
        set
        {
            if (value)
                bindData();
        }
    }

    private void bindData()
    {
        try
        {
            gridViewBookThemes.PageIndex = 0;
            gridViewBookThemes.DataBind();
        }
        catch (Exception exc)
        {
            Debug.Assert(false, exc.Message);
        }
    }

    #region IEntitiesGridView Members

    public IObjectGridView ObjectGridView
    {
        get { return new GridViewAdapter(gridViewBookThemes); }
    }

    #endregion
}
using System;
using System.Diagnostics;

using UIPProcess.Controllers.GridView;

public partial class UserEventsGrid : BaseUserControl, IEntitiesGridView
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menuActions.ProcessedGridObject = gridViewUserEvents.ClientJSObjectName;
        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;
        ActionsMenuClientId = menuActions.ClientID;

        dsEvents.ObjectCreated += Controller.OnDataSourceCreated;
        gridViewUserEvents.Sorting += Controller.OnGridViewSorting;
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
            gridViewUserEvents.PageIndex = 0;
            gridViewUserEvents.DataBind();
        }
        catch (Exception exc)
        {
            Debug.Assert(false, exc.Message);
        }
    }

    #region IEntitiesGridView Members

    public IObjectGridView ObjectGridView
    {
        get { return new GridViewAdapter(gridViewUserEvents); }
    }

    #endregion
}

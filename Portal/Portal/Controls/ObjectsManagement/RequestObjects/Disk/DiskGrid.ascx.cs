using System;
using System.Diagnostics;

using UIPProcess.Controllers.GridView;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RequestObjects;

public partial class DiskGrid : BaseUserControl, IEntitiesGridView
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menuActions.ProcessedGridObject = gridViewReqObjects.ClientJSObjectName;
        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;
        //gridViewReqObjects.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;

        ActionsMenuClientId = menuActions.ClientID;

        dsFilterReqObjects.ObjectCreated += Controller.OnDataSourceCreated;
        gridViewReqObjects.Sorting += Controller.OnGridViewSorting;
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
            gridViewReqObjects.PageIndex = 0;
            gridViewReqObjects.DataBind();
        }
        catch (Exception exc)
        {
            Debug.Assert(false, exc.Message);
        }
    }

    #region [ IEntitiesGridView Members ]

    public IObjectGridView ObjectGridView
    {
        get { return new GridViewAdapter(gridViewReqObjects); }
    }

    #endregion
}
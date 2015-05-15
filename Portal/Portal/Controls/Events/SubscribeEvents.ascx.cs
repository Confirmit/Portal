using System;
using System.Diagnostics;

using UIPProcess.Controllers.GridView;

public partial class SubscribeEvents : BaseUserControl, IEntitiesGridView
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        dsEventsForSubscribe.ObjectCreated += Controller.OnDataSourceCreated;
        gridViewSubscribeEvents.Sorting += Controller.OnGridViewSorting;
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
            gridViewSubscribeEvents.PageIndex = 0;
            gridViewSubscribeEvents.DataBind();
        }
        catch (Exception exc)
        {
            Debug.Assert(false, exc.Message);
        }
    }

    #region IEntitiesGridView Members

    public IObjectGridView ObjectGridView
    {
        get { return new GridViewAdapter(gridViewSubscribeEvents); }
    }

    #endregion
}

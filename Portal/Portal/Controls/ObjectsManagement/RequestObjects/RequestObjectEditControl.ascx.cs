using System;

using Controls;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers;

using UIPProcess.DataBinding;

public partial class RequestObjectEditControl : BaseUserControl
{
    #region  [ Page events ]

    protected override void OnInit(EventArgs e)
    {
        bindObjectTypes();
        simpleTabContainer.ActiveTabChanged += OnActiveTabChanged;

        base.OnInit(e);
        simpleTabContainer.ActiveTabChanged += ((ReqObjectEditCtlController)Controller).OnActiveTabChanged;
    }

    private void OnActiveTabChanged(object sender, int headerIndex)
    {
        if (ReqObjectTypeEnum == RequestObjectType.ObjectType.Book)
        {
            bookFilter.OnResetClicked(null, EventArgs.Empty);
            bookGrid.DataBind();
        }

        if (ReqObjectTypeEnum == RequestObjectType.ObjectType.Card)
        {
            cardFilter.OnResetClicked(null, EventArgs.Empty);
            cardGrid.DataBind();
        }

        if (ReqObjectTypeEnum == RequestObjectType.ObjectType.Disk)
        {
            diskFilter.OnResetClicked(null, EventArgs.Empty);
            diskGrid.DataBind();
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        bookFilter.Visible = bookInfo.Visible = bookGrid.Visible = ReqObjectTypeEnum == RequestObjectType.ObjectType.Book;
        cardFilter.Visible = cardInfo.Visible = cardGrid.Visible = ReqObjectTypeEnum == RequestObjectType.ObjectType.Card;
        diskFilter.Visible = diskInfo.Visible = diskGrid.Visible = ReqObjectTypeEnum == RequestObjectType.ObjectType.Disk;
    }

    #endregion

    public virtual RequestObjectType.ObjectType ReqObjectTypeEnum
    {
        get
        {
            return (RequestObjectType.ObjectType)
                        Enum.Parse(typeof(RequestObjectType.ObjectType), simpleTabContainer.ActiveHeaderIndex.ToString());
        }
    }

    #region [ Binding ]

    private void bindObjectTypes()
    {
        foreach (var value in Enum.GetValues(typeof(RequestObjectType.ObjectType)))
        {
            simpleTabContainer.Headers.Add(new SimpleTabHeader { HeaderText = Enum.GetName(typeof(RequestObjectType.ObjectType), value) });
        }
    }

    [DataBinding("ReqObjectType", null, false)]
    public Type ReqObjectType
    {
        get { return RequestObjectType.ObjectTypes[ReqObjectTypeEnum]; }
    }

    #endregion
}
using System;

using UlterSystems.PortalLib.BusinessObjects;

public partial class UserAttributeInfo : BaseUserControl
{
    #region Properties

    /// <summary>
    /// Person attribute key.
    /// </summary>
    private String UserAttributeKey
    {
        get { return string.Format("objectIdKey_{0}", ClientID); }
    }

    /// <summary>
    /// Id of editing attribute.
    /// </summary>
    public int UserAttributeId
    {
        set
        {
            ViewState[UserAttributeKey] = value;
            if (value > 0)
                attributeBinding();
        }
        get
        {
            if (ViewState[UserAttributeKey] == null)
                return -1;
            return (int)ViewState[UserAttributeKey];
        }
    }

    #endregion

    protected void btnAddAttribute_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbAttrName.Text))
        {
            lbErrorText.Text = (string)GetLocalResourceObject("noAttrNameData");
            lbErrorText.Visible = true;
            return;
        }

        PersonAttributeType attr = new PersonAttributeType();
        if (UserAttributeId > 0)
        {
            attr.Load(UserAttributeId);
            UserAttributeId = -1;
        }
        else
        {
            if (PersonAttributeType.GetAttributeType(tbAttrName.Text) != null)
            {
                lbErrorText.Text = (string) GetLocalResourceObject("isAttrError");
                lbErrorText.Visible = true;
                return;
            }
        }

        attr.AttributeName = tbAttrName.Text;
        attr.ShowToUsers = cbShowToUsers.Checked;
        attr.Save();

        clearData();
    }

    protected void btnCancelEditAttribute_Click(object sender, EventArgs e)
    {
        UserAttributeId = -1;
        clearData();
    }

    protected void attributeBinding()
    {
        PersonAttributeType attr = new PersonAttributeType();
        attr.Load(UserAttributeId);
        
        tbAttrName.Text = attr.AttributeName.Trim();
        cbShowToUsers.Checked = attr.ShowToUsers;

        tbAttrName.Enabled = Enum.IsDefined(typeof (PersonAttributeTypes), attr.AttributeName.Trim())
                                 ? false 
                                 : true;

        lbErrorText.Visible = false;
    }

    private void clearData()
    {
        tbAttrName.Text = string.Empty;
        cbShowToUsers.Checked = false;
        lbErrorText.Visible = false;
        tbAttrName.Enabled = true;
    }
}
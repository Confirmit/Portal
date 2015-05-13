using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using AjaxControlToolkit;

using Core;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.FilesManagers;
using UlterSystems.PortalLib.BusinessObjects;

public partial class UserInfoView : BaseUserControl
{
	#region Fields

	private readonly string m_uploadObjectName = "uploadFileObject";
	private readonly string m_attachID = "fileUploader";

	#endregion

	#region Properties

	/// <summary>
	/// ID пользователя.
	/// </summary>
	public int? UserID
	{
		get { return (int?)ViewState["UserID"]; }
		set { ViewState["UserID"] = value; }
	}

	/// <summary>
	/// Filter.
	/// </summary>
	private int FilterSelectedIndex
	{
		get
		{
			return ViewState["FilterSelectedIndex"] == null
					   ? 0
					   : (int)ViewState["FilterSelectedIndex"];
		}
		set { ViewState["FilterSelectedIndex"] = value; }
	}

	/// <summary>
	/// Is current user can edit person information data.
	/// </summary>
	public bool IsCurrentUserCanEditData
	{
		get
		{
			return Page.CurrentUser.ID == UserID
				   || Page.CurrentUser.IsInRole(RolesEnum.Administrator);
		}
	}

	#endregion    

	#region Page Events

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		gvContact.RowEditing += gvContact_RowEditing;
		gvContact.RowCancelingEdit += gvContact_RowCancelingEdit;
		gvContact.RowDeleting += gvContact_RowDeleting;
		gvContact.RowUpdating += gvContact_RowUpdating;
		gvContact.PageIndexChanging += gvContact_PageIndexChanging;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (IsPostBack)
			return;

		fillUserInfo();
		fillAttributeTypesList();
		enableControls();

		hlEdit.NavigateUrl = String.Format("{0}?UserID={1}",
										   hlEdit.NavigateUrl, UserID);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		RegisterJavaScript();

		//if (IsPostBack)
		//    return;

		bindAttributesGridView();
		ddlTypeSelect.SelectedIndex = FilterSelectedIndex;
	}

	#endregion

	#region GridView support

	public string UserKeyWordPrint(int AttributeID)
	{
		string typeAtr = ddlTypeSelect.SelectedItem.ToString();
		PersonAttributeType attr = new PersonAttributeType();

		if (!attr.Load(AttributeID))
			return string.Empty;

		return typeAtr.Equals("All")
				   ? attr.AttributeName
				   : string.Empty;
	}

	public string AllUserInfoPrint(string SrtringField, int AttributeID)
	{
		string typeAtr = ddlTypeSelect.SelectedItem.ToString();
		if (typeAtr.Equals("All"))
		{
			PersonAttributeType attr = new PersonAttributeType();
			attr.Load(AttributeID);
			return string.Format("{0} - {1}"
								 , attr.AttributeName
								 , SrtringField);
		}

		return SrtringField;
	}

	public bool printIsAllTypeAtr()
	{
		string typeAtr = ddlTypeSelect.SelectedItem.ToString();
		return typeAtr.Equals("All"); 
	}

	#region Edit, Cancel, Update and Delete row.

	// Редактируем строку
	protected void gvContact_RowEditing(object sender, GridViewEditEventArgs e)
	{
		gvContact.EditIndex = e.NewEditIndex;
	}

	// Отменяем то, что отредактировали в строке
	protected void gvContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		gvContact.EditIndex = -1;
	}

	// Удаляем строку
	protected void gvContact_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		int id = (int)gvContact.DataKeys[e.RowIndex].Value;
		PersonAttribute attr = new PersonAttribute();
		if (!attr.Load(id) || !attr.IsSaved)
			return;
		
		attr.Delete();
	}

	// Сохраняем строку, которую отредактировали
	protected void gvContact_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		int id = (int)gvContact.DataKeys[e.RowIndex].Value;
		PersonAttribute attr = new PersonAttribute();
		
		if(!attr.Load(id) || !attr.IsSaved)
			return;

		TextBox tbText = (TextBox)gvContact.Rows[e.RowIndex].FindControl("dataPanelEdit").FindControl("tbUserData");
		attr.StringField = tbText.Text;
		attr.Save();
		gvContact.EditIndex = -1;
	}

	#endregion

	protected void gvContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		gvContact.PageIndex = e.NewPageIndex;
	}

	#endregion

	#region Controls events

	protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
	{
		FilterSelectedIndex = ((DropDownList)sender).SelectedIndex;
		string typeAtr = ddlTypeSelect.SelectedItem.ToString();
		if (typeAtr.Equals("All"))
		{
			tbContInfo.Enabled = false;
			btnAddContact.Enabled = false;
		}
		else
		{
			tbContInfo.Enabled = true;
			btnAddContact.Enabled = true;
		}
		gvContact.EditIndex = -1;
		gvContact.PageIndex = 0;
	}

	/// <summary>
	/// Добавить новый контакт текущему пользователю
	/// </summary>
	protected void btnAddContact_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tbContInfo.Text)
			|| ddlTypeSelect.SelectedValue.Equals("All"))
			return;

		Person person = Person.GetPersonByID(UserID.Value);
		if (person == null)
			return;

		person.AddStandardStringAttribute(ddlTypeSelect.SelectedItem.ToString()
										  , tbContInfo.Text);
		tbContInfo.Text = string.Empty;
	}
    
	#endregion

	#region Methods

	protected void RegisterJavaScript()
	{
        //if (!IsCurrentUserCanEditData)
        //    return;

        //if (!(Page.ClientScript.IsClientScriptIncludeRegistered("FileUploader")))
        //    Page.ClientScript.RegisterClientScriptInclude(GetType(), "FileUploader"
        //                                                  , "../Scripts/FileUploader.js");

        //StringBuilder strScript = new StringBuilder();
        //strScript.Append("<script language='javascript' type='text/javascript'>");
        //strScript.AppendFormat("var {0} = new FileUploader('{1}', '{2}', '{0}');",
        //                       m_uploadObjectName,
        //                       div_photoAttachments.ClientID,
        //                       m_attachID);
        //strScript.Append("</script>");

        //if (!(Page.ClientScript.IsStartupScriptRegistered("InitializeUploadObject")))
        //    Page.ClientScript.RegisterStartupScript(GetType(), "InitializeUploadObject"
        //                                            , strScript.ToString());
	}

	/// <summary>
	/// Определям видимость контролов
	/// </summary>
	private void enableControls()
	{
		//ссылка на редактирование информации
		hlEdit.Visible = Page.CurrentUser.IsInRole(RolesEnum.Administrator);

		btnAddContact.Visible = tbContInfo.Visible = IsCurrentUserCanEditData;
	}

	/// <summary>
	/// Заполняет элементы управления информацией о пользователe.
	/// </summary>
	private void fillUserInfo()
	{
		if (UserID == null)
			return;

		Person user = new Person();
		if (!user.Load(UserID.Value))
		{
			lblFirstName.Text = lblMiddleName.Text
								= lblLastName.Text = "Error while loading person information.";
			return;
		}

		lblFirstName.Text = user.FirstName.ToString();
		lblMiddleName.Text = user.MiddleName.ToString();
		lblLastName.Text = user.LastName.ToString();
	}

	private void fillAttributeTypesList()
	{
		PersonAttributeType[] alltypes = PersonAttributeType.GetAllTypesAttributes();
		IEnumerable<string> attrTextList = from attr in alltypes
											where attr.ShowToUsers
											select attr.AttributeName;

		IList<string> resList = attrTextList.ToList();
		resList.Insert(0, "All");
		
		ddlTypeSelect.DataSource = resList;
		ddlTypeSelect.DataBind();
	}

	/// <summary>
	/// Заполняет информацию о контактах.
	/// </summary>
	private void bindAttributesGridView()
	{
		try
		{
			if (UserID == null)
				return;

			string typeAtr = ddlTypeSelect.SelectedItem.ToString();
			IList<PersonAttribute> dnAttribs = new List<PersonAttribute>();
			
			if(typeAtr.Equals("All")) 
				dnAttribs = getAllPublicPersonAttibutes();
			else 
			{
				PersonAttributeType attrtype = new PersonAttributeType();
				attrtype.LoadByReference("AttributeName", typeAtr);
				dnAttribs = PersonAttributes.GetPersonAttributesByKeyword(UserID.Value, attrtype);
			}
			
			gvContact.DataSource = dnAttribs;
			gvContact.DataBind();

			if (!IsCurrentUserCanEditData)
			{
				for (int i = 0; i < gvContact.Rows.Count; i++)
				{
					HoverMenuExtender hoverMenu = (HoverMenuExtender) gvContact.Rows[i].FindControl("hoverMenuView");
					hoverMenu.Enabled = false;
				}
				dvAddInfo.Visible = false;
			}
			if (typeAtr.Equals("All"))
				//dvAddInfo.Disabled = true;
			{
				tbContInfo.Enabled = false;
				btnAddContact.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			Logger.Log.Error(ex.Message, ex);
		}
	}

	private PersonAttribute[] getAllPublicPersonAttibutes()
	{
		IList<PersonAttribute> attributes = PersonAttributes.GetAllPersonAttributes(UserID.Value);
		PersonAttributeType[] alltypes = PersonAttributeType.GetAllTypesAttributes();
		PersonAttributeType typePhoto = PersonAttributeType.GetAttributeType(PersonAttributeTypes.Photo);

		IEnumerable<int> privateAttrList = from attr in alltypes
											where attr.ShowToUsers == false
											select int.Parse(attr.ID.ToString());

		if (IsCurrentUserCanEditData)
			return (from attr in attributes
					orderby attr.AttributeID, attr.StringField
					where attr.AttributeID != typePhoto.ID
					select attr).ToArray();

		return (from attr in attributes
				orderby attr.AttributeID, attr.StringField
				where !privateAttrList.Contains(attr.AttributeID)
				select attr).ToArray();
	}

	#endregion
}
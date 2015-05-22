using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BAL.Events;

using UIPProcess.DataBinding;
using UIPProcess.DataValidating;

using UlterSystems.PortalLib.BusinessObjects;

public partial class EventInfo : BaseUserControl
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		ActionsMenuClientId = menuActions.ClientID;
		menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;

		reqDate.Visible = reqEventName.Visible = false;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		
		calendar.Format = String.Format("{0} {1}",
										Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern,
										Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		RegisterJavaScript();
	}

	#region Properties

	public UserEvent SelectedUserEvent
	{
		get { return (UserEvent)_selectedEntity; }
	}

	protected new UserEventInfoCtlController Controller
	{
		get { return (UserEventInfoCtlController)base.Controller; }
	}

	public override object SelectedEntity
	{
		set
		{
			base.SelectedEntity = value;
			setReadOnly(!ValidateUserRight());
		}
	}

	#endregion

	#region Methods

	protected void RegisterJavaScript()
	{
		if (!(Page.ClientScript.IsClientScriptIncludeRegistered("EventInfoController")))
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "EventInfoController",
																	  "../controls/events/EventInfoController.js");

		if (!(Page.ClientScript.IsStartupScriptRegistered("InitializeEventInfoControllerObject")))
			Page.ClientScript.RegisterStartupScript(GetType(), "InitializeEventInfoControllerObject"
														  , InitializeEventInfoControllerScript);

		cbPersonalEvent.InputAttributes["onclick"] = string.Format("{0}.CheckBoxes_OnClick(this, '{1}');",
																	ClientJSObjectName, cblUsers.ClientID);

		cbGroupEvent.InputAttributes["onclick"] = string.Format("{0}.CheckBoxes_OnClick(this, '{1}');",
																 ClientJSObjectName, checkBoxListGroup.ClientID);
	}

	private string InitializeEventInfoControllerScript
	{
		get
		{
			var script = new StringBuilder();
			script.Append("<script language='javascript' type='text/javascript'>");
			script.AppendFormat("var {0} = new EventInfoController('{0}', '{1}', '{2}');",
								   ClientJSObjectName,
								   cbPersonalEvent.ClientID, cbGroupEvent.ClientID);

			script.AppendFormat("{0}.InitCBLists('{1}', '{2}');"
								, ClientJSObjectName
								, cblUsers.ClientID
								, checkBoxListGroup.ClientID);

			script.Append("</script>");

			return script.ToString();
		}
	}

	private void setReadOnly(bool value)
	{
		tbEventDate.Enabled = !value;
		tbEventName.Enabled = !value;
		tbDescription.Disabled = value;
		dropDownFormat.Enabled = !value;
		checkBoxPublicEvent.Enabled = !value;

		calendar.Enabled = !value;

		setPersonalEnable(!value);
		setGroupEnable(!value);
	}

	#region process enable of check box lists

	private void setPersonalEnable(bool value)
	{
		cblUsers.Enabled = value;

		if (!value)
			cbPersonalEvent.InputAttributes.Add("disabled", "disabled");
		else
			cbPersonalEvent.InputAttributes.Remove("disabled");
	}

	private void setGroupEnable(bool value)
	{
		checkBoxListGroup.Enabled = value;

		if (!value)
			cbGroupEvent.InputAttributes.Add("disabled", "disabled");
		else
			cbGroupEvent.InputAttributes.Remove("disabled");
	}

	#endregion

	#endregion

	#region DataBinding and DataValidating

	[DataBinding("DateTime", null, false)]
	public DateTime DateTime
	{
		set
		{
			calendar.SelectedDate = (value == DateTime.MinValue)
										? DateTime.Now
										: value;
		}
		get
		{
			return string.IsNullOrEmpty(tbEventDate.Text)
					   ? System.DateTime.Now
					   : Convert.ToDateTime(tbEventDate.Text);
		}
	}

	[DataBinding("Title", "", false)]
	public string Title
	{
		set { tbEventName.Text = value; }
		get { return tbEventName.Text; }
	}

	[DataBinding("Description", "", false)]
	public string Description
	{
		set { tbDescription.Value = value; }
		get { return tbDescription.Value; }
	}

	[DataBinding("IsPublic", true, false)]
	public bool PublicEvent
	{
		set { checkBoxPublicEvent.Checked = value; }
		get { return checkBoxPublicEvent.Checked; }
	}

	[DataBinding("OwnersUsersID", null, false)]
	public IList<int> OwnersUsersID
	{
		set
		{
			cblUsers.ClearSelection();
			cbPersonalEvent.Checked = (value != null && value.Count != 0);

			if (!ValidateUserRight())
			{
				cbPersonalEvent.InputAttributes.Add("disabled", "disabled");
				cblUsers.Enabled = false;
			}

			if (value == null)
				return;

			foreach (int userID in value)
			{
				cblUsers.Items.FindByValue(userID.ToString()).Selected = true;
			}
		}
		get
		{
			IList<int> resList = new List<int>();

			if (!cbPersonalEvent.Checked)
				return resList;

			foreach (ListItem item in cblUsers.Items)
			{
				if (item.Selected)
					resList.Add(int.Parse(item.Value));
			}

			return resList;
		}
	}

	[DataBinding("OwnersGroups", null, false)]
	public IList<Role> OwnersGroups
	{
		set
		{
			checkBoxListGroup.ClearSelection();
			cbGroupEvent.Checked = (value != null && value.Count != 0);

			if (!ValidateUserRight())
			{
				cbGroupEvent.InputAttributes.Add("disabled", "disabled");
				checkBoxListGroup.Enabled = false;
			}

			if (value == null)
				return;

			foreach (Role role in value)
			{
				checkBoxListGroup.Items.FindByValue(role.RoleID).Selected = true;
			}
		}
		get
		{
			IList<Role> resList = new List<Role>();
			if (!cbGroupEvent.Checked)
				return resList;

			foreach (ListItem item in checkBoxListGroup.Items)
			{
				if (item.Selected)
				{
					Role role = Role.GetRole(item.Value);
					resList.Add(role);
				}
			}

			return resList;
		}
	}

	private string OwnerIDKey
	{
		get { return string.Format("{0}_OwnerIdkey", ClientID); }
	}

	[DataBinding("OwnerID", null, false)]
	public int OwnerID
	{
		set
		{
			ViewState[OwnerIDKey] = value;
			if (value == 0)
			{
				lblAuthorName.Text = String.Empty;
				return;
			}

			Person author = new Person();
			author.Load(value);

			lblAuthorName.Text = author.ID.HasValue
									 ? author.FullName
									 : String.Format("can't find user with ID '{0}'.", value);
		}
		get
		{
			return (ViewState[OwnerIDKey] == null || (int)ViewState[OwnerIDKey] == 0)
					   ? Page.CurrentUser.ID.Value
					   : (int)ViewState[OwnerIDKey];
		}
	}

	[DataBinding("DateFormat", "", false)]
	public string DateFormat
	{
		set
		{
			if (dropDownFormat.Items.FindByValue(value) != null)
				dropDownFormat.SelectedValue = value;
		}
		get { return dropDownFormat.SelectedValue; }
	}

	[DataValidating]
	public bool ValidateUserRight()
	{
		return SelectedUserEvent != null
				   ? Page.CurrentUser.IsCanEditEvent(SelectedUserEvent)
				   : true;
	}

	[DataValidating]
	public bool ValidateData()
	{
		reqDate.Visible = string.IsNullOrEmpty(tbEventDate.Text);
		return !reqDate.Visible;
	}

	[DataValidating(true)]
	public void ClearValidateData()
	{
		reqDate.Visible = false;
	}

	[DataValidating]
	public bool ValidateEventName()
	{
		reqEventName.Visible = string.IsNullOrEmpty(tbEventName.Text);
		return !reqEventName.Visible;
	}

	[DataValidating(true)]
	public void ClearValidateEventName()
	{
		reqEventName.Visible = false;
	}

	#endregion

	#region Mapping

	public IList<Role> Roles
	{
		set
		{
			checkBoxListGroup.DataTextField = "Name";
			checkBoxListGroup.DataValueField = "RoleID";
			checkBoxListGroup.DataSource = value;
			checkBoxListGroup.DataBind();
		}
	}

	public IList<Person> Users
	{
		set
		{
			cblUsers.DataTextField = "FullName";
			cblUsers.DataValueField = "ID";
			cblUsers.DataSource = value;
			cblUsers.DataBind();
		}
	}

	public ListItemCollection DateFormats
	{
		set
		{
			dropDownFormat.DataTextField = "Text";
			dropDownFormat.DataValueField = "Value";
			DataBinder.BindDropDownDataSource(dropDownFormat, value);
		}
	}

	#endregion
}
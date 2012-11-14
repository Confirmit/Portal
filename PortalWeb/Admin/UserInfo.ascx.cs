using System;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Admin_UserInfol : BaseUserControl
{
	#region События

	public event EventHandler Apply;
	public event EventHandler Cancel;
	
    #endregion

	#region Свойства
	
    /// <summary>
	/// ID пользователя.
	/// </summary>
	public int? UserID
	{
		get
		{
		    return ViewState["UserID"] == null
		               ? (int?) null
		               : (int) ViewState["UserID"];
		}
	    set 
		{ 
			ViewState["UserID"] = value;
            fillUserInfo();
		}
	}

	#endregion

	#region Обработчики событий

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        if (ddlSex.Items.Count == 0)
            FillSexList();

        wzrdUserInfo_ActiveStepChanged(this, EventArgs.Empty);
    }

    /// <summary>
	/// Обработчик нажатия на кнопку Apply.
	/// </summary>
	protected void btnApply_Click(object sender, EventArgs e)
	{
		OnApply();
	}

	/// <summary>
	/// Обработчик нажатия на кнопку Cancel.
	/// </summary>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
        fillUserInfo();
		OnCancel();
	}

	/// <summary>
	/// Запускает событие применения данных.
	/// </summary>
    protected virtual void OnApply()
	{
	    if (Apply != null)
	        Apply(this, EventArgs.Empty);
	}

    /// <summary>
	/// Запускает событие отмены изменений.
	/// </summary>
	protected virtual void OnCancel()
	{
		if (Cancel != null)
		    Cancel(this, EventArgs.Empty);
	}

	protected void wzrdUserInfo_ActiveStepChanged( object sender, EventArgs e )
	{
		wzrdUserInfo.HeaderText = wzrdUserInfo.ActiveStep.Title;
	}

	#endregion

	#region Методы

	/// <summary>
	/// Заполняет список полов.
	/// </summary>
	protected void FillSexList()
	{
		// Заполнить список полов.
		ddlSex.Items.Clear();
		for (int i = 0; i < 3; i++)
		{
			ddlSex.Items.Add(((Person.UserSex)i).ToString());
		}
	}

    public void OnUserChanging(object sender, SelectedObjectEventArgs e)
    {
        UserID = e.ObjectID;
    }

	/// <summary>
	/// Заполняет элементы управления информацией о пользователях.
	/// </summary>
	private void fillUserInfo()
	{
		Person user = new Person();
		if (UserID != null)
		    user.Load(UserID.Value);

		tbFirstName.MultilingualText = user.FirstName;
		tbMiddleName.MultilingualText = user.MiddleName;
		tbLastName.MultilingualText = user.LastName;

		if (ddlSex.Items.Count == 0)
			FillSexList();

		ddlSex.SelectedIndex = (int)user.Sex;
        tbBirthday.Text = user.Birthday != null
	                          ? user.Birthday.Value.ToShortDateString()
	                          : string.Empty;

		dnEditor.UserID = user.ID;
		tbPrimaryEMail.Text = user.PrimaryEMail;
		tbProject.Text = user.Project;
		tbRoom.Text = user.Room;
		tbPrimaryIP.Text = user.PrimaryIP;
		gmEditor.UserID = user.ID;
	}

	/// <summary>
	/// Saves information about user and returns modified user.
	/// </summary>
	/// <returns>Object of modified user.</returns>
    public virtual Person SaveUserChanges()
	{
	    Person user = new Person();
	    if (UserID != null)
	        user.Load(UserID.Value);

	    user.FirstName = tbFirstName.MultilingualText;
	    user.MiddleName = tbMiddleName.MultilingualText;
	    user.LastName = tbLastName.MultilingualText;

	    user.Sex = (Person.UserSex) ddlSex.SelectedIndex;
	    user.Birthday = string.IsNullOrEmpty(tbBirthday.Text)
	                        ? (DateTime?) null
	                        : DateTime.Parse(tbBirthday.Text);

	    user.PrimaryEMail = tbPrimaryEMail.Text;
	    user.Project = tbProject.Text;
	    user.Room = tbRoom.Text;
	    user.PrimaryIP = tbPrimaryIP.Text;
	    user.Save();

	    dnEditor.GenerateDomainNames(user.ID.Value);
	    gmEditor.GenerateMembership(user.ID.Value);

	    return user;
	}

    #endregion
}

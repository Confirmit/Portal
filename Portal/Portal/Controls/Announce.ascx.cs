using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.NewsManager;
using UlterSystems.PortalLib.NewsTape;

public partial class NewsTape_Announce : BaseUserControl
{
	#region Fields
	[DebuggerBrowsable( DebuggerBrowsableState.Never )]
	private bool m_AllowOfficeNewsEditors = false;
	[DebuggerBrowsable( DebuggerBrowsableState.Never )]
	private bool m_AllowGeneralNewsEditors = true;
	#endregion

	#region Properties
	/// <summary>
	/// Does office news editor allowed to edit this news.
	/// </summary>
	public bool AllowOfficeNewsEditors
	{
		[DebuggerStepThrough]
		get { return m_AllowOfficeNewsEditors; }
		[DebuggerStepThrough]
		set { m_AllowOfficeNewsEditors = value; }
	}

	/// <summary>
	/// Does general news editor allowed to edit this news.
	/// </summary>
	public bool AllowGeneralNewsEditors
	{
		[DebuggerStepThrough]
		get { return m_AllowGeneralNewsEditors; }
		[DebuggerStepThrough]
		set { m_AllowGeneralNewsEditors = value; }
	}
	#endregion

	protected void Page_Load( object sender, EventArgs e )
	{
		this.Page.RegisterRequiresControlState( this );

		//------ Text
		list.Text = (string)GetLocalResourceObject("lblNews.Text");
		search.Text = (string)GetLocalResourceObject("lblSearch.Text");
		archive.Text = (string)GetLocalResourceObject("lblArchive.Text");
		hlAddNews.Text = (string)GetLocalResourceObject("hlAddNews.Text");
		//-------

		hlAddNews.Visible = false;
		if (Page.CurrentUser != null)
		{
			if (AllowOfficeNewsEditors && Page.CurrentUser.IsInRole(RolesEnum.OfficeNewsEditor))
				hlAddNews.Visible = true;

			if (AllowGeneralNewsEditors && Page.CurrentUser.IsInRole(RolesEnum.GeneralNewsEditor))
				hlAddNews.Visible = true;

			// Получить пользователя, просматривающего страницу.
			ds.SelectParameters["personID"].DefaultValue = Page.CurrentUser.ID.Value.ToString();
			gvNews.EmptyDataText = this.GetGlobalResourceObject("NewsTape", "EmptyActualNews").ToString();
		}
	}

	protected void gvNews_RowDataBound( object sender, GridViewRowEventArgs e )
	{
		if( e.Row.RowType == DataControlRowType.DataRow )
		{
			News news = e.Row.DataItem as News;
			Image image = e.Row.FindControl( "imgNewsType" ) as Image;
			Image imageAttach = e.Row.FindControl("imgHasAttach") as Image;
			
			setOfficeImage(news, image);
			SetAttachImage(news, imageAttach);

			// Set time of news.
			TableCell cell = e.Row.Cells[3];
			if( news.CreateTime.Date == DateTime.Today )
				cell.Text = this.GetLocalResourceObject( "Today" ).ToString();
			else
				cell.Text = news.CreateTime.ToShortDateString();

			cell.Text += Environment.NewLine + news.CreateTime.ToShortTimeString();
		}
	}

	#region Controls state persistence

	protected override void LoadControlState( object savedState )
	{
		object[] state = (object[]) savedState;
		base.LoadControlState( state[ 0 ] );
		m_AllowOfficeNewsEditors = (bool) state[ 1 ];
		m_AllowGeneralNewsEditors = (bool) state[ 2 ];
	}

	protected override object SaveControlState()
	{
		object[] state = new object[ 3 ];
		state[ 0 ] = base.SaveControlState();
		state[ 1 ] = m_AllowOfficeNewsEditors;
		state[ 2 ] = m_AllowGeneralNewsEditors;
		return state;
	}

	#endregion

	#region Methods

	/// <summary>
	/// Set image of news type.
	/// </summary>
	/// <param name="news">News.</param>
	/// <param name="image">Image control.</param>
	private void setOfficeImage(News news, Image image)
	{
		if (image == null)
			return;

		String strOffice = String.Empty;

		if (news.OfficeID == 0) // общие новости.
		{
			image.ImageUrl = "~/Images/generalNewsImage.gif";
			strOffice = GetGlobalResourceObject("NewsTape", "generalNews").ToString();
		}
		else //новости офисов.
		{
			strOffice = news.OfficeName;
			image.ImageUrl = ConfigurationManager.AppSettings["officeNewsImage" + news.OfficeName];
		}
		image.ToolTip = strOffice;
		image.AlternateText = strOffice;
	}

	/// <summary>
	/// Set image if news has attach.
	/// </summary>
	/// <param name="news">News.</param>
	/// <param name="image">Image control.</param>
	private void SetAttachImage(News news, Image image)
	{
		if (image == null)
			return;

		if (news.Attachments.Count == 0)
		{
			image.Visible = false;
			return;
		}

		image.ImageUrl = "~/Images/attachments/clip.gif";
		image.AlternateText = "Has attachments";
		image.ToolTip = String.Format("Has attachments ({0})", news.Attachments.Count);
	}

	/// <summary>
	/// Get the author E-Mail of news.
	/// </summary>
	/// <param name="objContainer">data container.</param>
	/// <returns></returns>
	protected String GetNewsAuthorMail(Object objContainer)
	{
		GridViewRow row = objContainer as GridViewRow;
		if (row == null)
			return String.Empty;

		Person author = new Person();
		author.Load(((News)row.DataItem).AuthorID);

		if (String.IsNullOrEmpty(author.PrimaryEMail))
			return String.Empty;

		return String.Format("mailto:{0}", author.PrimaryEMail);
	}
	#endregion
}

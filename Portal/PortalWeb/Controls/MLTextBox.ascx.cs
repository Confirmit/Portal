using System;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Generic;
using System.Threading;

using Core;

/// <summary>
/// Text box for multilingual text.
/// </summary>
public partial class Controls_MLTextBox : BaseUserControl
{
	#region Properties

    public bool Enabled
    {
        set { tbText.Enabled = value; }
    }

	/// <summary>
	/// Width of control.
	/// </summary>
	public Unit Width
	{
		get { return mlTextPanel.Width; }
		set { mlTextPanel.Width = value; }
	}

	/// <summary>
	/// Multilingual text.
	/// </summary>
	public MLText MultilingualText
	{
		get
		{
			OnCultureChanged( this, EventArgs.Empty );
			return InternalMultilingualText;
		}
		set { InternalMultilingualText = value; }
	}


	/// <summary>
	/// Multilingual text for internal using.
	/// </summary>
	private MLText InternalMultilingualText
	{
		get
		{
			if( ViewState[ "MLText" ] == null )
			{ ViewState[ "MLText" ] = new MLText(); }

			return ( ViewState[ "MLText" ] as MLText );
		}
		set
		{
			ViewState[ "MLText" ] = value;
			SetText();
		}
	}

	/// <summary>
	/// Current culture.
	/// </summary>
	protected string CurrentCulture
	{
		get
		{
			if( ViewState[ "CurrentCulture" ] == null )
			{
				try
				{
					// Set current culture.
					CultureInfo ci = new CultureInfo( Thread.CurrentThread.CurrentCulture.Name.Substring( 0, 2 ) );
					ViewState[ "CurrentCulture" ] = ci.Name;
				}
				catch( Exception ex )
				{
					ViewState[ "CurrentCulture" ] = "en";
					ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
				}
			}
			return ( ViewState[ "CurrentCulture" ] as string );
		}
		set
		{
			ViewState[ "CurrentCulture" ] = value;
			SetText();
		}
	}

    public string DropDownListCssClass
    {
        set { ddlCultures.CssClass = value; }
        get { return ddlCultures.CssClass; }
    }

    public string TextBoxCssClass
    {
        set { tbText.CssClass = value; }
        get { return tbText.CssClass; }
    }

	#endregion

	#region Event handlers
	/// <summary>
	/// Handles control loading.
	/// </summary>
	protected void Page_Load( object sender, EventArgs e )
	{
		if( !IsPostBack || (ddlCultures.Items.Count == 0) )
		{
			FillListOfCultures();

			try
			{
				// Set current culture.
				CultureInfo ci = new CultureInfo( Thread.CurrentThread.CurrentCulture.Name.Substring( 0, 2 ) );
				ddlCultures.SelectedValue = ci.Name;

				CurrentCulture = ci.Name;
			}
			catch( Exception ex )
			{
				ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex); 
			}
		}
	}

	/// <summary>
	/// Handles changing of current culture.
	/// </summary>
	protected void OnCultureChanged( object sender, EventArgs e )
	{
		try
		{
			if( !string.IsNullOrEmpty( tbText.Text ) )
				InternalMultilingualText[ CurrentCulture ] = tbText.Text;
			else if( InternalMultilingualText.ContainsCulture( CurrentCulture ) )
			{ InternalMultilingualText.RemoveText( CurrentCulture ); }

			CurrentCulture = ddlCultures.SelectedValue;
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex); 
		}
	}
	#endregion

	#region Methods
	/// <summary>
	/// Fills list of cultures.
	/// </summary>
	private void FillListOfCultures()
	{
		try
		{
			List<CultureInfo> culturesList =
					new List<CultureInfo>(
						CultureInfo.GetCultures( CultureTypes.NeutralCultures ) );
			culturesList.Sort(
				delegate( CultureInfo x, CultureInfo y )
				{ return string.Compare( x.EnglishName, y.EnglishName ); } );
			ddlCultures.DataSource = culturesList;
			ddlCultures.DataBind();
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}

	/// <summary>
	/// Sets text according to the current culture.
	/// </summary>
	private void SetText()
	{
		try
		{
			if( InternalMultilingualText.ContainsCulture( CurrentCulture ) )
				tbText.Text = InternalMultilingualText[ CurrentCulture ];
			else
				tbText.Text = string.Empty;
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}

	/// <summary>
	/// Reloads list of languages and sets current language.
	/// </summary>
	public void ReloadLanguages()
	{
		FillListOfCultures();

		try
		{
			// Set current culture.
			CultureInfo ci = new CultureInfo( Thread.CurrentThread.CurrentCulture.Name.Substring( 0, 2 ) );
			ddlCultures.SelectedValue = ci.Name;

			CurrentCulture = ci.Name;
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}
	#endregion
}

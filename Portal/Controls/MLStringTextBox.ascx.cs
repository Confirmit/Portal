using System;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Generic;
using System.Threading;

using Core;

/// <summary>
/// Text box for multilingual text.
/// </summary>
public partial class MLStringTextBox : BaseUserControl
{
	#region Properties

	/// <summary>
	/// Multilingual text.
	/// </summary>
	public MLString MultilingualText
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
    private MLString InternalMultilingualText
	{
		get
		{
			if( ViewState[ "MLStringTextBox" ] == null )
            { ViewState["MLStringTextBox"] = new MLString(); }

            return (ViewState["MLStringTextBox"] is MLString ? (MLString)ViewState["MLStringTextBox"] : new MLString());
		}
		set
		{
            ViewState["MLStringTextBox"] = value;
			SetText();
		}
	}

	/// <summary>
	/// Current culture.
	/// </summary>
    protected CultureManager.Languages CurrentCulture
	{
		get
		{
			if( ViewState[ "CurrentCulture" ] == null )
			{
				try
				{
                    //TODO
					CultureInfo ci = new CultureInfo( Thread.CurrentThread.CurrentCulture.Name.Substring( 0, 2 ) );
					ViewState[ "CurrentCulture" ] = ci.Name;
				}
				catch( Exception ex )
				{
					ViewState[ "CurrentCulture" ] = "en";
					ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
				}
			}
            return (ViewState["CurrentCulture"] is CultureManager.Languages ? (CultureManager.Languages) ViewState["CurrentCulture"] : CultureManager.Languages.Russian);
		}
		set
		{
			ViewState[ "CurrentCulture" ] = value;
			SetText();
		}
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
                //TODO
				CultureInfo ci = new CultureInfo( Thread.CurrentThread.CurrentCulture.Name.Substring( 0, 2 ) );
				ddlCultures.SelectedValue = ci.Name;

                CurrentCulture = CultureManager.Languages.Russian;
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
            //TODO
            //if (!string.IsNullOrEmpty(tbText.Text))
            //{
            //    InternalMultilingualText[CurrentCulture] = tbText.Text;
            //}
            //else if (InternalMultilingualText[CurrentCulture] != "")
            //{
            //    InternalMultilingualText.RemoveText( CurrentCulture );
            //}

            //CurrentCulture = ddlCultures.SelectedValue;
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
		    var culturesList =
		        new List<CultureInfo> {CultureInfo.GetCultureInfo("en"), CultureInfo.GetCultureInfo("ru")};
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
			if(InternalMultilingualText[CurrentCulture] != "" )
                tbText.Text = InternalMultilingualText[CurrentCulture];
			else
				tbText.Text = string.Empty;
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
    }
    #endregion
}

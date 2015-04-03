using System;
using System.Globalization;
using System.Collections.Generic;
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
            SaveMultilanguageText();
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
		if( !IsPostBack || (DropDownListCultures.Items.Count == 0) )
		{
			FillListOfCultures();

			try
			{
				DropDownListCultures.SelectedValue = CultureInfo.GetCultureInfo("ru").Name;
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
		    SaveMultilanguageText();
		    ChangeCulture();
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex); 
		}
	}

    private void SaveMultilanguageText()
    {
        string englishValue;
        string russianValue;
        if (CurrentCulture == CultureManager.Languages.English)
        {
            englishValue = TextBoxContent.Text;
            russianValue = InternalMultilingualText.RussianValue;
        }
        else
        {
            russianValue = TextBoxContent.Text;
            englishValue = InternalMultilingualText.EnglishValue;
        }
        InternalMultilingualText = new MLString(russianValue, englishValue);
    }

    #endregion

	#region Methods

    private void ChangeCulture()
    {
        if(CurrentCulture == CultureManager.Languages.English)
            CurrentCulture = CultureManager.Languages.Russian;
        else
            CurrentCulture = CultureManager.Languages.English;
    }

	/// <summary>
	/// Fills list of cultures.
	/// </summary>
	private void FillListOfCultures()
	{
		try
		{
		    var culturesList =
		        new List<CultureInfo> {CultureInfo.GetCultureInfo("en"), CultureInfo.GetCultureInfo("ru")};
			DropDownListCultures.DataSource = culturesList;
			DropDownListCultures.DataBind();
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
                TextBoxContent.Text = InternalMultilingualText[CurrentCulture];
			else
				TextBoxContent.Text = string.Empty;
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
    }
    #endregion
}

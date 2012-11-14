using System;
using System.Globalization;
using System.Threading;

using Core;
using System.Collections.Generic;

public partial class Controls_CultureChanger : BaseUserControl
{
	protected void Page_Load( object sender, EventArgs e )
	{
		if( !IsPostBack )
		{
			FillListOfCultures();

            try
            {
                // Set current culture.
                CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2));
                ddlCultures.SelectedValue = ci.Name;
            }
            catch (Exception ex)
            {
				ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
            }
		}
	}

	/// <summary>
	/// Fills list of cultures.
	/// </summary>
    private void FillListOfCultures()
	{
	    try
	    {
	        List<CultureInfo> culturesList = new List<CultureInfo>(2);

	        culturesList.Add(new CultureInfo("ru"));
	        culturesList.Add(new CultureInfo("en"));

	        culturesList.Sort(
	            delegate(CultureInfo x, CultureInfo y)
	                {
	                    return string.Compare(x.EnglishName, y.EnglishName);
	                }
	            );
	        ddlCultures.DataSource = culturesList;
	        ddlCultures.DataBind();

	        ddlCultures.SelectedValue = "en";
	    }
	    catch (Exception ex)
	    {
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
	    }
	}

    /// <summary>
	/// Handles changing of current culture.
	/// </summary>
	protected void OnCultureChanged( object sender, EventArgs e )
	{
		try
		{
			MLText.CurrentCultureID = ddlCultures.SelectedItem.Value;
			Page.RedirectToMySelf();
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}
}

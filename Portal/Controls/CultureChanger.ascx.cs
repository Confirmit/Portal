using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using Core;

public partial class Controls_CultureChanger : BaseUserControl
{
	#region Life Cycle

	protected override void OnInit(EventArgs e)
	{
		ddlCultures.SelectedIndexChanged += ddlCultures_SelectedIndexChanged;
		
		base.OnInit(e);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (IsPostBack) return;

		BindCultures();

		try
		{
			var ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2));
			ddlCultures.SelectedValue = ci.Name;
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	} 

	#endregion

	#region Event Handlers

	void ddlCultures_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			MLText.CurrentCultureID = ddlCultures.SelectedItem.Value;
            CultureManager.SetLanguage(MLText.CurrentCultureID);
			Page.RedirectToMySelf();
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}

	#endregion

	#region Methods

	private void BindCultures()
	{
		try
		{
			var culturesList = new List<CultureInfo>
			                   	{
			                   		new CultureInfo("ru"), 
									new CultureInfo("en")
			                   	};

			culturesList.Sort((x, y) => string.Compare(x.EnglishName, y.EnglishName));

			ddlCultures.DataSource = culturesList;
			ddlCultures.DataBind();
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	} 

	#endregion
}
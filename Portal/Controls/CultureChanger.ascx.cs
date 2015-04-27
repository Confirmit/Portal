using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using Core;

public partial class Controls_CultureChanger : BaseUserControl
{
	#region Life Cycle

	protected override void OnInit(EventArgs e)
	{
		DropDownListCultures.SelectedIndexChanged += DropDownListCulturesSelectedIndexChanged;
		
		base.OnInit(e);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (IsPostBack) return;

		BindCultures();

		try
		{
			var ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2));
			DropDownListCultures.SelectedValue = ci.Name;
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	} 

	#endregion

	#region Event Handlers

	void DropDownListCulturesSelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			MLText.CurrentCultureID = DropDownListCultures.SelectedItem.Value;
            CultureManager.SetLanguage(DropDownListCultures.SelectedItem.Value);
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

			DropDownListCultures.DataSource = culturesList;
			DropDownListCultures.DataBind();
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	} 

	#endregion
}
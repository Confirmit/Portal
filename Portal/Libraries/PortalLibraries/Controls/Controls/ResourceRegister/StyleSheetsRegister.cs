using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Controls.ResourceRegister
{
	public class StyleSheetsRegister : ResourceRegister
	{
		#region Overrides

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (Page != null && Page.Header != null)
			{
				var htmlLinks =  Page.Header.Controls.OfType<HtmlLink>();

				foreach(var htmlLink in htmlLinks)
				{
					if (htmlLink.Href.Contains("css"))
						htmlLink.Visible = false;
				}
			}
		}

		protected override void RenderMinResource(HtmlTextWriter writer)
		{
			writer.WriteLine(@"<link href=""" + GetThemePath("css/css.min.css") + @""" rel=""stylesheet"" type=""text/css"" />");
		}

		protected override void RenderResource(string virtualPath, HtmlTextWriter writer)
		{
			writer.WriteLine(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", virtualPath));
		}

		#endregion

		#region Methods

		public string GetThemePath(string relativePath)
		{
			relativePath = relativePath.TrimStart('/');
			var result = "~/App_Themes/" + Page.Theme + "/" + relativePath;

			return ResolveUrl(result);
		}

		#endregion
	}
}
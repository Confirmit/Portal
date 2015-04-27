using System.Web.UI;

namespace Controls.ResourceRegister
{
	public class ScriptsRegister : ResourceRegister
	{
		#region Overrides

		protected override void RenderMinResource(HtmlTextWriter writer)
		{
			writer.WriteLine(@"<script type=""text/javascript"" src=""/Scripts/scripts.min.js""></script>");
		}

		protected override void RenderResource(string virtualPath, HtmlTextWriter writer)
		{
			writer.WriteLine(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", virtualPath));
		}

		#endregion
	}
}
using System.IO;
using System.Web.UI;
using System.Xml.Linq;

namespace Controls.ResourceRegister
{
	public abstract class ResourceRegister : Control
	{
		#region Properties

		public string XmlFileDescriptionPath { get; set; }

		#endregion

		#region Overrides

		protected override void Render(HtmlTextWriter writer)
		{
#if DEBUG
			if (string.IsNullOrEmpty(XmlFileDescriptionPath))
				return;

			var physicalPath = Page.Server.MapPath(XmlFileDescriptionPath);

			if (!File.Exists(physicalPath))
				throw new FileNotFoundException(string.Format("Cannot found file specified: {0}.", XmlFileDescriptionPath));

			var xDocument = XDocument.Load(Page.Server.MapPath(XmlFileDescriptionPath));

			var scriptPaths = xDocument.Elements("root").Elements("output").Elements("input").Attributes("path");

			foreach (var scriptPath in scriptPaths)
			{
				var virtualPath = Page.ResolveUrl(scriptPath.Value.Replace("..", "~").Replace("\\", "/"));

				RenderResource(virtualPath, writer);
			}
#else			
			RenderMinResource(writer);
#endif
		}

		#endregion

		#region Methods

		protected abstract void RenderMinResource(HtmlTextWriter writer);

		protected abstract void RenderResource(string virtualPath, HtmlTextWriter writer);

		#endregion
	}
}
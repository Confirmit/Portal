using System;
using System.Web.UI;
using System.Xml;

namespace Controls.WebGenerator
{
    [ToolboxData("<{0}:Menu runat=server></{0}:Menu>")]
    public class Menu : MenuBase
    {
        private String xmlMenu;

        public String XmlMenu
        {
            get { return xmlMenu; }
            set { xmlMenu = value; }
        }

        protected override void RenderMenu(HtmlTextWriter writer)
        {
            XmlDocument xmlMenuD = new XmlDocument();
            String mapPath = Page.MapPath("~/");
            xmlMenuD.Load(mapPath + XmlMenu);

            String page = Page.AppRelativeVirtualPath;
            page = page.Replace("~/", "");
            Generator generator = new Generator(
                Resource.WebGenerator_vmenu,
                xmlMenuD.GetElementsByTagName("node"),
                page, controllerObjectName);
            String arr = Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.WebGenerator.img.arr.gif");

            writer.Write(generator.BuildMenu(arr, "", ClientID));
        }
    }
}
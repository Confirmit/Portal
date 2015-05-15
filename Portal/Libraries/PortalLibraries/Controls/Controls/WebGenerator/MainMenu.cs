using System;
using System.Web.UI;
using System.Xml;

namespace Controls.WebGenerator
{
    [ToolboxData("<{0}:MainMenu runat=server></{0}:MainMenu>")]
    public class MainMenu : MenuBase
    {
        private XmlElement xmlMenu;

        public XmlElement XmlMainMenu
        {
            get { return xmlMenu; }
            set { xmlMenu = value; }
        }

        protected override void RenderMenu(HtmlTextWriter writer)
        {
            String page = Page.AppRelativeVirtualPath;
            page = page.Replace("~/", "");
            Generator generator = new Generator(
                Resource.WebGenerator_vmenu,
                xmlMenu.GetElementsByTagName("node"),
                page, controllerObjectName);
            String arr = Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.WebGenerator.img.arr.gif");

            writer.Write(generator.BuildMenu(arr, "", ClientID));
            ;
        }
    }
}
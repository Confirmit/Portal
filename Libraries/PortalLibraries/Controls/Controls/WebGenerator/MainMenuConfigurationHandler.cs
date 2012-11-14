using System.Configuration;
using System.Xml;

namespace Controls.WebGenerator
{
    public class MainMenuConfigurationHandler : IConfigurationSectionHandler
    {
        public MainMenuConfigurationHandler()
        {
        }

        public object Create(object parent, object configContext, XmlNode xmlNode)
        {
            return xmlNode;
        }
    }
}

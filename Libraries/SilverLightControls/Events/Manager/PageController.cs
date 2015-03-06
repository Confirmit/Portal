using System;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Windows.Controls;
using System.Xml.Linq;

using Events.SLServiceReference;
using Helpers;

namespace Events.Manager
{
    public class PageController
    {
        #region Fields

        private Panel m_menuPanel = null;

        #endregion

        #region WCF Service communication

        public void GetUserEvents(string SLService, int userID, EventHandler<GetEventsForUserCompletedEventArgs> callbackFunction)
        {
            SLServiceClient client = new SLServiceClient(new BasicHttpBinding(),
                                             new EndpointAddress(PathHelper.GetAbsoluteUrl(SLService)));
            client.GetEventsForUserCompleted +=
                callbackFunction ?? GetEventsForUserCompleted;

            client.GetEventsForUserAsync(userID);
        }

        #endregion

        #region Async Methods

        protected virtual void GetEventsForUserCompleted(object sender, GetEventsForUserCompletedEventArgs e)
        {
        }

        #endregion

        #region Menu Support

        public void PrepareMenu(String menuPath, Panel menuPanel)
        {
            m_menuPanel = menuPanel;

            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            Uri uri = new Uri(PathHelper.GetAbsoluteUrl(menuPath), UriKind.Absolute);
            wc.DownloadStringAsync(uri);
        }

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (m_menuPanel == null)
                return;

            XElement root = XElement.Parse(e.Result);

            // MenuItems
            var q = from mi in root.Elements("MenuItem")
                    select new
                    {
                        Title = mi.Attribute("Title").Value,
                        Image = mi.Attribute("Image").Value,
                        NavigateUrl = mi.Attribute("NavigateUrl").Value
                    };

            foreach (var item in q)
            {
                var link = new HyperlinkButton
                {
                    NavigateUri = new Uri(PathHelper.GetAbsoluteUrl(item.NavigateUrl), UriKind.Absolute),
                    Content = item.Title
                };
                m_menuPanel.Children.Add(link);
            }
        }

        #endregion
    }
}

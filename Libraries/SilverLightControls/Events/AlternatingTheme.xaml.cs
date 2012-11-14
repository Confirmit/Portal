using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

using Events.Manager;
using Events.SLServiceReference;

using Helpers;

namespace Events
{
    public partial class AlternatingTheme : UserControl
    {
        #region Fields

        private readonly PageController m_controller = new PageController();

        #endregion

        #region Constructor

        public AlternatingTheme(string menuPath, string SLService, int userID, string culture)
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            m_controller.GetUserEvents(SLService, userID, GetEventsForUserCompleted);
            m_controller.PrepareMenu(menuPath, menuPanel);
        }

        #endregion

        #region Async Methods

        private void GetEventsForUserCompleted(object sender, GetEventsForUserCompletedEventArgs e)
        {
            Slider.ItemsSource = e.Result;
        }

        #endregion

        private void OnEventName_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null || element.DataContext == null)
                return;

            string strPath = String.Format("{0}?EventID={1}",
                               PathHelper.GetAbsoluteUrl("Events/UserEvents.aspx"),
                               ((Event)element.DataContext).ID.Value);
            HtmlPage.Window.Navigate(new Uri(strPath));
        }
    }
}
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Events.Manager;
using Events.SLServiceReference;

namespace Events
{
    public partial class Page : UserControl
    {
        #region Fields

        private readonly PageController m_controller = new PageController();

        #endregion

        #region Constructor

        public Page(string menuPath, string SLService, int userID, string culture)
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            m_controller.GetUserEvents(SLService, userID, GetEventsForUserCompleted);
            m_controller.PrepareMenu(menuPath, menuPanel);
        }

        #endregion

        #region Async Methods

        protected virtual void GetEventsForUserCompleted(object sender, GetEventsForUserCompletedEventArgs e)
        {
            if (e.Result == null || e.Result.Count == 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "- No Events";
                textBlock.FontSize = 16;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Foreground = new SolidColorBrush(Colors.Blue);

                dataPanel.Children.Add(textBlock);
                return;
            }

            foreach (Event userEvent in e.Result)
            {
                EventInfo ctlInfo = new EventInfo
                                        {
                                            Margin = new Thickness(2, 0, 0, 0),
                                            DataContext = userEvent
                                        };
                dataPanel.Children.Add(ctlInfo);
            }
        }

        #endregion
    }
}

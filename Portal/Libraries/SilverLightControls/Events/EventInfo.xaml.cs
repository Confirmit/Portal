using System;
using System.Windows.Browser;
using System.Windows.Controls;

using Events.Manager;
using Events.Resources;
using Events.SLServiceReference;

using Helpers;

namespace Events
{
    public partial class EventInfo : UserControl
    {
        #region Fields
        
        private EventInfoManager m_EventManager = null;
        
        #endregion

        #region Constructor

        public EventInfo()
        {
            InitializeComponent();

            lblEvent.Text = Resource.SLEvent;
            lblDate.Text = Resource.SLEventDate;
            lblWorkDays.Text = Resource.SLWorkDays;
            lblTimeCaption.Text = Resource.SLTimeCaption;

            Loaded += EventInfo_Loaded;
        }

        private void EventInfo_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            m_EventManager = new EventInfoManager(riseTime,
                                                  riseWorkdays,
                                                  EventInformation);

            m_EventManager.ConfigureTimer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Event information.
        /// </summary>
        public Event EventInformation
        {
            get { return DataContext as Event; }
        }

        /// <summary>
        /// Resource class for providing multilanguage.
        /// </summary>
        internal static Resource Resource
        {
            get { return new Resource(); }
        }

        #endregion

        #region Methods

        private void OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            string strPath = String.Format("{0}?EventID={1}",
                                           PathHelper.GetAbsoluteUrl("Events/UserEvents.aspx"),
                                           EventInformation.ID.Value);
            HtmlPage.Window.Navigate(new Uri(strPath));
        }

        #endregion
    }
}

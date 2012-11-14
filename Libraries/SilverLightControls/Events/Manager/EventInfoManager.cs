using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Events.SLServiceReference;


namespace Events.Manager
{
    public class EventInfoManager
    {
        #region Fields

        private Event m_Event = null;
        private int m_restDays = -1;

        private TextBlock m_riseTime = null;
        private TextBlock m_riseWorkDays = null;

        private readonly TimeSpan m_TimerTick = new TimeSpan(0, 0, 0, 1);
        private readonly DispatcherTimer m_Timer = new DispatcherTimer();

        #endregion

        public EventInfoManager(TextBlock riseTime,
                               TextBlock riseWorkDays,
                                Event eventInfo)
        {
            m_riseTime = riseTime;
            m_riseWorkDays = riseWorkDays;
            m_Event = eventInfo;
        }

        #region Properties

        #endregion

        #region Methods

        public void ConfigureTimer()
        {
            m_Timer.Interval = m_TimerTick;
            m_Timer.Tick += Each_Tick;
            m_Timer.Start();
        }

        /// <summary>
        /// Timer event.
        /// </summary>
        /// <param name="o">Timer.</param>
        /// <param name="sender">Args.</param>
        private void Each_Tick(object o, EventArgs sender)
        {
            CalculateRiseTime();

            if (m_Timer == null)
                return;

            if (IsHappend)
            {
                m_Timer.Tick -= Each_Tick;
                m_Timer.Stop();
            }
        }

        /// <summary>
        /// Get work days.
        /// </summary>
        /// <param name="TotalDays">Total days before event.</param>
        /// <returns>Number of work days.</returns>
        public int GetWorkDays(int TotalDays)
        {
            if (m_restDays == -1)
                m_restDays = TotalDays - m_Event.WorkDays;

            return TotalDays - m_restDays >= 0
                       ? TotalDays - m_restDays
                       : 0;
        }

        /// <summary>
        /// Event is happend.
        /// </summary>
        public bool IsHappend
        {
            get { return m_Event.DateTime <= DateTime.Now; }
        }

        /// <summary>
        /// Calculate rise time.
        /// </summary>
        public void CalculateRiseTime()
        {
            if (IsHappend)
            {
                //border.BorderBrush = new SolidColorBrush(Colors.Red);
                m_riseTime.Foreground = new SolidColorBrush(Colors.Red);
                m_riseTime.Text = "00:00:00";

                return;
            }

            TimeSpan rise = m_Event.DateTime - DateTime.Now;
            m_riseTime.Text = (rise.Days != 0)
                                ? String.Format("{0}, {1}",
                                                rise.Days,
                                                new DateTime(rise.Ticks).ToString("HH:mm:ss"))
                                : new DateTime(rise.Ticks).ToString("HH:mm:ss");

            m_riseWorkDays.Text = GetWorkDays(rise.Days).ToString();
        }

        #endregion
    }
}

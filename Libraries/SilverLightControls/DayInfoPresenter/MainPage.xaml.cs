using System;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Threading;

using Helpers;

using DayInfoPresenter.SLServiceReference;
using DayInfoPresenter.Resources;
using DayInfoPresenter.WorkEvents;
using DayInfoPresenter.Extensions;

namespace DayInfoPresenter
{
	// CAUTION: Control consist 4 buttons for providing user work events actions. 
	// This buttons and events duplicate in ~/Controls/NewDay.ascx control.
	// If will be the problems with silverlight buttons, please uncomment code in ascx control.
	public partial class MainPage : UserControl
	{
		#region Fields

		private TimeSpan m_TodayWork;
		private TimeSpan m_TodayRest;
		private TimeSpan m_WeekRest;
		private TimeSpan m_MonthRest;

		private int m_UserID = 0;
		private string m_ServiceURI = string.Empty;

		private ControlState m_State = ControlState.WorkFinished;

		private readonly DispatcherTimer m_DispatcherTimer = new DispatcherTimer();
		private readonly TimeSpan m_Timer = new TimeSpan(0, 0, 1, 0);
		private readonly Resource m_Resource = new Resource();

		#endregion

		#region Constructor

		public MainPage(int userID, string SLService, string Culture)
		{
			InitializeComponent();
			Thread.CurrentThread.CurrentCulture = new CultureInfo(Culture);

			m_UserID = userID;
			m_ServiceURI = SLService;

			#region WCFService events subscribe

			var wcfClient = new SLServiceClient(new BasicHttpBinding(),
			                                    new EndpointAddress(PathHelper.GetAbsoluteUrl(m_ServiceURI)));

			wcfClient.GetMainWorkAndLastEventCompleted += wcfClient_GetMainWorkAndLastEventCompleted;
			wcfClient.GetMainWorkAndLastEventAsync(m_UserID);

			loadFullDayInfo();
			loadTodayEvents();

			#endregion

			#region Buttons events subscribe

			btnWork.Click += OnBtnWorkClick;
			btnTime.Click += OnBtnTimeClick;
			btnLesson.Click += OnBtnLessonClick;
			btnDinner.Click += OnBtnDinnerClick;

			#endregion

			#region SLDataGrid events subscribe

			gridWorkEvents.SelectedTextChanged += OnGridWorkEventsSelectedTextChanged;

			#endregion
		}

		#endregion

		#region Properties

		/// <summary>
		/// State of control.
		/// </summary>
		public ControlState State
		{
			get { return m_State; }
			set
			{
				m_State = value;
				enableControls();

				if (value == ControlState.WorkGoes)
					startTimer();
			}
		}

		private bool isInitializedDateTime
		{
			get { return !(m_TodayWork == TimeSpan.Zero); }
		}

		#endregion

		#region Timer Tick

		/// <summary>
		/// Timer tick.
		/// </summary>
		/// <param name="sender">Dispather timer.</param>
		/// <param name="e">Event args.</param>
		private void OnTimerTick(object sender, EventArgs e)
		{
			if (State == ControlState.WorkFinished)
			{
				setText();
				return;
			}

			loadFullDayInfo();
			loadTodayEvents();
		}

		#endregion

		#region Buttons events

		private void OnBtnWorkClick(object sender, System.Windows.RoutedEventArgs e)
		{
			if (State == ControlState.WorkFinished)
			{
				setUserWorkEvent(true, WorkEventType.MainWork);
				State = ControlState.WorkGoes;
				return;
			}

			setUserWorkEvent(false, WorkEventType.MainWork);
			State = ControlState.WorkFinished;
		}

		private void OnBtnDinnerClick(object sender, System.Windows.RoutedEventArgs e)
		{
			if (State == ControlState.WorkGoes)
			{
				setUserWorkEvent(true, WorkEventType.LanchTime);
				State = ControlState.Feeding;
				return;
			}

			if (State == ControlState.Feeding)
			{
				setUserWorkEvent(false, WorkEventType.LanchTime);
				State = ControlState.WorkGoes;
				return;
			}
		}

		private void OnBtnLessonClick(object sender, System.Windows.RoutedEventArgs e)
		{
			if (State == ControlState.WorkGoes)
			{
				setUserWorkEvent(true, WorkEventType.StudyEnglish);
				State = ControlState.EnglishLesson;
				return;
			}

			if (State == ControlState.EnglishLesson)
			{
				setUserWorkEvent(false, WorkEventType.StudyEnglish);
				State = ControlState.WorkGoes;
				return;
			}
		}

		private void OnBtnTimeClick(object sender, System.Windows.RoutedEventArgs e)
		{
			if (State == ControlState.WorkGoes)
			{
				setUserWorkEvent(true, WorkEventType.TimeOff);
				State = ControlState.Absent;
				return;
			}

			if (State == ControlState.Absent)
			{
				setUserWorkEvent(false, WorkEventType.TimeOff);
				State = ControlState.WorkGoes;
				return;
			}
		}

		#endregion

		#region Async WCF Methods

		private void wcfClient_GetTodayWorkEventsOfUserCompleted(object sender, GetTodayWorkEventsOfUserCompletedEventArgs e)
		{
			gridWorkEvents.ItemsSource = e.Result;
		}

		private void wcfClient_GetFullDayTimesCompleted(object sender, GetFullDayTimesCompletedEventArgs e)
		{
			m_TodayWork = e.Result[TimeKey.TodayWork];
			m_TodayRest = e.Result[TimeKey.TodayRest];
			m_WeekRest = e.Result[TimeKey.WeekRest];
			m_MonthRest = e.Result[TimeKey.MonthRest];

			setText();
			startTimer();
		}

		void wcfClient_GetMainWorkAndLastEventCompleted(object sender, GetMainWorkAndLastEventCompletedEventArgs e)
		{
			WorkEvent TodayWorkEvent = e.Result[0];
			WorkEvent LastEvent = e.Result[1];

			if (TodayWorkEvent == null || (TodayWorkEvent.BeginTime != TodayWorkEvent.EndTime))
			{
				State = ControlState.WorkFinished;
				return;
			}

			switch (LastEvent.EventType)
			{
				case WorkEventType.MainWork:
					State = ControlState.WorkGoes;
					break;

				case WorkEventType.TimeOff:
					State = LastEvent.BeginTime == LastEvent.EndTime
								? ControlState.Absent
								: ControlState.WorkGoes;
					break;

				case WorkEventType.LanchTime:
					State = LastEvent.BeginTime == LastEvent.EndTime
								? ControlState.Feeding
								: ControlState.WorkGoes;
					break;

				case WorkEventType.StudyEnglish:
					State = LastEvent.BeginTime == LastEvent.EndTime
								? ControlState.EnglishLesson
								: ControlState.WorkGoes;
					break;
			}
		}

		private void wcfClient_GetNumberOfActiveUsersCompleted(object sender, GetNumberOfActiveUsersCompletedEventArgs e)
		{
			if (e.Result > 2)
				return;

			String strAlert = string.Empty;
			switch (e.Result)
			{
				case 0:
					strAlert += "Вы последний уходите из офиса!";
					break;
				case 1:
					strAlert += "В офисе остался только один человек!";
					break;
				case 2:
					strAlert += "В офисе осталось только двое!";
					break;
			}

			strAlert += String.Format("{0}Уходя, выключите свет и кулер в столовой!",
									  Environment.NewLine);
			HtmlPage.Window.Alert(strAlert);
		}

		// TODO: May be wil be correct return not all event collection, but onle added event to list?
		private void wcfClient_SetUserWorkEventCompleted(object sender, SetUserWorkEventCompletedEventArgs e)
		{
			gridWorkEvents.ItemsSource = e.Result;

			if (!isInitializedDateTime)
				loadFullDayInfo();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Event on datagrid selected text cvhanged.
		/// </summary>
		/// <param name="newSelectedText">New text.</param>
		private void OnGridWorkEventsSelectedTextChanged(string newSelectedText)
		{
			HtmlPage.Window.Eval(String.Format("copyToClipboard('{0}');", newSelectedText));
		}

		/// <summary>
		/// Start timer.
		/// </summary>
		private void startTimer()
		{
			if (m_DispatcherTimer.IsEnabled)
				return;

			m_DispatcherTimer.Interval = m_Timer;
			m_DispatcherTimer.Tick += OnTimerTick;
			m_DispatcherTimer.Start();
		}

		/// <summary>
		/// Set user event information.
		/// </summary>
		/// <param name="isOpenaction">Is open or close action.</param>
		/// <param name="eventType">Event type.</param>
		private void setUserWorkEvent(bool isOpenaction, WorkEventType eventType)
		{
			SLServiceClient wcfClient = new SLServiceClient(new BasicHttpBinding(),
															new EndpointAddress(PathHelper.GetAbsoluteUrl(m_ServiceURI)));

			wcfClient.SetUserWorkEventCompleted += wcfClient_SetUserWorkEventCompleted;
			wcfClient.SetUserWorkEventAsync(m_UserID, isOpenaction, eventType);

			if (eventType == WorkEventType.MainWork && !isOpenaction)
			{
				wcfClient.GetNumberOfActiveUsersCompleted += wcfClient_GetNumberOfActiveUsersCompleted;
				wcfClient.GetNumberOfActiveUsersAsync();
			}
		}

		#region Enable buttons and set localization

		/// <summary>
		/// Set buttons visbility.
		/// </summary>
		private void enableControls()
		{
			setLocalization();
			//btLessonOn.Enabled = Globals.Settings.GlobalSettings.IsEnableBreakButtons;
			//btLessonOff.Enabled = Globals.Settings.GlobalSettings.IsEnableBreakButtons;

			var wcfClient = new SLServiceClient(new BasicHttpBinding(),
			                                    new EndpointAddress(PathHelper.GetAbsoluteUrl(m_ServiceURI)));
			wcfClient.IsLocalHttpRequestCompleted += wcfClient_IsLocalHttpRequestCompleted;
			wcfClient.IsLocalHttpRequestAsync();
		}

		private void wcfClient_IsLocalHttpRequestCompleted(object sender, IsLocalHttpRequestCompletedEventArgs e)
		{
			if (!e.Result)
			{
				btnWork.Visibility =
					btnDinner.Visibility =
					btnLesson.Visibility =
					btnTime.Visibility = System.Windows.Visibility.Collapsed;

				return;
			}

			switch (State)
			{
				case ControlState.WorkGoes:
					btnWork.Visibility = System.Windows.Visibility.Visible;
					btnTime.Visibility = System.Windows.Visibility.Visible;
					btnDinner.Visibility = System.Windows.Visibility.Visible;
					btnLesson.Visibility = System.Windows.Visibility.Visible;
					break;

				case ControlState.WorkFinished:
					btnWork.Visibility = System.Windows.Visibility.Visible;
					btnTime.Visibility = System.Windows.Visibility.Collapsed;
					btnDinner.Visibility = System.Windows.Visibility.Collapsed;
					btnLesson.Visibility = System.Windows.Visibility.Collapsed;
					break;

				case ControlState.Absent:
					btnWork.Visibility = System.Windows.Visibility.Visible;
					btnTime.Visibility = System.Windows.Visibility.Visible;
					btnDinner.Visibility = System.Windows.Visibility.Collapsed;
					btnLesson.Visibility = System.Windows.Visibility.Collapsed;
					break;

				case ControlState.Feeding:
					btnWork.Visibility = System.Windows.Visibility.Visible;
					btnTime.Visibility = System.Windows.Visibility.Collapsed;
					btnDinner.Visibility = System.Windows.Visibility.Visible;
					btnLesson.Visibility = System.Windows.Visibility.Collapsed;
					break;

				case ControlState.EnglishLesson:
					btnWork.Visibility = System.Windows.Visibility.Visible;
					btnTime.Visibility = System.Windows.Visibility.Collapsed;
					btnDinner.Visibility = System.Windows.Visibility.Collapsed;
					btnLesson.Visibility = System.Windows.Visibility.Visible;
					break;
			}
		}

		/// <summary>
		/// Set text to controls.
		/// </summary>
		private void setLocalization()
		{
			if (gridWorkEvents.Columns.Count > 1)
				gridWorkEvents.Columns[1].Header = m_Resource.headerBegin;

			if (gridWorkEvents.Columns.Count > 2)
				gridWorkEvents.Columns[2].Header = m_Resource.headerEnd;

			if (gridWorkEvents.Columns.Count > 3)
				gridWorkEvents.Columns[3].Header = m_Resource.headerDuration;

			if (gridWorkEvents.Columns.Count > 4)
				gridWorkEvents.Columns[4].Header = m_Resource.headerEventType;

			btnWork.Content = State == ControlState.WorkFinished
								  ? m_Resource.btnWorkBegin
								  : m_Resource.btnWorkEnd;

			btnTime.Content = State == ControlState.WorkGoes
								  ? m_Resource.btnTimeOn
								  : m_Resource.btnTimeOff;

			btnLesson.Content = State == ControlState.EnglishLesson
									? m_Resource.btnStudyOff
									: m_Resource.btnStudyOn;

			btnDinner.Content = State == ControlState.Feeding
									? m_Resource.btnDinnerOff
									: m_Resource.btnDinnerOn;
		}

		/// <summary>
		/// Set text to TextBox information.
		/// </summary>
		private void setText()
		{
			var text = new StringBuilder();
			text.AppendFormat("{0} {1}", m_Resource.SLWorkedTime,
							  m_TodayWork.TimeToString());
			text.AppendFormat("{0}{1} {2}", Environment.NewLine,
							  m_Resource.SLMustWorkTime,
							  getTodayRestTime());
			text.AppendFormat("{0}{1} {2}", Environment.NewLine,
							  m_Resource.SLWeekTime,
							  m_WeekRest.TimeToString());
			text.AppendFormat("{0}{1} {2}", Environment.NewLine,
							  m_Resource.SLMonthTime,
							  m_MonthRest.TimeToString());

			tbInfo.Text = text.ToString();
		}

		//TODO: -TodayRest 
		/// <summary>
		/// Get upper floor of working.
		/// </summary>
		/// <returns>String of upper floor time.</returns>
		private string getTodayRestTime()
		{
			StringBuilder text = new StringBuilder();
			text.AppendFormat("{0} ({1} {2})", m_TodayRest.TimeToString(),
							  m_Resource.SLTill,
							  DateTime.Now.Add(m_TodayRest).ToShortTimeString());

			return text.ToString();
		}

		/// <summary>
		/// Load full time information of current day.
		/// </summary>
		private void loadFullDayInfo()
		{
			SLServiceClient wcfClient = new SLServiceClient(new BasicHttpBinding(),
															new EndpointAddress(PathHelper.GetAbsoluteUrl(m_ServiceURI)));

			wcfClient.GetFullDayTimesCompleted += wcfClient_GetFullDayTimesCompleted;
			wcfClient.GetFullDayTimesAsync(m_UserID);
		}

		/// <summary>
		/// Load events for user.
		/// </summary>
		private void loadTodayEvents()
		{
			SLServiceClient wcfClient = new SLServiceClient(new BasicHttpBinding(),
								 new EndpointAddress(PathHelper.GetAbsoluteUrl(m_ServiceURI)));

			wcfClient.GetTodayWorkEventsOfUserCompleted += wcfClient_GetTodayWorkEventsOfUserCompleted;
			wcfClient.GetTodayWorkEventsOfUserAsync(m_UserID);
		}

		#endregion

		#endregion
	}
}

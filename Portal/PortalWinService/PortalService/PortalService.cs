using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.Logger;
using ConfirmIt.PortalLib.Notification;

using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Notification;
using UlterSystems.PortalService.Configuration;
using UlterSystems.PortalService.Properties;

using Core.DB;

namespace UlterSystems.PortalService
{
	/// <summary>
	/// Class of Portal Windows service.
	/// </summary>
	partial class PortalService : ServiceBase
	{
		#region Fields

		private Timer m_MailSendTimer = null;				// Таймер отсылки почтовых сообщений.
		private Timer m_NotRegisteredTimer = null;			// Таймер оповещения неотметившихся в портале.
		private Timer m_CloseEventsTimer = null;			// Таймер, закрывающий незакрытые рабочие интервалы.
		private Timer m_StatisticsDeliveryTimer = null;		// Таймер, отвечающий за рассылки.
		private Timer m_StatisticsTimerChanger = null;
	    private Timer _sheduleGenerationTimer;
	    private Timer _ruleExecutionTimer;
	   
	    private TimeNotification _notification;
		#endregion

		#region Constructors

		/// <summary>
		/// Конструктор.
		/// </summary>
		public PortalService()
		{
			InitializeComponent();
		}

		#endregion

		protected override void OnStart(string[] args)
		{
			Logger.Instance.SplitLogFile = bool.Parse(ConfigurationManager.AppSettings["SplitLogFile"]);
			Logger.Instance.Info(Resources.ServiceStarted);

			// Инициализировать соединение с базой данных.
			ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
			ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
			
            Logger.Instance.Info(Resources.DBConnectionInitialized);

            InitializeNotification();

		    var mailExpiration = ConfigureMailExpiration();

			CreateNRNotificationTimer();
            CreateCENotificationTimer();
            CreateStatisticDeliveryTimer();
			CreateMailSenderTimer(mailExpiration);
		    CreateSheduleGenerationTimer();
		    CreateRuleExecutionTimer();

			Logger.Instance.Info(Resources.TimerCreatedMail);
		}

	    private void CreateRuleExecutionTimer()
	    {
	        _ruleExecutionTimer = new Timer(_notification.ExecuteRules, null, new TimeSpan(0), TimeSpan.FromMinutes(1));
	    }

	    private void CreateSheduleGenerationTimer()
	    {
	        var currentTime = DateTime.Now.TimeOfDay;
            var launchTime = new TimeSpan(0,0,1,0);//we have to get this time from resources

	        TimeSpan dueTime;

	        if (currentTime > launchTime)
	        {
	            dueTime = TimeSpan.FromDays(1) - currentTime + launchTime;
	        }
	        else
	        {
	            dueTime = launchTime - currentTime;
	        }

	       _sheduleGenerationTimer = new Timer(_notification.GenerateShedule,null, dueTime, TimeSpan.FromDays(1));
	    }

	    private void InitializeNotification()
	    { 
            var mailStorage = new DataBaseMailStorage();
            var mailManager = new MailManager(new SmtpSender(Settings.Default.SMTPServer), mailStorage);
            var ruleRepository = new RuleRepository(new GroupRepository());
	        var ruleInstanceRepository = new RuleInstanceRepository(ruleRepository);
	        var compositeRuleFilter = new CompositeRuleFilter(new ActiveTimeFilter(), new ExperationTimeFilter(),
	            new DayOfWeekFilter());

	        var ruleManager = new RuleManager(ruleInstanceRepository , compositeRuleFilter);

	        var insertTimeOffExecutor = new InsertTimeOffRuleExecutor(ruleInstanceRepository);
            var notifyByTimeExecutor = new NotifyByTimeRuleExecutor(new MailProvider(Settings.Default.NRNotificationFromAddress, 
                MailTypes.NRNotification, mailStorage), ruleInstanceRepository);

	        var ruleVisitor = new RuleVisitor(insertTimeOffExecutor, notifyByTimeExecutor, null, null);
            var ruleProcessor = new RuleProcessor(ruleVisitor);

            _notification = new TimeNotification(mailManager, mailStorage, ruleManager, ruleProcessor);
	    }

		private IEnumerable<MailExpire> ConfigureMailExpiration()
		{
			var config = ConfigurationManager.GetSection("MailExpirationSection") as MailExpireConfigSection;
			var mailExpirations = new List<MailExpire>();

			if (config != null)
			{
				foreach (MailExpireItem item in config.Items)
				{
					mailExpirations.Add(new MailExpire
					                    	{
					                    		MailType = item.MailType,
					                    		Name = item.Name,
					                    		TimeExpire = item.ExpirationTimeSpan
					                    	});
				}
			}

			return mailExpirations;
		}

		private void CreateMailSenderTimer(IEnumerable<MailExpire> mailExpiration)
		{
		    try
            {
                // Создать таймер отсылки почтовых сообщений.
                m_MailSendTimer = new Timer(_notification.SendMail, mailExpiration, Settings.Default.MailSendPeriod, Settings.Default.MailSendPeriod);
            }
            catch
            {
                m_MailSendTimer = new Timer(_notification.SendMail, mailExpiration, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            }
        }

        private void CreateStatisticDeliveryTimer()
        {
            try
            {
                // Создать таймер рассылок статистик.
                var sdStartTime = Settings.Default.StatisticsDeliveryStartTime;
                var firstStartStatisticsDelivery = DateClass.GetNextStatisticsDeliveryDate(sdStartTime.Hour, sdStartTime.Minute);

                m_StatisticsDeliveryTimer = new Timer(_notification.DeliverStatistics, null, firstStartStatisticsDelivery - DateTime.Now, TimeSpan.FromMilliseconds(-1));
                m_StatisticsTimerChanger = new Timer(StatisticsTimerChange, null, firstStartStatisticsDelivery.AddMinutes(2) - DateTime.Now, TimeSpan.FromMilliseconds(-1));

                Logger.Instance.Info(Resources.TimerCreatedStat);
            }
            catch (Exception ex)
            {
                CreateAndSaveMail(Settings.Default.StatisticsDeliveryFromAddress,
                                    (int)MailTypes.OfficeStatistics,
                                    Resources.TimerErrorStat + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void CreateCENotificationTimer()
        {
            var now = DateTime.Now;
            var day = new TimeSpan(24, 0, 0);

            try
            {
                // Создать таймер для закрытия незакрытых рабочих интервалов.
                var ceStartTime = Settings.Default.CEStartTime;
                var firstStartCENotification = new DateTime(now.Year, now.Month, now.Day, ceStartTime.Hour, ceStartTime.Minute, ceStartTime.Second);

                if (firstStartCENotification < now)
                    firstStartCENotification += day;

                m_CloseEventsTimer = new Timer(_notification.CloseOpenedWorkEvents, null, firstStartCENotification - now, day);

                Logger.Instance.Info(Resources.TimerCreatedCE);
            }
            catch (Exception ex)
            {
                CreateAndSaveMail(Settings.Default.CENotificationFromAddress,
                                    (int)MailTypes.CENotification,
                                    Resources.TimerErrorCE + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void CreateNRNotificationTimer()
        {
            var now = DateTime.Now;
            var day = new TimeSpan(24, 0, 0);

            try
            {
                // Создать таймер оповещения неотметившихся в портале.
                var nrStartTime = Settings.Default.NRNotificationStartTime;
                var firstStartNRNotification = new DateTime(now.Year, now.Month, now.Day, nrStartTime.Hour, nrStartTime.Minute, nrStartTime.Second);

                if (firstStartNRNotification < now)
                    firstStartNRNotification += day;

                m_NotRegisteredTimer = new Timer(_notification.NotifyNonRegisteredUsers, null, firstStartNRNotification - now, day);

                Logger.Instance.Info(Resources.TimerCreatedNR);
            }
            catch (Exception ex)
            {
                CreateAndSaveMail(Settings.Default.NRNotificationFromAddress,
                                   (int)MailTypes.NRNotification,
                                    Resources.TimerErrorNR + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void CreateAndSaveMail(string fromAddress, int messageType, string body)
        {
            var mailItem = new MailItem
            {
                FromAddress = fromAddress,
                ToAddress = Settings.Default.ErrorToAddress,
                Subject = Settings.Default.ErrorSubject,
                MessageType = messageType,
                IsHTML = false,
                Body = body
            };
            mailItem.Save();
        }

        protected void StatisticsTimerChange(object state)
        {
            Logger.Instance.Info("StatisticsTimerChange started");

            try
            {
                // Изменить таймер.
                m_StatisticsDeliveryTimer.Dispose();

                var now = DateTime.Now;
                var sdStartTime = Settings.Default.StatisticsDeliveryStartTime;
                var nextDeliveryDay = DateClass.GetNextStatisticsDeliveryDate(sdStartTime.Hour, sdStartTime.Minute);

                m_StatisticsDeliveryTimer = new Timer(_notification.DeliverStatistics, null, nextDeliveryDay - now, TimeSpan.FromMilliseconds(-1));
            }
            catch (Exception ex)
            {
                CreateAndSaveMail(Settings.Default.StatisticsDeliveryFromAddress,
                                    (int)MailTypes.OfficeStatistics,
                                    Resources.TimerErrorStat + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        #region ConnectionTypeResolver, OnStop function

        /// <summary>
		/// Процедура привязки соединения к типу сервера.
		/// </summary>
		/// <param name="kind">Тип соединения.</param>
		/// <returns>Тип сервера.</returns>
		protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
		{
			return ConnectionType.SQLServer;
		}

		protected override void OnStop()
		{
			// Уничтожить таймеры.
			if( m_NotRegisteredTimer != null )
				m_NotRegisteredTimer.Dispose();

			if( m_CloseEventsTimer != null )
				m_CloseEventsTimer.Dispose();

			if( m_StatisticsDeliveryTimer != null )
				m_StatisticsDeliveryTimer.Dispose();

			if( m_MailSendTimer != null )
				m_MailSendTimer.Dispose();

			Logger.Instance.Info(Resources.ServiceStoped);
        }

        #endregion
    }
}
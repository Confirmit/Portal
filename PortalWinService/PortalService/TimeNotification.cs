using System;
using System.Collections.Generic;
using UlterSystems.PortalService.Properties;
using UlterSystems.PortalLib.Notification;
using ConfirmIt.PortalLib.Notification;
using Core;
using Logger = ConfirmIt.PortalLib.Logger.Logger;

namespace UlterSystems.PortalService
{
	public class TimeNotification
	{
	    public IMailManager MailManager { get; set; }
        public IMailStorage StorageMail { get; set; }
	    
		/// <summary>
		/// Процедура оповещения незарегистрировавшихся в портале пользователей.
		/// </summary>
		public void NotifyNonRegisteredUsers(object state)
		{
			try
			{
				Logger.Instance.Info(Resources.ProcStartedNR);

                var delivery = new NotificationDelivery
                {
                    SmtpServer = Settings.Default.SMTPServer,
                    FromAddress = Settings.Default.NRNotificationFromAddress,
                    Subject = Resources.NRNotificationSubject,
                    SubjectAdmin = Resources.NRAllNotificationSubjectAdmin,
                    MailRegisterToday = Resources.NRTodayNotificationMessage,
                    MailRegisterYesterday = Resources.NRYesterdayNotificationMessage,
                    MailAdminNotRegisterYesterday = Resources.NRAllNotificationMessageAdmin,
                    MailAdminNotRegistredToday = Resources.NRAllNotificationMessageAdmin,
                    AddresAdmin = Settings.Default.AddressAdminNotification,
                    MinTimeWork = Settings.Default.MinTimeWork
                };

                delivery.StorageMail = StorageMail;
				delivery.DeliverNotification();
			}
			catch (Exception ex)
			{
				Logger.Instance.Error(Resources.ProcErrorNR, ex);
			}
			finally
			{
				Logger.Instance.Info(Resources.ProcFinishedNR);
			}
		}

		/// <summary>
		/// Закрывает открытые рабочие интервалы.
		/// </summary>
		public void CloseOpenedWorkEvents(object state)
		{
			try
			{
				Logger.Instance.Info(Resources.ProcStartedCE);

				var closer = new WorkIntervalsCloser
				{
					SmtpServer = Settings.Default.SMTPServer,
					FromAddress = Settings.Default.CENotificationFromAddress,
					Subject = Resources.CENotificationSubject,
					SubjectAdmin = Resources.CENotificationSubjectAdmin,
					Message = Resources.CENotificationMessage,
					MessageAdmin = Resources.CENotificationMessageAdmin,
				};
				closer.CloseWorkIntervals();
			}
			catch (Exception ex)
			{
				Logger.Instance.Error(Resources.ProcErrorCE, ex);
			}
			finally
			{
				Logger.Instance.Info(Resources.ProcFinishedCE);
			}
		}

		/// <summary>
		/// Рассылает статистики по почте.
		/// </summary>
		public void DeliverStatistics(object state)
		{
			try
			{
				Logger.Instance.Info(Resources.ProcStartedStat);

				var statDeliver = new StatisticsDelivery
				{
					SmtpServer = Settings.Default.SMTPServer,
					FromAddress = Settings.Default.StatisticsDeliveryFromAddress,
					Subject = Resources.StatisticsDeliverySubject,
					SubjectAdmin = Resources.StatisticsDeliverySubjectAdmin
				};
				statDeliver.DeliverStatistics();
			}
			catch (Exception ex)
			{
				Logger.Instance.Error(Resources.ProcErrorStat, ex);
			}
			finally
			{
				Logger.Instance.Info(Resources.ProcFinishedStat);
			}
		}

		/// <summary>
		/// Отправляет отчеты пользователей.
		/// </summary>
		public void SendMail(object state)
		{
			try
			{
				Logger.Instance.Info(Resources.ProcStartedMail);

				var mailExpiration = (IEnumerable<MailExpire>) state;
			    var letters = StorageMail.GetMails(false);
                MailManager.SendMails(mailExpiration, letters);
			}
			catch (Exception ex)
			{
				Logger.Instance.Error(Resources.ProcErrorMail, ex);
			}
			finally
			{
				Logger.Instance.Info(Resources.ProcFinishedMail);
			}
		}
	}
}
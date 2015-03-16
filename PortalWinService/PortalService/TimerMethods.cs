using System;
using System.Collections.Generic;

using ConfirmIt.PortalLib.Logger;
using UlterSystems.PortalService.Properties;
using UlterSystems.PortalLib.Notification;
using ConfirmIt.PortalLib.Notification;

namespace UlterSystems.PortalService
{
	public class TimerMethods
	{
		/// <summary>
		/// Процедура оповещения незарегистрировавшихся в портале пользователей.
		/// </summary>
		public static void NotifyNonRegisteredUsers(object state)
		{
			try
			{
				Logger.Instance.Info(Resources.ProcStartedNR);

                var delivery = new NotificationDelivery
                {
                    SmtpServer = Settings.Default.SMTPServer,
                    FromAddress = Settings.Default.NRNotificationFromAddress,
                    Subject = Resources.NRNotificationSubject,
                    SubjectAdmin = Resources.NRNotificationSubjectAdmin,
                    Message = Resources.NRNotificationMessage,
                    MessageAdmin = Resources.NRNotificationMessageAdmin
                };

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
		public static void CloseOpenedWorkEvents(object state)
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
					MessageAdmin = Resources.CENotificationMessageAdmin
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
		public static void DeliverStatistics(object state)
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
		public static void SendMail(object state)
		{
			try
			{
				Logger.Instance.Info(Resources.ProcStartedMail);

				var mailExpiration = (IEnumerable<MailExpire>) state;

				MailManager.SendMessages(Settings.Default.SMTPServer, mailExpiration, null);
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

	    public static void NotifyNotNoteUsers(object state)
	    {
            try
            {
                Logger.Instance.Info(Resources.ProcStartedNR);

                var delivery = new NotificateionNotNote()
                {
                    SmtpServer = Settings.Default.SMTPServer,
                    FromAddress = Settings.Default.NRNotificationFromAddress,
                    Subject = Resources.NRNotificationSubject,
                    SubjectAdmin = Resources.NRAllNotificationSubjectAdmin,
                    MessageForUser = Resources.NRNotificationMessage,
                    AddresAdmin = Settings.Default.AddressAdminNotification,
                    MessageForAdminForNotRegistredToday = Resources.NRAllNotificationMessageAdmin,
                    MessageForAdminForLittleWorkedYesterday = Resources.LWAllNotificationSubjectAdmin,
                    PieceOfMessageToAdminNotRegisterToday = Resources.NRPieceOfMessageToAdmin,
                    PieceOfMessageToAdminLittleWorkYesterday = Resources.LWPieceOfMessageToAdmin
                };

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
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.Notification;
using ConfirmIt.PortalLib.Properties;
using Core;

namespace UlterSystems.PortalLib.Notification
{
    #region enum MailTypes

    /// <summary>
	/// Types of mail messages.
	/// </summary>
	public enum MailTypes
	{
		/// <summary>
		/// Report from user to PM.
		/// </summary>
		UserReport = 1,

		/// <summary>
		/// User statistics.
		/// </summary>
		UserStatistics = 2,

		/// <summary>
		/// Office statistics.
		/// </summary>
		OfficeStatistics = 3,

		/// <summary>
		/// Notification about absence of registration.
		/// </summary>
		NRNotification = 4,

		/// <summary>
		/// Notification about not closed work intervals.
		/// </summary>
		CENotification = 5,

        /// <summary>
        /// Notification about news.
        /// </summary>
        NewsNotification = 6,
    }

    #endregion

    /// <summary>
	/// Class for mail management.
	/// </summary>
	public class MailManager : IMailManager
    {
        public IMailSender MailSender { get; private set; }
        public IMailStorage MailStorage { get; private set; }

        public MailManager(IMailSender mailSender, IMailStorage mailStorage)
        {
            MailSender = mailSender;
            MailStorage = mailStorage;
        }
        /// <summary>
        /// Sends all messages waiting to be send.
        /// </summary>
        /// <param name="mailExpirations">Period of expiration of messages.</param>
        /// <param name="mails">The messages to be send</param>
        public void SendMails(IEnumerable<MailExpire> mailExpirations, IList<MailItem> mails)
		{
            try
            {
                if (mails == null || mails.Count == 0)
                    return;

                var newsIds = new List<int>();

                foreach (var item in mails)
                {
                	var expirations = mailExpirations.Where(mailExpiration => mailExpiration.MailType == item.MessageType);

                	var expirePeriod = TimeSpan.FromHours(5); // default value
					if (expirations.Count() > 0)
					{
						expirePeriod = expirations.First().TimeExpire;
					}
					else
					{
						Logger.Log.WarnFormat("There are no settings for mail expiration of type: {0}.", item.MessageType);
					}

                	// Delete all expired messages.))
                    if (item.Date < (DateTime.Now - expirePeriod))
                    {
                        item.Delete();
                        continue;
                    }

                    try
                    {
                        // Get mail message.
                        var message = item.GetMailMessage();
                        if (message != null)
                        {
                            // IRC sending
                            if (item.MessageType == (int)MailTypes.NewsNotification) // NewsNotification
                            {
                                var newsId = ParseNewsIDFromSubject(message.Subject);
                                if (newsId != 0 && newsIds.IndexOf(newsId) < 0)
                                {
                                    newsIds.Add(newsId);
                                }
                            }

                            // Send message.
                            MailSender.Send(message);

                            // Mark message as send.
                            item.IsSend = true;
                            MailStorage.SaveMail(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(Resources.MailSendingError, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(Resources.MailSendingError, ex);
            }
		}

        private int ParseNewsIDFromSubject(string subject)
        {
            var strNewsId = subject.Replace(Resources.NewsNotificationSubject, "");
            strNewsId = strNewsId.Replace("(", "").Replace(")", "");

            var newsId = 0;
            return int.TryParse(strNewsId, out newsId) ? newsId : 0;
        }
        
    }
}
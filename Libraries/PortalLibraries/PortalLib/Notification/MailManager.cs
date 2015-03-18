using System;
using System.Linq;
using System.Net.Mail;
using System.Collections.Generic;

using Core;
using Core.ORM.Attributes;

using ConfirmIt.PortalLib.Notification;
using ConfirmIt.PortalLib.Properties;

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
	public class MailManager
	{
		#region Methods
		
        /// <summary>
		/// Sends all messages waiting to be send.
		/// </summary>
		/// <param name="smtpServer">Address of SMTP server.</param>
		/// <param name="mailExpiration">Period of expiration of messages.</param>
		/// <param name="adminEMail">Addresses of administrators to get notifications.</param>
		public static void SendMessages(string smtpServer, IEnumerable<MailExpire> mailExpirations, string adminEMail)
		{
			if (string.IsNullOrEmpty(smtpServer))
				throw new ArgumentNullException("smtpServer", Resources.SMTPServerIsNotSet);

            try
            {
                var ircConnection = new IRCConnection();
                var IRCIsUsed = false;

            	var coll = (BaseObjectCollection<MailItem>)
            	           BasePlainObject.GetObjects(typeof (MailItem),
            	                                      "IsSend", (object) false);

                if (coll == null || coll.Count == 0)
                    return;

                // Create SMTP client.
                var smtpClient = new SmtpClient(smtpServer);
                //NetworkCredential Credentials = new NetworkCredential("oktober21", "21108816");
                //smtpClient.Credentials = Credentials;

                var newsIds = new List<int>();

                foreach (var item in coll)
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
                                var newsId = parseNewsIDFromSubject(message.Subject);
                                if (newsId != 0 && newsIds.IndexOf(newsId) < 0)
                                {
                                    ircConnection.SendMessage(message.Body);
                                    newsIds.Add(newsId);
                                }
                                IRCIsUsed = true;
                            }

                            // Send message.
                            smtpClient.Send(message);

                            // Mark message as send.
                            item.IsSend = true;
                            item.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(Resources.MailSendingError, ex);
                    }
                }

                if (IRCIsUsed)
                    ircConnection.Disconnect();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(Resources.MailSendingError, ex);
            }
		}

        private static int parseNewsIDFromSubject(string subject)
        {
            var strNewsId = subject.Replace(Resources.NewsNotificationSubject, "");
            strNewsId = strNewsId.Replace("(", "").Replace(")", "");

            var newsId = 0;
            return int.TryParse(strNewsId, out newsId) ? newsId : 0;
        }

        #endregion
    }
}
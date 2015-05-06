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

    #region class MailManager

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
                            if (item.MessageType == (int)MailTypes.NewsNotification) // NewsNotification
                            {
                                var newsId = parseNewsIDFromSubject(message.Subject);
                                if (newsId != 0 && newsIds.IndexOf(newsId) < 0)
                                {
                                    newsIds.Add(newsId);
                                }
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

    #endregion

    #region class MailItem : BasePlainObject

    /// <summary>
	/// Class of single mail to send.
	/// </summary>
	[DBTable("MailsStorage")]
	public class MailItem : BasePlainObject
	{
		#region Fields

		private DateTime m_DateTime = DateTime.Now;
		private bool m_IsSend = false;
		private string m_FromAddress;
		private string m_ToAddresses;
		private string m_Subject;
		private string m_Body;
		private bool m_IsHTML = false;
		private int m_MessageType = -1;
		
        #endregion

		#region Properties

		/// <summary>
		/// Date of queueing of message.
		/// </summary>
		[DBRead("Date")]
		public DateTime Date
		{
			get { return m_DateTime; }
			set { m_DateTime = value; }
		}

		/// <summary>
		/// Was the message send.
		/// </summary>
		[DBRead("IsSend")]
		public bool IsSend
		{
			get { return m_IsSend; }
			set { m_IsSend = value; }
		}

		/// <summary>
		/// Sender address.
		/// </summary>
		[DBRead("FromAddress")]
		public string FromAddress
		{
			get { return m_FromAddress; }
			set { m_FromAddress = value; }
		}

		/// <summary>
		/// Comma separated addresses of recipients.
		/// </summary>
		[DBRead("ToAddress")]
		public string ToAddress
		{
			get { return m_ToAddresses; }
			set { m_ToAddresses = value; }
		}

		/// <summary>
		/// Subject of message.
		/// </summary>
		[DBRead("Subject")]
		public string Subject
		{
			get { return m_Subject; }
			set { m_Subject = value; }
		}

		/// <summary>
		/// Text of message.
		/// </summary>
		[DBRead("Body")]
		public string Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}

		/// <summary>
		/// Has the message HTML format.
		/// </summary>
		[DBRead("IsHTML")]
		public bool IsHTML
		{
			get { return m_IsHTML; }
			set { m_IsHTML = value; }
		}

		/// <summary>
		/// Type of message.
		/// </summary>
		[DBRead("MessageType")]
		public int MessageType
		{
			get { return m_MessageType; }
			set { m_MessageType = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns mail message for this object.
		/// </summary>
		/// <returns>Mail message for this object.</returns>
		public MailMessage GetMailMessage()
		{
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(FromAddress),
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = IsHTML
                };
                message.To.Add(ToAddress);

                return message;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(Resources.MailComposingError, ex);
                return null;
            }
		}

		#endregion
    }

    #endregion
}
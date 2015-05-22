using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using ConfirmIt.PortalLib.Properties;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.Notification
{
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
                Core.Logger.Log.Error(Resources.MailComposingError, ex);
                return null;
            }
        }

        #endregion
    }
}

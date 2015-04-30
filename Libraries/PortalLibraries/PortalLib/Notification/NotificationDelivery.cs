using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.Logger;
using ConfirmIt.PortalLib.Notification;

namespace UlterSystems.PortalLib.Notification
{
    /// <summary>
    /// ����� �������� ����������� �� ���������� �������� ���������.
    /// </summary>
    public class NotificationDelivery
    {
        private List<Person> _personsNotRegisterToday = new List<Person>();
        private List<Person> _personsNotRegisterYesterday = new List<Person>();
        
        #region Properties
        public IMailStorage MailStorage { get; private set; }
        public IUsersProvider ProviderUsers { get; private set; }
        public INotificationController ControllerNotification { get; private set; }
        public IWorkEventProvider ProviderWorkEvent { get; private set; }


        /// <summary>
        /// ����������� ����� ������ ���������� � �����, ����� ����� ������� ��� ��������������
        /// </summary>
        public TimeSpan MinTimeWork { get; set; }
        /// <summary>
        /// ����������� �������� ����� ������ ��������������
        /// </summary>
        public string CompleteBodyMailToAdmin { get; private set; }

        /// <summary>
        /// ����� SMTP-�������.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// �������� �����.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// ���� ������ ��� �������� ���������� ������������.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// ���� ������ ��� �������� ���������� �����.
        /// </summary>
        public string SubjectAdmin { get; set; }

        /// <summary>
        /// ����� ������ �� �� ������� �������.
        /// </summary>
        public string MailRegisterToday { get; set; }

        /// <summary>
        /// ����� ������ �� �� ������� �����.
        /// </summary>
        public string MailRegisterYesterday { get; set; }

        /// <summary>
        /// ����� ������ ��� ��������������.
        /// </summary>
        public string MailAdminNotRegisterYesterday { get; set; }

        /// <summary>
        /// ����������� ��� ��� ����� ������, � ������� ��������� ������ �� ������������ �������
        /// </summary>
        public string MailAdminNotRegistredToday { get; set; }

        /// <summary>
        /// ������ ��������������, ��� ����������� ������ �� ������������
        /// </summary>
        public string AddresAdmin { get; set; }

        #endregion


        public NotificationDelivery(IUsersProvider providerUsers, INotificationController controllerNotification, IWorkEventProvider providerWorkEvent, IMailStorage mailStorage)
        {
            ProviderUsers = providerUsers;
            ControllerNotification = controllerNotification;
            ProviderWorkEvent = providerWorkEvent;
            MailStorage = mailStorage;
        }

        /// <summary>
        /// ������� ����, ��� �� ��������� ����� ��� �������
        /// </summary>
        private void BuildNotRegistedUsersTodayOrYesterday()
        {
            // �� ��������� �� ����������.
            if (!ControllerNotification.IsNotified(DateTime.Now))
                return;

            // �������� ������ ���� �������������.
            var users = ProviderUsers.GetAllEmployees();
            if (users == null || users.Count == 0)
                return;

            foreach (Person person in users)
            {
                try
                {
                    if(!ControllerNotification.IsNotified(person)) continue;

                    WorkEvent lastEventToday = ProviderWorkEvent.GetCurrentEventOfDate(person, DateTime.Today);
                    WorkEvent lastEventYesterday = ProviderWorkEvent.GetMainWorkEvent(person, DateTime.Today.AddDays(-1));

                    if (lastEventToday == null)
                        _personsNotRegisterToday.Add(person);
                    if (lastEventYesterday == null || lastEventYesterday.Duration < MinTimeWork)
                        _personsNotRegisterYesterday.Add(person);

                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(
                        "��� ��������� ���������� � ������������ " + person.FullName + " ��������� ������.", ex);
                }
            }
        }

        /// <summary>
        /// ��������� ����������� �� ���������� ������� ����������.
        /// </summary>
        public void DeliverNotification()
        {
            BuildNotRegistedUsersTodayOrYesterday();
            CreateAndSaveMailNotRegisterToday();
            CreateAndSaveMailNotRegisterYesterday();
            CreateAndSaveBodyMailToAdmin();
        }

        /// <summary>
        /// ���������� ��������� �������������� � ��� ��� �� ��������� �����
        /// </summary>
        /// <returns></returns>
        private string GetBodyMailToAdminYesterday()
        {
            if (_personsNotRegisterYesterday.Count == 0) return string.Empty;
            var mailToAdminYesterday = Regex.Replace(MailAdminNotRegisterYesterday, "_Date_",
                DateTime.Today.AddDays(-1).ToLongDateString());

            var mailAdmin = new StringBuilder(mailToAdminYesterday);
            mailAdmin.AppendLine();
            for (int i = 0; i < _personsNotRegisterYesterday.Count; i++)
            {
                var line = string.Format("{0}) FullName: {1}, ID: {2}", i + 1, _personsNotRegisterYesterday[i].FullName,
                    _personsNotRegisterYesterday[i].ID);
                mailAdmin.AppendLine(line);
            }
            mailAdmin.AppendLine();
            return mailAdmin.ToString();
        }

        /// <summary>
        /// ���������� ��������� �������������� � ��� ��� �� ��������� �������
        /// </summary>
        /// <returns></returns>
        private string GetBodyMailToAdminToday()
        {
            if (_personsNotRegisterToday.Count == 0) return string.Empty;
            var mailToAdminToday = Regex.Replace(MailAdminNotRegistredToday, "_Date_", DateTime.Today.ToLongDateString());
            var mailAdmin = new StringBuilder(mailToAdminToday);
            mailAdmin.AppendLine();
            for (int i = 0; i < +_personsNotRegisterToday.Count; i++)
            {
                var line = string.Format("{0}) FullName: {1}, ID: {2}", i + 1, _personsNotRegisterToday[i].FullName,
                    _personsNotRegisterToday[i].ID);
                mailAdmin.AppendLine(line);
            }
            mailAdmin.AppendLine();
            return mailAdmin.ToString();
        }

        /// <summary>
        /// ������ � ��������� ��������� ������ ��� ���� ��� �� ��������� �����
        /// </summary>
        private void CreateAndSaveBodyMailToAdmin()
        {
            CompleteBodyMailToAdmin = GetBodyMailToAdminYesterday() + GetBodyMailToAdminToday();
            if (string.IsNullOrEmpty(CompleteBodyMailToAdmin)) return;

            Logger.Instance.Info("Notice sending to administrator E-Mail " + AddresAdmin + ".");
            SaveMailItem(AddresAdmin, CompleteBodyMailToAdmin, SubjectAdmin);
        }

        /// <summary>
        /// ������ � ��������� ��������� ��� ������� ��� �� ��������� �����
        /// </summary>
        private void CreateAndSaveMailNotRegisterYesterday()
        {
            foreach (var person in _personsNotRegisterYesterday)
            {
                Logger.Instance.Info("Notice sending to " + person.FullName + ".");
                string bodyMail = GetMailAfterChanging(MailRegisterYesterday, person, DateTime.Today.AddDays(-1));

                SaveMailItem(person.PrimaryEMail, bodyMail, Subject);
            }
        }


        /// <summary>
        /// ������ � ��������� ��������� ��� ������� ��� �� ��������� �������
        /// </summary>
        private void CreateAndSaveMailNotRegisterToday()
        {
            foreach (var person in _personsNotRegisterToday)
            {
                Logger.Instance.Info("Notice sending to " + person.FullName + ".");
                string bodyMail = GetMailAfterChanging(MailRegisterToday, person, DateTime.Today);

                SaveMailItem(person.PrimaryEMail, bodyMail, Subject);
            }
        }

        private string GetMailAfterChanging(string text, Person person, DateTime date)
        {
            text = Regex.Replace(text, "_UserName_", person.FullName);
            text = Regex.Replace(text, "_Date_", date.ToLongDateString());
            return text;
        }

        /// <summary>
        /// ��������� ���������
        /// </summary>
        /// <param name="toAddress">������ ����������</param>
        /// <param name="bodyMail">���� ���������</param>
        /// <param name="subject">���� ���������</param>
        private void SaveMailItem(string toAddress, string bodyMail, string subject)
        {
            var item = new MailItem
            {
                FromAddress = this.FromAddress,
                ToAddress = toAddress,
                Subject = subject,
                Body = bodyMail,
                MessageType = ((int)MailTypes.NRNotification)
            };
            MailStorage.SaveMail(item);
        }

        
       
    }
}






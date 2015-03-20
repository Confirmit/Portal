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
        public IStorageMail StorageMail { get; set; }

        /// <summary>
        /// ����������� ����� ������ ���������� � �����, ����� ����� ������� ��� ��������������
        /// </summary>
        public TimeSpan MinTimeWork { get; set; }
        /// <summary>
        /// ����������� ������ ��������������
        /// </summary>
        public string CompleteLetterToAdmin { get; private set; }

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
        public string MessageRegisterToday { get; set; }

        /// <summary>
        /// ����� ������ �� �� ������� �����.
        /// </summary>
        public string MessageRegisterYesterday { get; set; }

        /// <summary>
        /// ����� ������ ��� ��������������.
        /// </summary>
        public string MessageAdminNotRegisterYesterday { get; set; }

        /// <summary>
        /// ����������� ��� ��� ����� ������, � ������� ��������� ������ �� ������������ �������
        /// </summary>
        public string MessageAdminNotRegistredToday { get; set; }


        /// <summary>
        /// ������ ��������������, ��� ����������� ������ �� ������������
        /// </summary>
        public string AddresAdmin { get; set; }

        #endregion

        /// <summary>
        /// ������� ����, ��� �� ��������� ����� ��� �������
        /// </summary>
        private void BuildNotRegistedUsersTodayOrYesterday()
        {
            // �� ��������� �� ����������.
            if (CalendarItem.GetHoliday(DateTime.Now))
                return;

            // �������� ������ ���� �������������.
            Person[] users = UserList.GetEmployeeList();
            if (users == null || users.Length == 0)
                return;

            foreach (Person person in users)
            {
                try
                {
                    // �� ��������� �� ��������.
                    // �� ��������� ��������, �� ������� ������ ����������� �����.
                    // �� ��������� ���������� ��������.
                    if (!person.IsInRole("Employee")
                        || string.IsNullOrEmpty(person.PrimaryEMail)
                        || person.EmployeesUlterSYSMoscow)
                        continue;

                    
                    // �������� ��������� ������� �� �������.
                    WorkEvent lastEventToday = WorkEvent.GetCurrentEventOfDate(person.ID.Value, DateTime.Today);
                    WorkEvent lastEventYesterday = WorkEvent.GetMainWorkEvent(person.ID.Value,
                        DateTime.Today.AddDays(-1));

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
            CreateAndSaveMessagesNotRegisterToday();
            CreateAndSaveMessageNotRegisterYesterday();
            CreateAndSaveMessageToAdmin();
        }

        /// <summary>
        /// ���������� ��������� �������������� � ��� ��� �� ��������� �����
        /// </summary>
        /// <returns></returns>
        private string GetMessageToAdminYesterday()
        {
            if (_personsNotRegisterYesterday.Count == 0) return string.Empty;
            var messageToAdminYesterday = Regex.Replace(MessageAdminNotRegisterYesterday, "_Date_",
                DateTime.Today.AddDays(-1).ToLongDateString());

            var messageAdmin = new StringBuilder(messageToAdminYesterday);
            messageAdmin.AppendLine();

            for (int i = 0; i < _personsNotRegisterYesterday.Count; i++)
            {
                var line = string.Format("{0}) FullName: {1}, ID: {2}", i + 1, _personsNotRegisterYesterday[i].FullName,
                    _personsNotRegisterYesterday[i].ID);
                messageAdmin.AppendLine(line);
            }
            messageAdmin.AppendLine();
            return messageAdmin.ToString();
        }

        /// <summary>
        /// ���������� ��������� �������������� � ��� ��� �� ��������� �������
        /// </summary>
        /// <returns></returns>
        private string GetMessageToAdminToday()
        {
            if (_personsNotRegisterToday.Count == 0) return string.Empty;
            var messageToAdminToday = Regex.Replace(MessageAdminNotRegistredToday, "_Date", DateTime.Today.ToLongDateString());
            var messageAdmin = new StringBuilder(messageToAdminToday);
            for (int i = 0; i < +_personsNotRegisterToday.Count; i++)
            {
                var line = string.Format("{0}) FullName: {1}, ID: {2}", i + 1, _personsNotRegisterToday[i].FullName,
                    _personsNotRegisterToday[i].ID);
                messageAdmin.AppendLine(line);
            }
            messageAdmin.AppendLine();
            return messageAdmin.ToString();
        }

        /// <summary>
        /// ������ � ��������� ��������� ������ ��� ���� ��� �� ��������� �����
        /// </summary>
        private void CreateAndSaveMessageToAdmin()
        {
            CompleteLetterToAdmin = GetMessageToAdminYesterday() + GetMessageToAdminToday();
            if (string.IsNullOrEmpty(CompleteLetterToAdmin)) return;

            Logger.Instance.Info("Notice sending to administrator E-Mail " + AddresAdmin + ".");
            SaveMailItem(AddresAdmin, CompleteLetterToAdmin, SubjectAdmin);
        }

        /// <summary>
        /// ������ � ��������� ��������� ��� ������� ��� �� ��������� �����
        /// </summary>
        private void CreateAndSaveMessageNotRegisterYesterday()
        {
            foreach (var person in _personsNotRegisterYesterday)
            {
                Logger.Instance.Info("Notice sending to " + person.FullName + ".");
                string message = GetMessageAfterChanging(MessageRegisterYesterday, person, DateTime.Today.AddDays(-1));

                SaveMailItem(person.PrimaryEMail, message, Subject);
            }
        }


        /// <summary>
        /// ������ � ��������� ��������� ��� ������� ��� �� ��������� �������
        /// </summary>
        private void CreateAndSaveMessagesNotRegisterToday()
        {
            foreach (var person in _personsNotRegisterToday)
            {
                Logger.Instance.Info("Notice sending to " + person.FullName + ".");
                string message = GetMessageAfterChanging(MessageRegisterToday, person, DateTime.Today);

                SaveMailItem(person.PrimaryEMail, message, Subject);
            }
        }

        /// <summary>
        /// ��������� ���������
        /// </summary>
        /// <param name="toAddress">������ ����������</param>
        /// <param name="message">���� ���������</param>
        /// <param name="subject">���� ���������</param>
        private void SaveMailItem(string toAddress, string message, string subject)
        {
            var item = new MailItem
            {
                FromAddress = this.FromAddress,
                ToAddress = toAddress,
                Subject = subject,
                Body = message,
                MessageType = ((int)MailTypes.NRNotification)
            };
            StorageMail.SaveMail(item);
        }

        
        private string GetMessageAfterChanging(string message, Person person, DateTime date)
        {
            message = Regex.Replace(message, "_UserName_", person.FullName);
            message = Regex.Replace(message, "_Date_", date.ToLongDateString());
            return message;
        }
    }
}






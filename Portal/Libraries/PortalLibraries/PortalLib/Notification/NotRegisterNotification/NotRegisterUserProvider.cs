using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification.Interfaces;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification.NotRegisterNotification
{
    public class NotRegisterUserProvider : INotRegisterUserProvider
    {
        public List<Person> GetNotRegisterUsers(DateTime datetime)
        {
            var users = UserList.GetEmployeeList();
            var result = new List<Person>();

            foreach (Person person in users)
            {
                try
                {
                    WorkEvent userEvent = WorkEvent.GetCurrentEventOfDate(person.ID.Value, datetime);
                    if (userEvent == null) result.Add(person);
                }
                catch (Exception ex)
                {
                    Logger.Logger.Instance.Error(
                        "При обработке информации о пользователе " + person.FullName + " произошла ошибка.", ex);
                }
            }

            return result;
        }

        public List<Person> GetUsersWithShortMainWork(DateTime datetime, TimeSpan minDurationMainWork)
        {
            var users = UserList.GetEmployeeList();
            var result = new List<Person>();

            foreach (Person person in users)
            {
                try
                {
                    WorkEvent userEvent = WorkEvent.GetMainWorkEvent(person.ID.Value, datetime);
                    if (userEvent == null || userEvent.Duration < minDurationMainWork)
                        result.Add(person);
                }
                catch (Exception ex)
                {
                    Logger.Logger.Instance.Error(
                        "При обработке информации о пользователе " + person.FullName + " произошла ошибка.", ex);
                }
            }

            return result;
        }
    }
}
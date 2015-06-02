using System;
using System.Collections.Generic;
using System.ComponentModel;
using ConfirmIt.PortalLib.Notification.Interfaces;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.TestClasses
{
    public class TestNotRegisteredUserProvider : INotRegisterUserProvider
    {
        public int[] NRTodayUserIds { get; set; }
        public int[] NRYesterdayUserIds { get; set; }

        public TestNotRegisteredUserProvider(int numberOfUsersNRToday,int numberOfUsersNRYesterday)
        {
            NRTodayUserIds = new int[numberOfUsersNRToday];
            NRYesterdayUserIds = new int[numberOfUsersNRYesterday];

            for (int i = 0; i < NRTodayUserIds.Length; i++)
            {
                NRTodayUserIds[i] = i;
            }

            for (int i = 0; i < NRYesterdayUserIds.Length; i++)
            {
                NRYesterdayUserIds[i] = i;
            }
        }

        public List<Person> GetNotRegisterUsers(DateTime datetime)
        {
            return GetUsers(NRTodayUserIds);
        }

        public List<Person> GetUsersWithShortMainWork(DateTime datetime, TimeSpan minDurationMainWork)
        {
            return GetUsers(NRYesterdayUserIds);
        }

        private List<Person> GetUsers(int[] userIds)
        {
            var users = new List<Person>();
            foreach (var id in userIds)
            {
                var user = new Person
                {
                    ID = id,
                    FirstName = new MLText("en", id.ToString()),
                    PrimaryEMail = id.ToString()
                };
                users.Add(user);
            }
            return users;
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using ConfirmIt.PortalLib.BAL;
using System.Globalization;
using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.Configuration;
using ConfirmIt.PortalLib.DAL;

namespace Confirmit.Portal.PortalLib.Test.BALTest
{
    /// <summary>
    /// Test class for CalendarItem class.
    /// </summary>
    [TestClass]
    [ClearMocks]
    [VerifyMocks]
    public class CalendarItemTest
    {
        [TestInitialize()]
        public void TestInitialize() 
        {
            MockManager.Init();

            CalendarSection calenderSection = new CalendarSection();
            calenderSection.CacheEnabled = false;

            WorkTimeSection workTimeSection = new WorkTimeSection();
            workTimeSection.DefaultWorkTime = new TimeSpan(8, 30, 00);

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(Globals.Settings.Calendar, calenderSection).RepeatAlways();
                recorder.ExpectAndReturn(Globals.Settings.WorkTime, workTimeSection).RepeatAlways();
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CalendarItem_GetCalendarItem_NotInCache_NotInDatabase_WorkDay()
        {
            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(CalendarItem.Cache.ContainsKey(null), false);
                SiteProvider.Calendar.GetCalendarDetails(DateTime.Now);
                recorder.Return(null);

                CalendarItem.Cache[null] = null;
            }

            CalendarItem item = CalendarItem.GetCalendarItem(new DateTime(2008, 10, 6));
            Assert.IsNotNull(item);
            Assert.AreEqual(Globals.Settings.WorkTime.DefaultWorkTime, item.WorkTime);
            Assert.IsFalse(item.IsHoliday);
            Assert.IsFalse(item.IsSaved);
        }

        [TestMethod]
        public void CalendarItem_GetCalendarItem_NotInCache_NotInDatabase_WeekEnd()
        {
            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(CalendarItem.Cache.ContainsKey(null), false);
                SiteProvider.Calendar.GetCalendarDetails(DateTime.Now);
                recorder.Return(null);

                CalendarItem.Cache[null] = null;
            }

            CalendarItem item = CalendarItem.GetCalendarItem(new DateTime(2008, 10, 5));
            Assert.IsNotNull(item);
            Assert.AreEqual(TimeSpan.Zero, item.WorkTime);
            Assert.IsTrue(item.IsHoliday);
            Assert.IsFalse(item.IsSaved);
        }

        [TestMethod]
        public void CalendarItem_GetCalendarItem_InCache()
        {
            CalendarItem cachedItem = new CalendarItem();

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(CalendarItem.Cache.ContainsKey(null), true);
                recorder.ExpectAndReturn(CalendarItem.Cache[null], cachedItem);
            }

            CalendarItem item = CalendarItem.GetCalendarItem(new DateTime(2008, 10, 6));
            Assert.AreSame(cachedItem, item);
        }
        [TestMethod]
        public void CalendarItem_GetCalendarItem_NotInCache_InDatabase()
        {
            CalendarDetails details = new CalendarDetails();
            details.ID = 1;
            details.Date = new DateTime(2008, 10, 6);
            details.WorkTime = new DateTime(2008, 10, 6, 8, 30, 0);

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(CalendarItem.Cache.ContainsKey(null), false);
                SiteProvider.Calendar.GetCalendarDetails(DateTime.Now);
                recorder.Return(details);

                CalendarItem.Cache[null] = null;
            }

            CalendarItem item = CalendarItem.GetCalendarItem(new DateTime(2008, 10, 6));
            Assert.IsNotNull(item);
            Assert.AreEqual(new TimeSpan(8, 30, 0), item.WorkTime);
            Assert.AreEqual(new DateTime(2008, 10, 6), item.Date);
            Assert.IsTrue(item.IsSaved);
        }
    }
}

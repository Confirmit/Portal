using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using TypeMock;
using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.DB;

namespace PortalTest.PortalLibTest.CalendarTest
{
/*	[TestFixture]
	[ClearMocks]
	public class CalendarTest
	{
		[Test]
		public void GetCalendarDate_FromCache()
		{
			CalendarDate cDate = new CalendarDate();
			cDate.Date = DateTime.Now;
			cDate.WorkRate = TimeSpan.FromHours(6);

			using (RecordExpectations recorder = new RecordExpectations())
			{
				Cache.Contains(null);
				recorder.Return(true);

				Cache.InsertDate(null);
				recorder.Return(DateTime.Now);

				Cache.GetObject(null);
				recorder.Return(cDate);

				Cache.Add(null, null);
			}

			ICalendarDate icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(DateTime.Now);
			Assert.AreSame(cDate, icDate);
			Assert.AreEqual(DateTime.Today, icDate.Date);
			Assert.AreEqual(TimeSpan.FromHours(6), icDate.WorkRate);
			Assert.IsFalse(icDate.IsHoliday);
		}

		[Test]
		public void GetCalendarDate_NotFromCache()
		{
			using (RecordExpectations recorder = new RecordExpectations())
			{
				Cache.Contains(null);
				recorder.Return(false);

				DBManager.GetCalendarDate(DateTime.Today);
				recorder.Return(null);

				Cache.Add(null, null);
			}

			ICalendarDate icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 12));
			Assert.AreEqual(new DateTime(2007, 9, 12), icDate.Date);
			Assert.AreEqual(TimeSpan.FromHours(8), icDate.WorkRate);
			Assert.IsFalse(icDate.IsHoliday);
		}

		[Test]
		public void GetCalendarDate_FromCacheExpire()
		{
			using (RecordExpectations recorder = new RecordExpectations())
			{
				Cache.Contains(null);
				recorder.Return(true);

				Cache.InsertDate(null);
				recorder.Return(DateTime.Now.AddDays(-356));

				Cache.Remove(null);
				recorder.Return(true);

				DBManager.GetCalendarDate(DateTime.Today);
				recorder.Return(null);

				Cache.Add(null, null);
			}

			ICalendarDate icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 12));
			Assert.AreEqual(new DateTime(2007, 9, 12), icDate.Date);
			Assert.AreEqual(TimeSpan.FromHours(8), icDate.WorkRate);
			Assert.IsFalse(icDate.IsHoliday);
		}

		[Test]
		public void IsHoliday_False()
		{
			CalendarDate cDate = new CalendarDate();
			cDate.WorkRate = TimeSpan.FromHours(8);

			using (RecordExpectations recorder = new RecordExpectations())
			{
				UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(DateTime.Now);
				recorder.Return(cDate);
			}

			Assert.IsFalse(UlterSystems.PortalLib.BusinessObjects.Calendar.IsHoliday(DateTime.Now));
		}

		[Test]
		public void IsHoliday_True()
		{
			CalendarDate cDate = new CalendarDate();
			cDate.WorkRate = TimeSpan.Zero;

			using (RecordExpectations recorder = new RecordExpectations())
			{
				UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(DateTime.Now);
				recorder.Return(cDate);
			}

			Assert.IsTrue(UlterSystems.PortalLib.BusinessObjects.Calendar.IsHoliday(DateTime.Now));
		}

		[Test]
		public void GetWorkRate()
		{
			CalendarDate cDate = new CalendarDate();
			cDate.WorkRate = TimeSpan.FromHours(3);

			using (RecordExpectations recorder = new RecordExpectations())
			{
				UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(DateTime.Now);
				recorder.Return(cDate);
			}

			Assert.AreEqual(TimeSpan.FromHours(3), UlterSystems.PortalLib.BusinessObjects.Calendar.GetWorkRate(DateTime.Now));
		}
	}*/
}

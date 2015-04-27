using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using UlterSystems.PortalLib.BusinessObjects;
using Core;
using System.Configuration;

namespace PortalTest.PortalLibTest.CalendarTest
{
	[TestFixture]
	[Category("Calendar")]
	public class CalendarDateTest
	{
		private CalendarDate m_cDate;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			Utils.InitDBConnection();
		}

		[SetUp]
		public void SetUp()
		{
			m_cDate = new CalendarDate();
		}

		[Test]
		public void Constructor()
		{
			m_cDate = new CalendarDate();
			Assert.IsNotNull(m_cDate);
		}

		[Test]
		public void Date()
		{
			DateTime now = DateTime.Now;

			m_cDate.Date = now;
			Assert.AreEqual(now.Date, m_cDate.Date);
		}

		[Test]
		public void IsHoliday()
		{
			Assert.IsTrue(m_cDate.IsHoliday);

			m_cDate.Date = new DateTime(2007, 9, 8); // Saturday
			m_cDate.WorkRate = TimeSpan.FromHours(8);
			Assert.IsFalse(m_cDate.IsHoliday);

			m_cDate.Date = new DateTime(2007, 9, 9); // Sunday
			Assert.IsFalse(m_cDate.IsHoliday);

			m_cDate.WorkRate = TimeSpan.Zero;
			Assert.IsTrue(m_cDate.IsHoliday);
		}

		[Test]
		public void WorkTime()
		{
			m_cDate.WorkTime = new DateTime(2007, 9, 10, 5, 0, 0);
			Assert.AreEqual(TimeSpan.FromHours(5), m_cDate.WorkRate);

			m_cDate.WorkTime = new DateTime(2007, 9, 10, 0, 0, 30);
			Assert.AreEqual(TimeSpan.Zero, m_cDate.WorkRate);
		}

		//[Test]
		public void MyTest()
		{
			ICalendarDate icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 25));
			Assert.IsFalse(icDate.IsHoliday);
			icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 26));
			Assert.IsTrue(icDate.IsHoliday);
			icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 27));
			Assert.IsTrue(icDate.IsHoliday);
			icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 28));
			Assert.IsTrue(icDate.IsHoliday);
			icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 29));
			Assert.IsTrue(icDate.IsHoliday);
			icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 9, 30));
			Assert.IsTrue(icDate.IsHoliday);
			icDate = UlterSystems.PortalLib.BusinessObjects.Calendar.GetCalendarDate(new DateTime(2007, 10, 1));
			Assert.IsFalse(icDate.IsHoliday);
		}
	}
}

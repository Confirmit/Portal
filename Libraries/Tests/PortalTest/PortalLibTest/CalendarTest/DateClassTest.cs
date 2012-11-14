using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using TypeMock;
using UlterSystems.PortalLib.BusinessObjects;
using System.Diagnostics;

namespace PortalTest.PortalLibTest.CalendarTest
{
	[TestFixture]
	public class DateClassTest
	{
		[Test]
		public void GetNextStatisticsDeliveryDate_BeforeTime()
		{
			int deliveryHour = 12;
			int deliveryMinute = 0;

			int randomYear = ( new Random().Next( 100 ) ) + 2000;
			Console.WriteLine( "Testing " + randomYear.ToString() + " year." );

			DateTime date = new DateTime( randomYear, 1, 1, 11, 0, 0 );
			while( date.Year == randomYear )
			{
				MockManager.Init();

				Mock dateClassMock = MockManager.Mock( typeof( DateClass ) );
				dateClassMock.ExpectAndReturn( "GetCurrentTime", date );

				DateTime deliveryDate = DateClass.GetNextStatisticsDeliveryDate( deliveryHour, deliveryMinute );
				Console.WriteLine( String.Format( "{0} - {1}", date.ToString( "d MMMM yyyy (dddd)" ), deliveryDate.ToString( "d MMMM yyyy (dddd) H:mm" ) ) );

				Assert.AreEqual( deliveryHour, deliveryDate.Hour );
				Assert.AreEqual( deliveryMinute, deliveryDate.Minute );

				Assert.Greater( deliveryDate, date );
				Assert.IsTrue( ( deliveryDate.DayOfWeek == DayOfWeek.Monday ) || ( deliveryDate.Day == 1 ) );

				Assert.IsTrue( ( (TimeSpan) ( deliveryDate - date ) ).TotalDays < 8 );

				MockManager.Verify();
				MockManager.ClearAll();

				date = date.AddDays( 1 );
			}
		}

		[Test]
		public void GetNextStatisticsDeliveryDate_AfterTime()
		{
			int deliveryHour = 12;
			int deliveryMinute = 0;

			int randomYear = ( new Random().Next( 100 ) ) + 2000;
			Console.WriteLine( "Testing " + randomYear.ToString() + " year." );

			DateTime date = new DateTime( randomYear, 1, 1, 13, 0, 0 );
			while( date.Year == randomYear )
			{
				MockManager.Init();

				Mock dateClassMock = MockManager.Mock( typeof( DateClass ) );
				dateClassMock.ExpectAndReturn( "GetCurrentTime", date );

				DateTime deliveryDate = DateClass.GetNextStatisticsDeliveryDate( deliveryHour, deliveryMinute );
				Console.WriteLine( String.Format( "{0} - {1}", date.ToString( "d MMMM yyyy (dddd)" ), deliveryDate.ToString( "d MMMM yyyy (dddd) H:mm" ) ) );

				Assert.AreEqual( deliveryHour, deliveryDate.Hour );
				Assert.AreEqual( deliveryMinute, deliveryDate.Minute );

				Assert.Greater( deliveryDate, date );
				Assert.AreNotEqual( deliveryDate.Day, date.Day );
				Assert.IsTrue( ( deliveryDate.DayOfWeek == DayOfWeek.Monday ) || ( deliveryDate.Day == 1 ) );

				Assert.IsTrue( ( (TimeSpan) ( deliveryDate - date ) ).TotalDays < 8 );

				MockManager.Verify();
				MockManager.ClearAll();

				date = date.AddDays( 1 );
			}
		}
	}
}

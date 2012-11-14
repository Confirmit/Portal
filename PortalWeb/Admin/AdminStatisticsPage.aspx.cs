using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;

public partial class Admin_AdminStatisticsPage : BaseWebPage
{
	#region Поля
	private string sBegDate, sEndDate;
	#endregion

	#region Обработчики событий
	protected void Page_Load( object sender, EventArgs e )
	{ }
	#endregion

	#region Обработчики вызова отчетов

	protected void lbtnRSCurrentWeek_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за текущую неделю
		DateClass.GetPeriodCurrentWeek( out sBegDate, out sEndDate );
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}
	protected void lbtnRSCurrentMonth_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за текущий месяц
		DateClass.GetPeriodCurrentMonth( out sBegDate, out sEndDate );
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}
	protected void lbtnRSLastMonth_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за предыдущий месяц
		DateClass.GetPeriodLastMonth( out sBegDate, out sEndDate );
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}
	protected void lbtnRSLastWeek_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за последнюю неделю
		DateClass.GetPeriodLastWeek( out sBegDate, out sEndDate );
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	#endregion

	protected void GenerateReport( object sender, EventArgs e )
	{
		ReportToMoscowProducer producer = new ReportToMoscowProducer();
		DateTime begin = DateTime.Parse( tbReportFromDate.Text );
		DateTime end = DateTime.Parse( tbReportToDate.Text );
		Stream strm = producer.ProduceReport( begin, end );

		if( strm != null )
		{
			// очищаем поток ответа
			Response.Clear();
			// формируем заголовки ответа
			Response.ContentType = "application/octet-stream";

			Response.AddHeader( "Content-Disposition", "attachment; filename=" + HttpUtility.UrlPathEncode( "ExcelReport.xml" ) );

			// записываем данные в выходной поток
			strm.Seek( 0, SeekOrigin.Begin );
			byte[] data = new byte[ strm.Length ];
			strm.Read( data, 0, data.Length );
			Response.BinaryWrite( data );

			// сбрасываем данные в поток
			Response.Flush();

			// завершаем работу
			Response.End();
		}
	}
}

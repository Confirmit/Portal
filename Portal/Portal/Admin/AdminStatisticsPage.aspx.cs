using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

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
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect(URL);
	}

	protected void lbtnRSCurrentMonth_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за текущий месяц
		DateClass.GetPeriodCurrentMonth( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	protected void lbtnRSLastMonth_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за предыдущий месяц
		DateClass.GetPeriodLastMonth( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	protected void lbtnRSLastWeek_Click( object sender, EventArgs e )
	{
		//отправляем пользователя на страницу с отчетом за последнюю неделю
		DateClass.GetPeriodLastWeek( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	#endregion

	protected void GenerateReport( object sender, EventArgs e )
	{
		ReportToMoscowProducer producer = new ReportToMoscowProducer();
        DateTime begin = tbReportFromDate.Date;
        DateTime end = tbReportToDate.Date;

        //TODO this part of code will not work in the future
		Stream strm = producer.ProduceReport( begin, end, new List<int>());

		if( strm != null )
		{
			SendReport(strm);
		}
	}

    private void SendReport(Stream strm)
    {
// ������� ����� ������
        Response.Clear();
        // ��������� ��������� ������
        Response.ContentType = "application/octet-stream";

        Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlPathEncode("ExcelReport.xml"));

        // ���������� ������ � �������� �����
        strm.Seek(0, SeekOrigin.Begin);
        byte[] data = new byte[strm.Length];
        strm.Read(data, 0, data.Length);
        Response.BinaryWrite(data);

        // ���������� ������ � �����
        Response.Flush();

        // ��������� ������
        Response.End();
    }
}

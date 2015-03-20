using System;
using System.IO;
using System.Web;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;

public partial class Admin_AdminStatisticsPage : BaseWebPage
{
	#region ����
	private string sBegDate, sEndDate;
	#endregion

	#region ����������� �������
	protected void Page_Load( object sender, EventArgs e )
	{ }
	#endregion

	#region ����������� ������ �������

	protected void lbtnRSCurrentWeek_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ������� ������
		DateClass.GetPeriodCurrentWeek( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect(URL);
	}

	protected void lbtnRSCurrentMonth_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ������� �����
		DateClass.GetPeriodCurrentMonth( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	protected void lbtnRSLastMonth_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ���������� �����
		DateClass.GetPeriodLastMonth( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	protected void lbtnRSLastWeek_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ��������� ������
		DateClass.GetPeriodLastWeek( out sBegDate, out sEndDate );
		var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}

	#endregion

	protected void GenerateReport( object sender, EventArgs e )
	{
        tbReportFromDate.SelectedDate = DateTime.Today;
        tbReportToDate.SelectedDate = DateTime.Today;
		ReportToMoscowProducer producer = new ReportToMoscowProducer();
        DateTime begin = tbReportFromDate.SelectedDate;
        DateTime end = tbReportToDate.SelectedDate;
        tbReportFromDate.SelectedDate = end;
        //DateTime begin = DateTime.Parse( tbReportFromDate.Text );
        //DateTime end = DateTime.Parse( tbReportToDate.Text );
		Stream strm = producer.ProduceReport( begin, end );

		if( strm != null )
		{
			// ������� ����� ������
			Response.Clear();
			// ��������� ��������� ������
			Response.ContentType = "application/octet-stream";

			Response.AddHeader( "Content-Disposition", "attachment; filename=" + HttpUtility.UrlPathEncode( "ExcelReport.xml" ) );

			// ���������� ������ � �������� �����
			strm.Seek( 0, SeekOrigin.Begin );
			byte[] data = new byte[ strm.Length ];
			strm.Read( data, 0, data.Length );
			Response.BinaryWrite( data );

			// ���������� ������ � �����
			Response.Flush();

			// ��������� ������
			Response.End();
		}
	}
}

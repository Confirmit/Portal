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
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}
	protected void lbtnRSCurrentMonth_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ������� �����
		DateClass.GetPeriodCurrentMonth( out sBegDate, out sEndDate );
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}
	protected void lbtnRSLastMonth_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ���������� �����
		DateClass.GetPeriodLastMonth( out sBegDate, out sEndDate );
		string URL = hlStatPage.NavigateUrl + "?";
		URL += "BeginDate=" + sBegDate + "&";
		URL += "EndDate=" + sEndDate;
		Response.Redirect( URL );
	}
	protected void lbtnRSLastWeek_Click( object sender, EventArgs e )
	{
		//���������� ������������ �� �������� � ������� �� ��������� ������
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

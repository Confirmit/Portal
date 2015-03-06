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

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #endregion

    #region ����������� ������ �������

    protected void lbtnRSCurrentWeek_Click(object sender, EventArgs e)
    {
        //���������� ������������ �� �������� � ������� �� ������� ������
        DateClass.GetPeriodCurrentWeek(out sBegDate, out sEndDate);
        var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
        Response.Redirect(URL);
    }

    protected void lbtnRSCurrentMonth_Click(object sender, EventArgs e)
    {
        //���������� ������������ �� �������� � ������� �� ������� �����
        DateClass.GetPeriodCurrentMonth(out sBegDate, out sEndDate);
        var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
        Response.Redirect(URL);
    }

    protected void lbtnRSLastMonth_Click(object sender, EventArgs e)
    {
        //���������� ������������ �� �������� � ������� �� ���������� �����
        DateClass.GetPeriodLastMonth(out sBegDate, out sEndDate);
        var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
        Response.Redirect(URL);
    }

    protected void lbtnRSLastWeek_Click(object sender, EventArgs e)
    {
        //���������� ������������ �� �������� � ������� �� ��������� ������
        DateClass.GetPeriodLastWeek(out sBegDate, out sEndDate);
        var URL = "~/Statistics/OfficeStatistics.aspx?" + "BeginDate=" + sBegDate + "&" + "EndDate=" + sEndDate;
        Response.Redirect(URL);
    }

    #endregion

    protected void GenerateReport(object sender, EventArgs e)
    {
        ReportToMoscowProducer producer = new ReportToMoscowProducer();
        DateTime begin;
        DateTime end;
       // tbReportFromDate.Text = "123456";
            //    string.Format(tbReportFromDate.Text, this.Page.Request.QueryString["aspxerrorpath"]);
        if (!DateTime.TryParse(tbReportFromDate.Text, out begin))
        {
           // tbReportFromDate.Text = "123456";
				//string.Format( tbReportFromDate.Text, this.Page.Request.QueryString[ "aspxerrorpath" ] );
            return;
        }
        if (!DateTime.TryParse(tbReportToDate.Text, out end))
        {
            //error
            return;
        }

        //DateTime begin = DateTime.Parse( tbReportFromDate.Text );
        //  DateTime begin = new DateTime(2008, 12, 20);
        // DateTime end = new DateTime(2008, 12, 30);
        Stream strm = producer.ProduceReport(begin, end);

        if (strm != null)
        {
            // ������� ����� ������
            Response.Clear();
            // ��������� ��������� ������
            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition",
                "attachment; filename=" + HttpUtility.UrlPathEncode("ExcelReport.xml"));

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
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
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
        DateTime begin = tbReportFromDate.Date;
        DateTime end = tbReportToDate.Date;
	   
        var ruleRepository = new RuleRepository(new GroupRepository());
        
        var executor = new ReportComposerToMoscowExecutor(new RuleInstanceRepository(ruleRepository), begin, end);
        var rule = ruleRepository.GetAllRulesByType<NotReportToMoscowRule>().Single();
        executor.ExecuteRule(rule, new RuleInstance(rule, DateTime.Now));
	    Stream stream = executor.Stream;

        if (stream != null)
		{
            SendReport(stream);
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

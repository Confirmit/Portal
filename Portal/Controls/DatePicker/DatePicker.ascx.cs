using System;
using System.Globalization;
using System.Web.UI;

public partial class Controls_DatePicker : BaseUserControl
{
    public DateTime CurrentDate
    {
        get { return GetDate(); }
    }

    private DateTime GetDate()
    {
        DateTime result;
        string format = "MM.dd.yyyy";
        if (DateTime.TryParseExact(datePicker.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            return result;
            
        return DateTime.Today;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //datePicker.Attributes.Add("onclick", "test()");
        //var datePickerScript = "\n";
        //datePickerScript += "<script type=\"text/javascript\">\n";
        //datePickerScript += "$(function () {";
        //datePickerScript += "$('#ctl00$MainContentPlaceHolder$tbReportFromDate').datepick({ dateFormat: 'mm/dd/yyyy' });";
        //datePickerScript += "});\n";
        //datePickerScript += "</script>\n";
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", datePickerScript);


        var c = 0;
        c++;
        c = 5;
        String rsname = "js/test.js";
        String csname = "pickmeup";
        String csurl = "~js/jquery.pickmeup.min.js";
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        //ClientScriptManager cs = Page.ClientScript;

        //// Check to see if the include script exists already.
        //if (!cs.IsClientScriptIncludeRegistered(cstype, csname))
        //{
        //    cs.RegisterClientScriptInclude(cstype, csname, ResolveClientUrl(csurl));
        //}
        // Get a ClientScriptManager reference from the Page class.
      //  var text = (string)GetLocalResourceObject("Initialization");
     //   Page.ClientScript.RegisterStartupScript(this.GetType(), "datePicker", text);

        // var script = "  <script> $(function () {$(\"[id$=Date]\").datepicker({format: 'dd.mm.yyyy',language: 'ru',orientation: \"top left\",autoclose: true});});</script>";
        //  Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", script);
        //                 "$(\"[id$=Date]\").datepicker({" +
        //                 " format: 'dd.mm.yyyy'," +
        //                 " language: 'ru'"+
        //                "  });"+
        //                "  });";
        //// Check to see if the startup script is already registered.
        //    if (!cs.IsStartupScriptRegistered(this.GetType(), "myscript"))
        //    {
        //        String cstext1 = "alert('Hello World');";
        //        cs.RegisterStartupScript(this.GetType(), "myscript", script, true);
        //    }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        RegisterJavaScript();


        //var test = Page.ClientScript.GetWebResourceUrl(GetType(), @"\Portal\Controls\DatePicker\js\jquery.pickmeup.min.js");
        //if (!(Page.ClientScript.IsStartupScriptRegistered("DatePicker")))
        //    Page.ClientScript.RegisterStartupScript(GetType(), "DatePicker", "<script  language=\"javascript\" src=\""
        //        + Page.ClientScript.GetWebResourceUrl(GetType(), "Portal.Controls.DatePicker.js.DatePicker.js") +
        //        "\"></script>");

        //if (!Page.ClientScript.IsStartupScriptRegistered("DatePickerStylescss"))
        //    Page.ClientScript.RegisterStartupScript(GetType(), "DatePickerStylescss", "<link  href=\""
        //        + Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.DatePicker.Styles.css")
        //        + "\" rel=\"stylesheet\" type=\"text/css\" />");

    }

    protected void RegisterJavaScript()
    {
        //if (!(Page.ClientScript.IsClientScriptIncludeRegistered("DatePicker")))
        //    Page.ClientScript.RegisterClientScriptInclude(GetType(), "DatePicker"
        //                                                  , "../Controls/DatePicker/js/jquery.pickmeup.min.js");
        //if (!(Page.ClientScript.IsClientScriptIncludeRegistered("Jquery")))
        //    Page.ClientScript.RegisterClientScriptInclude(GetType(), "Jquery"
        //                                                  , "../Controls/DatePicker/js/jquery.js");
        //if (!(Page.ClientScript.IsClientScriptIncludeRegistered("Jquery")))
        //    Page.ClientScript.RegisterClientScriptInclude(GetType(), "Jquery"
        //                                                  , "../Controls/DatePicker/js/test.js");

        //StringBuilder strScript = new StringBuilder();
        //strScript.Append("<script language='javascript' type='text/javascript'>");
        //strScript.AppendFormat("var {0} = new FileUploader('{1}', '{2}', '{0}');",
        //                       m_uploadObjectName,
        //                       div_newsAttachments.ClientID,
        //                       m_attachID);
        //strScript.Append("</script>");

        //if (!(Page.ClientScript.IsStartupScriptRegistered("InitializeUploadObject")))
        //    Page.ClientScript.RegisterStartupScript(GetType(), "InitializeUploadObject"
        //                                                  , strScript.ToString());
    }

    protected void SetLocale()
    {
      //  var script = "  <script> $(function () { $(\"[id$=datePicker]\").pickmeup(";
        
      //  " {$(\"[id$=Date]\").datepicker({format: 'dd.mm.yyyy',language: 'ru',orientation: \"top left\",autoclose: true});});</script>";
    }
}

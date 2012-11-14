//   _                          _             _ _       
//  | |_ _____   _____ _ __ ___| |_ _   _  __| (_) ___  
//  | __/ _ \ \ / / _ \ '__/ __| __| | | |/ _` | |/ _ \ 
//  | || (_) \ V /  __/ |  \__ \ |_| |_| | (_| | | (_) |
//   \__\___/ \_/ \___|_|  |___/\__|\__,_|\__,_|_|\___/ 
//                                             (C)2006
//   Date  : 2006-07-10
//	 Author: loek@toverstudio:nl
//
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.Specialized;

namespace Controls.ColorPicker
{
    [ToolboxData("<{0}:ColorPicker runat=server></{0}:ColorPicker>")]
    public class ColorPicker : Control, IPostBackDataHandler
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SelectedHexValue
        {
            get
            {
                if (ViewState["SelectedValue"] == null)
                    return "";
                else
                    return (String)ViewState["SelectedValue"];
            }
            set { ViewState["SelectedValue"] = value; }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string opendialogscript = "\n";
            opendialogscript += "<script type=\"text/javascript\">\n";
            opendialogscript += "    var ActiveColorPicker;\n";
            opendialogscript += "    function OpenColorPicker(id)\n";
            opendialogscript += "    {\n";
            opendialogscript += "        ActiveColorPicker=id; \n";
            opendialogscript += "        var colorpickerdiv=document.getElementById('colorpicker');\n";
            opendialogscript += "        colorpickerdiv.style.visibility='visible';\n";
            opendialogscript += "        colorpickerdiv.style.left=ActiveColorPicker.style.left+ActiveColorPicker.style.width;\n";
            opendialogscript += "        colorpickerdiv.style.top=window.event.y;\n";
            opendialogscript += "        colorpickerdiv.style.left=window.event.x;\n";
            opendialogscript += "    }\n";
            opendialogscript += "    function ColorPickerOnColorClick(e)\n";
            opendialogscript += "    {\n";
            opendialogscript += "        var td = (e.target) ? e.target : e.srcElement; \n";
            opendialogscript += "        var color=td.style.backgroundColor; \n";
            opendialogscript += "        ActiveColorPicker.style.backgroundColor=color;\n";
            opendialogscript += "        ActiveColorPicker.style.color=color;\n";
            opendialogscript += "        var hiddenValue=document.getElementById(ActiveColorPicker.id + 'Hidden');\n";
            opendialogscript += "        hiddenValue.value=color;\n";
            opendialogscript += "        var colorpickerdiv=document.getElementById('colorpicker');\n";
            opendialogscript += "        colorpickerdiv.style.visibility='hidden';\n";
            opendialogscript += "    }\n";
            opendialogscript += "</script>\n";

            if (!Page.ClientScript.IsClientScriptBlockRegistered("opendialogscript"))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "opendialogscript", opendialogscript);

            string colorpickerdiv = "";

            colorpickerdiv += "<style>\n";
            colorpickerdiv += "     #colorpicker { visibility:hidden;border:1px solid #999;background-color:#eee;position:absolute;Z-INDEX: 1001; }\n";
            colorpickerdiv += "     #colorpicker td { border-right:1px solid #eee; border-bottom:1px solid #eee; width:10px;height:10px;cursor:pointer; }\n";
            colorpickerdiv += "     .colorpickerbutton{width:25px;border:1px solid #999;cursor:pointer;}\n";
            colorpickerdiv += "</style>\n";

            colorpickerdiv += "<div id=\"colorpicker\">";
            colorpickerdiv += "     <table cellpadding=\"0\" cellspacing=\"0\" onclick=\"javascript:ColorPickerOnColorClick(event);\" style=\"width:160px\">";
            string[] websafecolorlist = {"FFFFFF","CCCCCC","999999","666666","333333","000000","FFCC00","FF9900","FF6600","FF3300","FFFFFF","FFFFFF","FFFFFF","FFFFFF","FFFFFF","FFFFFF",
                    "99CC00","FFFFFF","FFFFFF","FFFFFF","FFFFFF","CC9900","FFCC33","FFCC66","FF9966","FF6633","CC3300","FFFFFF","FFFFFF","FFFFFF","FFFFFF","CC0033",
                    "CCFF00","CCFF33","333300","666600","999900","CCCC00","FFFF00","CC9933","CC6633","330000","660000","990000","CC0000","FF0000","FF3366","FF0033",
                    "99FF00","CCFF66","99CC33","666633","999933","CCCC33","FFFF33","996600","996600","663333","993333","CC3333","FF3333","CC3366","FF6699","FF0066",
                    "66FF00","99FF66","66CC33","669900","999966","CCCC66","FFFF66","996633","663300","996666","CC6666","FF6666","990033","CC3399","FF66CC","FF0099",
                    "33FF00","66FF33","339900","66CC00","99FF33","CCCC99","FFFF99","CC9966","CC6600","CC9999","FF9999","FF3399","CC0066","990066","FF33CC","FF00CC",
                    "00CC00","33CC00","336600","669933","99CC66","CCFF99","FFFFCC","FFCC99","FF9933","FFCCCC","FF99CC","CC6699","993366","660033","CC0099","330033",
                    "33CC33","66CC66","00FF00","33FF33","66FF66","99FF99","CCFFCC","FFFFFF","FFFFFF","FFFFFF","CC99CC","996699","993399","990099","663366","660066",
                    "006600","336633","009900","339933","669966","99CC99","FFFFFF","FFFFFF","FFFFFF","FFCCFF","FF99FF","FF66FF","FF33FF","FF00FF","CC66CC","CC33CC",
                    "003300","00CC33","006633","339966","66CC99","99FFCC","CCFFFF","3399FF","99CCFF","CCCCFF","CC99FF","9966CC","663399","330066","9900CC","CC00CC",
                    "00FF33","33FF66","009933","00CC66","33FF99","99FFFF","99CCCC","0066CC","6699CC","9999FF","9999CC","9933FF","6600CC","660099","CC33FF","CC00FF",
                    "00FF66","66FF99","33CC66","009966","66FFFF","66CCCC","669999","003366","336699","6666FF","6666CC","666699","330099","9933CC","CC66FF","9900FF",
                    "00FF99","66FFCC","33CC99","33FFFF","33CCCC","339999","336666","006699","003399","3333FF","3333CC","333399","333366","6633CC","9966FF","6600FF",
                    "00FFCC","33FFCC","00FFFF","00CCCC","009999","006666","003333","3399CC","3366CC","0000FF","0000CC","000099","000066","000033","6633FF","3300FF",
                    "00CC99","FFFFFF","FFFFFF","FFFFFF","FFFFFF","0099CC","33CCFF","66CCFF","6699FF","3366FF","0033CC","FFFFFF","FFFFFF","FFFFFF","FFFFFF","3300CC",
                    "FFFFFF","FFFFFF","FFFFFF","FFFFFF","FFFFFF","FFFFFF","00CCFF","0099FF","0066FF","0033FF","FFFFFF","FFFFFF","FFFFFF","FFFFFF","FFFFFF"};
            int rowcounter = 0;
            bool needrowstart = true;
            for (int i = 0; i < websafecolorlist.Length; i ++)
            {
                if (needrowstart)
                {
                    needrowstart = false;
                    colorpickerdiv += "            <tr> ";
                }
                string color = "#" + websafecolorlist[i];
                colorpickerdiv += "                <td style=\"background-color:" + color + "\"></td>";
                if (rowcounter++ == 16)
                {
                    colorpickerdiv += "            </tr>";
                    needrowstart = true;
                    rowcounter = 0;
                }
            }
            colorpickerdiv += "     </table>";
            colorpickerdiv += "</div>";

            if (!Page.ClientScript.IsClientScriptBlockRegistered("colorpickerdiv"))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "colorpickerdiv", colorpickerdiv);

        }

        protected override void Render(HtmlTextWriter output)
        {
            string html = "";
            string onclicker="onclick=\"javascript:OpenColorPicker(" + this.UniqueID + ");\"";
            html += "<input style=\"background-color:" + SelectedHexValue + ";color:" + SelectedHexValue + ";\" class=\"colorpickerbutton\" " + onclicker + " readonly=\"true\" class=\"lookupmodalvalue\" type=\"text\" name=\"" + this.UniqueID + "\" id=\"" + this.ClientID + "\">";
            html += "<input type=\"hidden\" name=\"" + this.UniqueID + "Hidden\" id=\"" + this.ClientID + "Hidden\" value=\"" + SelectedHexValue + "\">";
            output.Write(html);
        }

        public bool LoadPostData(String postDataKey, NameValueCollection values)
        {
            SelectedHexValue = values[this.UniqueID + "Hidden"];
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
        }
    }
}

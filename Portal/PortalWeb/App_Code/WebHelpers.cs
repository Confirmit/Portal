using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Net;

using ConfirmIt.PortalLib.IPRangeChecker;
using System.Configuration;

public class SelectedObjectEventArgs : EventArgs
{
	public int ObjectID { get; set; }
}

/// <summary>
/// Class with different helpers.
/// </summary>
public static class WebHelpers
{
	public static bool IsRequestIPAllowed()
	{
		if (HttpContext.Current.Request.UserHostAddress.Equals("127.0.0.1"))
			return true;

		var virtualXmlFilePath = ConfigurationManager.AppSettings["AllowedIPConfigFilePath"];
		var xmlFilePath = HttpContext.Current.Server.MapPath(virtualXmlFilePath);

		var xmlDoc = new XmlDocument();
		xmlDoc.Load(xmlFilePath);

		var rootNodes = xmlDoc.GetElementsByTagName("config");
		var xmlRootNode = rootNodes[0] as XmlElement;

		var ipList = (from XmlNode item in xmlRootNode.ChildNodes 
					  where item.Name == "ip" 
					  select item.Attributes["Mask"].Value).ToList();

		var userIp = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);

		return ipList.Any(ipMask => IPRangeChecker.IsIPValid(userIp, ipMask));
	}

	/// <summary>
	/// Adds the onfocus and onblur attributes to all input controls found in the specified parent,
	/// to change their apperance with the control has the focus
	/// </summary>
	public static void SetInputControlsHighlight(Control container, string className, bool onlyTextBoxes)
	{
		foreach (Control ctl in container.Controls)
		{
			if (ctl is Controls.HotGridView.GridView)
				continue;

			if ((onlyTextBoxes && ctl is TextBox) || (!onlyTextBoxes && (ctl is TextBox || ctl is DropDownList ||
				ctl is ListBox || ctl is CheckBox || ctl is RadioButton ||
				ctl is RadioButtonList || ctl is CheckBoxList)))
			{
				WebControl wctl = ctl as WebControl;
				if (wctl != null && wctl.EnableTheming)
				{
					wctl.Attributes.Add("onfocus",
										string.Format("this.className = '{0}';",
													  className));
					wctl.Attributes.Add("onblur",
										string.Format("this.className = '{0}';",
													  wctl.CssClass));
				}
			}
			else
			{
				if (ctl.Controls.Count > 0)
					SetInputControlsHighlight(ctl, className, onlyTextBoxes);
			}
		}
	}
}

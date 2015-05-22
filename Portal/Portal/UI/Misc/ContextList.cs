using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Список контекстных команд.
	/// </summary>
	[ParseChildren( typeof(ContextListItem) )]
	public class ContextList : Control
	{
		protected override void RenderChildren( HtmlTextWriter writer )
		{
			writer.AddAttribute( HtmlTextWriterAttribute.Cellpadding, "0" );
			writer.AddAttribute( HtmlTextWriterAttribute.Cellspacing, "0" );
			writer.RenderBeginTag( HtmlTextWriterTag.Table );
			writer.RenderBeginTag( HtmlTextWriterTag.Tr );
			foreach(Control control in Controls)
			{
				ContextListItem item = control as ContextListItem;
				if(item == null) continue;

				// если команда видима и доступна
				if(item.Visible && item.IsAccessible)
				{
					// создаем ячейку
					writer.RenderBeginTag( HtmlTextWriterTag.Td );
					item.RenderControl( writer );
					writer.RenderEndTag();

					writer.AddStyleAttribute( HtmlTextWriterStyle.PaddingLeft, "5px" );
					writer.AddStyleAttribute( HtmlTextWriterStyle.PaddingRight, "5px" );
					writer.RenderBeginTag( HtmlTextWriterTag.Td );
					writer.Write( "|" );
					writer.RenderEndTag( );
				}
			}
			writer.RenderEndTag(); // </tr>
			writer.RenderEndTag(); // </table>
		}
	}
}
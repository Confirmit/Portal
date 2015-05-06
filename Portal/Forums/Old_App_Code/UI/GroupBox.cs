using System;
using System.Data;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Тип отображения контрола.
	/// </summary>
	public enum GroupBoxDisplayType
	{
		/// <summary>
		/// Состояние сохраняется между запросами.
		/// </summary>
		Custom = 0,
		/// <summary>
		/// Всегда открыт.
		/// </summary>
		Expanded = 1,
		/// <summary>
		/// Всегда закрыт.
		/// </summary>
		Collapsed = 2
	}
	/// <summary>
	/// Контрол для группировки других контролов.
	/// </summary>
	public class GroupBox : PlaceHolder, ITextControl
	{
		private GroupBoxDisplayType m_DisplayType = GroupBoxDisplayType.Custom;
		
		/// <summary>
		/// Тип отображения контрола.
		/// </summary>
		public GroupBoxDisplayType DisplayType
		{
			get
			{
				return m_DisplayType;
			}
			set
			{
				m_DisplayType = value;
			}
		}
		[Localizable(true)]
		public string Text
		{
			get
			{
				object o = ViewState["Text"];
				return o != null ? (string)o : String.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		protected override void OnLoad( EventArgs e )
		{
			//if( this.Visible )
			base.OnLoad( e );
		}

		protected override void Render( HtmlTextWriter writer )
		{
			string border = "solid 1px #a0a0a0";

			// рендерим блок, который будет отображаться в открытом состоянии
			writer.Write(String.Format(
			@"<div id=""{0}"" style=""padding-bottom:5px; padding-top:5px;"">
			<table cellpadding=0 cellspacing=0 width=""100%"">
			<tr><td>
				<table cellpadding=0 cellspacing=0 width=""100%"">
				<tr> 
				<td><table cellpadding=0 cellspacing=0 width=""100%""><tr><td>&nbsp;</td></tr><tr><td style=""border-left:{2};border-top:{2};"">&nbsp;</td></tr></table></td>
				<td style=""padding-left:5px; padding-right:5px;"" nowrap>
					<a href=""javascript:toggleDiv('{0}','{0}');""><img alt="""" src=""{3}""></a>
					<b>{1}</b>
				</td>
				<td style=""width:100%;""><table cellpadding=0 cellspacing=0 width=""100%""><tr><td>&nbsp;</td></tr><tr><td style=""border-right:{2};border-top:{2};"">&nbsp;</td></tr></table></td>
				</tr>
				</table>
			</td>
			</tr>
			<tr>
				<td style=""border-left:{2};border-bottom:{2};border-right:{2};padding:5px 5px 5px 5px;"">
				",
				this.ClientID,
				Text,
				border,
				ResolveClientUrl("~/images/btn_up.gif")
				)
			);

			base.Render( writer );

			writer.Write(String.Format(
			@"	</td>
			</tr>
			</table>
			</div>",
					 this.ClientID
			));
			// рендерим блок, который будет отображаться в закрытом состоянии
			writer.Write(String.Format(
			@"<div id=""{0}_bottom"" style=""display: none; padding-bottom:5px; padding-top:5px;"">
			   <table cellpadding=0 cellspacing=0 width=""100%"">
			   <tr><td>
				   <table cellpadding=0 cellspacing=0 width=""100%"">
					   <tr> 
						   <td>
							   <table cellpadding=0 cellspacing=0 width=""100%"">
								   <tr>
									   <td>&nbsp;</td>
								   </tr>
								   <tr>
									   <td style=""border-top:{2};"">&nbsp;</td>
								   </tr>
							   </table>
						   </td>
						   <td style=""padding-left:6px; padding-right:5px;"" nowrap>
							   <a href=""javascript:toggleDiv('{0}','{0}');""><img alt="""" src=""{3}""></a>
							   <b>{1}</b>
						   </td>
						   <td style=""width:100%;"">
							   <table cellpadding=0 cellspacing=0 width=""100%"">
								   <tr>
									   <td>&nbsp;</td>
								   </tr>
								   <tr>
									   <td style=""border-top:{2};"">&nbsp;</td>
								   </tr>
							   </table>
						   </td>
					   </tr>
				   </table>
				   </td>
			   </tr>
			   </table>
			   </div>
				",
			             	this.ClientID,
			             	Text,
			             	border,
			             	ResolveClientUrl("~/images/btn_down.gif")
			             	)
				);
			
			if (DisplayType == GroupBoxDisplayType.Custom)
			{
				Page.ClientScript.RegisterStartupScript(
					this.GetType(), 
					this.ClientID + "_activation_script",
					String.Format(
						@"
						 var visibility = readCookie('{0}');
						 if( visibility != null)
						 if( visibility == 'hidden' )
								toggleDiv('{0}','{0}');
						",
						this.ClientID
						), 
					true
				);
			}
			else
			{
				if (DisplayType == GroupBoxDisplayType.Collapsed)
				{
					string script;
					// если это первая загрузка то группбокс свернут 
					if (!Page.IsPostBack)
					{
						script = String.Format(
						@"
						  eraseCookie('{0}');
						  createCookie('{0}','hidden', 1);
						  toggleDiv('{0}','{0}');
						",
						this.ClientID
						);
					}
					// иначе, смотрим содержимое cookie
					else 
					{
						script = String.Format(
						@"
						 var visibility = readCookie('{0}');
						 if( visibility == null)
						 {{
						  eraseCookie('{0}');
						  createCookie('{0}','hidden', 1);
						  toggleDiv('{0}','{0}');
						 }}
						  else
							if( visibility == 'hidden' )
								toggleDiv('{0}','{0}');
						",
						this.ClientID
						);
					}
					Page.ClientScript.RegisterStartupScript(
						this.GetType(), 
						this.ClientID + "_activation_script", 
						script,
						true
					);
				}
			}
		}
	}
}

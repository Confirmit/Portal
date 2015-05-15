using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

using Core;
using Core.Security;
using System.Text.RegularExpressions;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Режимы поля LinkField.
	/// </summary>
	public enum LinkFieldModes
	{
		/// <summary>
		/// В этом режиме используется LinkButton.
		/// Команда для него берется из свойства CommandName.
		/// В качестве аргумента команды исползуется идентификатор текущего объекта.
		/// </summary>
		Command = 0,
		/// <summary>
		/// В этом режиме используется прямая ссылка.
		/// Url берется из свойства RedirectUrl. 
		/// В ссылку подставляется идентфикатор текущего объекта в виде параметра id.
		/// </summary>
		Redirect = 1
	}

	/// <summary>
	/// Колонка таблицы, использующая ссылки.
	/// </summary>
	public class LinkField : BaseBoundField
	{
		#region Конструкторы

		public LinkField()
		{
		}

		#endregion

		#region Свойства
		/// <summary>
		/// Режим работы поля.
		/// </summary>
		[Browsable(true)]
		public LinkFieldModes Mode
		{
			get
			{
				object o = ViewState["Mode"];
				return o != null ? (LinkFieldModes)o : LinkFieldModes.Command;
			}
			set
			{
				ViewState["Mode"] = value;
			}
		}

		/// <summary>
		/// Имя команды для LinkButton.
		/// Например, "edit" или "select".
		/// </summary>
		[Browsable( true )]
		public string CommandName
		{
			get
			{
				string text = (string)ViewState["CommandName"];
				return text != null ? text : String.Empty;
			}
			set
			{
				ViewState["CommandName"] = value;
			}
		}

		/// <summary>
		/// Текст подсказки.
		/// </summary>
		[Browsable( true )]
		public string ToolTip
		{
			get
			{
				string text = (string)ViewState["ToolTip"];
				return text != null ? text : String.Empty;
			}
			set
			{
				ViewState["ToolTip"] = value;
			}
		}

		/// <summary>
		/// Url, на который будет вести ссылка в режиме Redirect. 
		/// В него подставляется идентификатор объекта (параметр id).
		/// </summary>
		[Browsable( true )]
		public string RedirectUrl
		{
			get
			{
				string text = (string)ViewState["RedirectUrl"];
				return text != null ? text : String.Empty;
			}
			set
			{
				ViewState["RedirectUrl"] = value;
			}
		}

		/// <summary>
		/// Имя поля (свойства) объекта, которое содержит идентификатор объекта.
		/// </summary>
		[Browsable( true )]
		public string DataKeyField
		{
			get
			{
				string text = (string)ViewState["DataKeyField"];
				return text != null ? text : "ID";
			}
			set
			{
				ViewState["DataKeyField"] = value;
			}
		}

		/// <summary>
		/// Текст ссылки. Если он задан, то используется он. 
		/// Иначе используется значение, вычисляемое с помощью свойства DataField.
		/// </summary>
		[Browsable( true )]
		[Localizable(true)]
		public string Text
		{
			get
			{
				string text = (string)ViewState["Text"];
				return text != null ? text : String.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		#endregion

		#region Дочерние контролы

		private WebControl m_Link;

		#endregion

		protected override void InitializeDataCell( DataControlFieldCell cell, DataControlRowState rowState )
		{
			// в зависимости от режима поля формируем различное содержимое
			if(IsInSelectMode( this.Control ))
			{
			}
			else
			{
				switch(Mode)
				{
					case LinkFieldModes.Command:
						{
							LinkButton link = new LinkButton();
							cell.Controls.Add( link );
							m_Link = link;
							break;
						}
					case LinkFieldModes.Redirect:
						{
							HyperLink link = new HyperLink();
							cell.Controls.Add( link );
							m_Link = link;
							break;
						}
				}
			}

			if(base.Visible)
			{
				cell.DataBinding += new EventHandler( this.OnDataBindField );
			}
		}

		protected override void OnDataBindField( object sender, EventArgs e )
		{
			Control control = (Control)sender;
			Control controlContainer = control.NamingContainer;
			string text;
			if(!String.IsNullOrEmpty( Text ))
			{
				// если задан Text, то используем его
				text = Text;
			}
			else
			{
				// иначе вычисляем значение с помощью DataField
				object dataValue = this.GetValue( controlContainer );
				bool encode = (this.SupportsHtmlEncode && this.HtmlEncode) && (control is TableCell);
				text = this.FormatDataValue( dataValue, encode );
			}

			if(control is TableCell)
			{
				if(text.Length == 0)
				{
					text = "&nbsp;";
				}
				string dataKeyValue = GetDataKeyValue( controlContainer ).ToString();
				if(IsInSelectMode( this.Control ))
				{
					// если грид находится в режиме выбора, то выводим простой текст
					((TableCell)control).Text = text;
				}
				else
				{
					// в зависимости от режима поля формируем различное содержимое
					switch(Mode)
					{
						case LinkFieldModes.Command:
							{
								LinkButton link = (LinkButton)m_Link;
								link.CommandName = CommandName;
								link.CommandArgument = dataKeyValue;
								link.Text = text;
								link.ToolTip = ToolTip;
								break;
							}
						case LinkFieldModes.Redirect:
							{
								if(String.IsNullOrEmpty( RedirectUrl ))
								{
									throw new ArgumentException( "You must specify RedirectUrl property if you use Mode=Redirect." );
								}

								HyperLink link = (HyperLink)m_Link;
								link.NavigateUrl = WebHelper.ReplaceUrlQueryParameter( RedirectUrl, "id", dataKeyValue );
								link.Text = text;
								link.ToolTip = ToolTip;
								control.Controls.Add( link );
								break;
							}
					}
				}
			}
		}

		/// <summary>
		/// Возвращаем идентификатор объекта, для которого формируется ссылка.
		/// </summary>
		/// <param name="controlContainer"></param>
		/// <returns></returns>
		private object GetDataKeyValue( Control controlContainer )
		{
			object dataObject = DataBinder.GetDataItem( controlContainer );

			return GetPropertyExpressionValue( dataObject, DataKeyField );
		}

		/// <summary>
		/// Находится ли грид в режиме выбора.
		/// </summary>
		/// <param name="control">Контрол GridView.</param>
		public bool IsInSelectMode( Control control )
		{
			return HttpContext.Current.Request["SelectMode"] != null
				&& HttpContext.Current.Request["SelectGridID"] == (control.NamingContainer).ID;
		}
	
	}
}
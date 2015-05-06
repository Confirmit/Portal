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

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Колонка таблицы, использовать многоязычные значения и сортировать по ним.
	/// </summary>
	public class TemplateField : System.Web.UI.WebControls.TemplateField, IAccessible
	{
		/// <summary>
		/// Является ли данная колонка многоязычной (типа MLString). 
		/// Если является, то выражение для сортировки будут формироваться 
		/// на основе указанного значения, а также префикса, соответствующего текущему языку.
		/// </summary>
		public bool IsMultilangual
		{
			get
			{
				object o = ViewState["IsMultilangual"];
				return o != null ? (bool)o : false;
			}
			set
			{
				ViewState["IsMultilangual"] = value;
			}
		}

		/// <summary>
		/// Выражение для сортировки.
		/// Для многоязычных колонок к исходному выражению добавляется префикс языка.
		/// </summary>
		public override string SortExpression
		{
			get
			{
				if(base.SortExpression != String.Empty && IsMultilangual)
				{
					return CultureManager.CurrentLanguage == CultureManager.Languages.Russian
						? "r" + base.SortExpression
						: "e" + base.SortExpression;
				}
				return base.SortExpression;
			}
			set
			{
				base.SortExpression = value;
			}
		}

		/// <summary>
		/// Список ролей, для которых разрешен показ данной кнопки. 
		/// Строка, в которой через запятую перечислены названия ролей. 
		/// Если оставить незаполненной или пустой, то контрол будет доступен всем.
		/// </summary>
		public string AllowedRoles
		{
			get
			{
				object o = ViewState["AllowedRoles"];
				return o != null ? (string)o : String.Empty;
			}
			set
			{
				ViewState["AllowedRoles"] = value;
			}
		}

		/// <summary>
		/// Определяет, будет ли колонка выводиться на печать
		/// </summary>
		public bool IsPrintable
		{
			get
			{
				object o = ViewState["IsPrintable"];
				return o != null ? (bool)o : true;
			}
			set
			{
				ViewState["IsPrintable"] = value;
			}
		}

		#region IAccessible Members

		public bool CheckAccessibilityToUser( User user )
		{
			if( this.Control != null )
				if( ( (BaseWebPage)this.Control.Page ).IsInPrintMode && !IsPrintable )
					return false;
			return !String.IsNullOrEmpty( AllowedRoles ) ? user.IsInRoles( AllowedRoles ) : true;
		}

		#endregion
	}
}

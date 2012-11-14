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
	///  олонка таблицы, использовать много€зычные значени€ и сортировать по ним.
	/// </summary>
	public class TemplateField : System.Web.UI.WebControls.TemplateField, IAccessible
	{
		/// <summary>
		/// явл€етс€ ли данна€ колонка много€зычной (типа MLString). 
		/// ≈сли €вл€етс€, то выражение дл€ сортировки будут формироватьс€ 
		/// на основе указанного значени€, а также префикса, соответствующего текущему €зыку.
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
		/// ¬ыражение дл€ сортировки.
		/// ƒл€ много€зычных колонок к исходному выражению добавл€етс€ префикс €зыка.
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
		/// —писок ролей, дл€ которых разрешен показ данной кнопки. 
		/// —трока, в которой через зап€тую перечислены названи€ ролей. 
		/// ≈сли оставить незаполненной или пустой, то контрол будет доступен всем.
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
		/// ќпредел€ет, будет ли колонка выводитьс€ на печать
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

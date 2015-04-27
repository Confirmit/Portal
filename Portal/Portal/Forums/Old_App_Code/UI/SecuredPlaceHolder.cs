using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

using Core;
using Core.Security;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ѕлейсхолдер с возможностью настройки безопасности (показ/сокрытие дл€ определенных ролей).
	/// </summary>
	public class SecuredPlaceHolder : System.Web.UI.WebControls.PlaceHolder, IAccessible
	{
		/// <summary>
		/// —писок ролей, дл€ которых разрешен показ данного контрола. 
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

		#region IAccessible Members

		public bool CheckAccessibilityToUser( User user )
		{
			bool isAccesible = !String.IsNullOrEmpty( AllowedRoles ) ? user.IsInRoles( AllowedRoles ) : true;
			if(!isAccesible)
			{
				this.Visible = false;
			}
			return isAccesible;
		}

		#endregion
	}
}
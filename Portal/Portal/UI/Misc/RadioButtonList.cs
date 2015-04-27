using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EPAMSWeb.UI
{

	/// <summary>
	/// Summary description for RadioButtonList
	/// </summary>
	public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList
	{
		protected override void OnInit( EventArgs e )
		{
			if( IsPermanent )
				if( Page.Session[UniqueSessionKey] != null )
					this.SelectedValue = (string)Page.Session[UniqueSessionKey];
			base.OnInit( e );
		}

		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			if( IsPermanent )
				Page.Session[UniqueSessionKey] = this.SelectedValue;
		}

		private bool m_permanent = false;
		/// <summary>
		/// —охран€ет ли контрол значение в сессии (по умолчанию - false)
		/// </summary>
		public bool IsPermanent
		{
			get
			{
				return m_permanent;
			}
			set
			{
				m_permanent = value;
			}
		}

		/// <summary>
		/// ”никальный в пределах системы ключ дл€ данного контрола дл€ сохранени€ данных в сессии
		/// </summary>
		protected string UniqueSessionKey
		{
			get
			{
				return BaseUserControl.GetUniqueSessionKey( this );
			}
		}
	}

}
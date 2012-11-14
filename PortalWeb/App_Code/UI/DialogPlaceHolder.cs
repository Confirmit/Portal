using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Core;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// PlaceHolder, � ������� ������ ���� �������� �������� �������, ���������� �� �������� �������.
	/// </summary>
	public class DialogPlaceHolder : System.Web.UI.WebControls.PlaceHolder
	{
		/// <summary>
		/// ���������� �������� BaseWebPage, �� ������� ����� ������ �������.
		/// </summary>
		public IDialog DialogPage
		{
			get
			{
				if(!(Page is IDialog))
					throw new InvalidOperationException( "You must use DialogPlaceHolder in IDialog page only." );

				return (IDialog)Page;
			}
		}

		public DialogPlaceHolder()
		{
		}

		protected override void OnPreRender( EventArgs e )
		{
			if(!DesignMode)
			{
				BaseObject obj = DialogPage.GetCurrentObject() as BaseObject;
				if(obj != null)
				{
					this.Visible = obj.IsSaved;
				}
			}

			base.OnPreRender( e );
		}
	}
}

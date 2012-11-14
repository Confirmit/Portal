using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Text.RegularExpressions;

using Core;
using Core.Security;
using EPAMSWeb.UI;
using UlterSystems.PortalLib.BusinessObjects;

/// <summary>
/// ������� ����� ��� �����
/// </summary>
public abstract class BaseWebPage : System.Web.UI.Page
{
	#region ������� ��������

	#endregion

	#region ������������

	protected BaseWebPage() { }

	#endregion

	#region ����������� ���������� �����

	protected override void  OnPreInit(EventArgs e)
	{
		if( IsInPrintMode )
			this.MasterPageFile = "~/MasterPages/PrintMode.master";
 		base.OnPreInit(e);
	}

	protected override void OnLoad( EventArgs e )
	{
		base.OnLoad( e );
	}

	protected override void OnPreRenderComplete( EventArgs e )
	{
		base.OnPreRenderComplete( e );
		// ����������� �� ���� ��������� � ��������� �� ����������� 
		// (� ����������� �� ���� �������� ������������)
		CheckControlsAccessibility( this.Controls );
		// ���������� �������� ��� ������ ������
		if( IsInPrintMode )
		{
			ContentPlaceHolder content = FindContent( this.Controls );
			if(content != null)
			{
				ExcludePrintControls( content.Controls );
			}
		}
	}

	/// <summary>
	/// ��������� ����������� ��������� �� �������� � �������� �� ���������.
	/// </summary>
	/// <param name="controls">��������� ���������.</param>
	private void CheckControlsAccessibility( ControlCollection controls )
	{
		foreach (Control control in controls)
		{
			// ���� ������� �����, �� ��� ������ ��������� ��� �����������.
			if(!control.Visible) continue;

			bool isAccessible = true;

			IAccessible accessControl = control as IAccessible;
			if(accessControl!=null)
			{
				isAccessible = accessControl.CheckAccessibilityToUser( Core.Security.User.Current );
			}

			// ��������� ��� �������� ��������, ������ ���� ������� ��������
			if(isAccessible && control.HasControls())
			{
				CheckControlsAccessibility( control.Controls );
			}
		}
	}

	/// <summary>
	/// ������� � ���������� ContentPlaceHolder � ���������.
	/// </summary>
	/// <param name="controls"></param>
	/// <returns></returns>
	private ContentPlaceHolder FindContent( ControlCollection controls )
	{ 
		foreach( Control control in controls )
		{
			if(control is ContentPlaceHolder && control.ID=="Content")
			{
				return (ContentPlaceHolder)control;
			}
			if( control.HasControls() )
			{
				ContentPlaceHolder cnt = FindContent( control.Controls );
				if(cnt != null)
				{
					return cnt;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// ��������� ��� ��������, ������� �� ����� � ����������� ������������ PrintPlaceHolder.
	/// </summary>
	/// <param name="controls"></param>
	/// <returns></returns>
	private bool ExcludePrintControls( ControlCollection controls )
	{
		bool visible = false;
		foreach( Control control in controls )
		{
			if(control is PrintPlaceHolder)
			{
				control.Visible = true;
			}
			else
			{
				if(control.Visible && !ExcludePrintControls( control.Controls ))
				{
					control.Visible = false;
				}
			}

			if(control.Visible)
			{
				visible = true;
			}
		}
		return visible;
	}

	#endregion

	#region ����� �������� ��������

    /// <summary>
    /// Current user of system.
    /// </summary>
    public Person CurrentUser
    {
        get
        {
            if (Session["UserID"] == null)
                return null;

            Person curUser = new Person();
            curUser.Load((int)Session["UserID"]);
            return curUser;
        }
    }
	private string m_caption = null;
	/// <summary>
	/// ��������� ��������� ����� ��������. ���������� �� ��������� �������� ����������� (���� Caption).
	/// </summary>
	public string Caption
	{
		get
		{
			if( m_caption != null )
				return m_caption;
			try
			{
				return (string)GetLocalResourceObject( "Caption" );
			}
			catch(System.Resources.MissingManifestResourceException)
			{
				return Title;
			}
			catch(System.InvalidOperationException)
			{
				return Title;
			}
		}
		set
		{ 
			m_caption = value;
		}
	}

	/// <summary>
	/// URL ��������, � ������� �� ����� �� �������, ��� null, ���� �� �����.
	/// </summary>
	public string ReturnUrl
	{
		get
		{
			return Request.Params["ReturnUrl"];
		}
		set
		{
			string url = WebHelper.ReplaceUrlQueryParameter( Request.RawUrl, "ReturnUrl", value );
			Response.Redirect( url, true );
		}
	}

	/// <summary>
	/// �������������� �������� ��� ������������� ReturnUrl.
	/// ��� ����� ���������� ������ ����������� �������� � ��������� ��� � ���� Url.
	/// ��������: ������������ ������ � ������ Page_Load().
	/// </summary>
	public virtual void PrepareReturnUrl()
	{
		// ���� �� ������ ��� ����� �� ��� ��������, � � ��� ��� ��������� ReturnUrl, 
		// �� ��������� � Url �������� ReturlUrl � ������ �������� �� ����.
		// ������ ���� ���� ��������, �� ������������� ReturnUrl �� ����, ����� �� ����.
		if(ReturnUrl == null)
		{
			ReturnUrl = (Request.UrlReferrer != null)
				? HttpUtility.UrlEncode( Request.UrlReferrer.PathAndQuery )
				: HttpUtility.UrlEncode( Request.RawUrl );
		}
	}

	/// <summary>
	/// URL ��� �������� �� ���������� ��������.
	/// � ������, ���� �������� �� ������, ������������ ReturnUrl.
	/// </summary>
	public string BackButtonURL
	{
		get
		{
			return (string)ViewState["BackButtonURL"];
		}
		set
		{
			ViewState["BackButtonURL"] = value;
		}
	}

	private bool m_hasPrintMode = false;
	/// <summary>
	/// �������� �� ����� ������ ��� ��������
	/// </summary>
	public bool HasPrintMode
	{
		get
		{
			return m_hasPrintMode;
		}
		set
		{
			m_hasPrintMode = value;
		}
	}

	/// <summary>
	/// ��������� �� �������� � ������ ������
	/// </summary>
	public bool IsInPrintMode
	{
		get
		{
			return Request.Params["print_mode"] != null;
		}
	}

	#endregion

	#region �������� � ������ ��� ������ � �������� �����

	/// <summary>
	/// ������ �����, ������� ���� ���������������� �� ����� ��������� ���������� ��������� ������.
	/// </summary>
	public MessageCollection Errors
	{
		get
		{
			if(m_Errors == null)
			{
				m_Errors = new MessageCollection();
			}
			return m_Errors;
		}
	}

	private MessageCollection m_Errors;

	/// <summary>
	/// �������� ��������� �� ������ � ������ ������ �����.
	/// </summary>
	/// <param name="message">��������� �� ������.</param>
	/// <returns></returns>
	public void ReportError( string message )
	{
		Errors.Add( new Message( message, MessageType.Error ) );
	}

	#endregion

	#region ������ ����������� ��������
	/// <summary>
	/// ����������� �������������, ������������ ��� ����� �� ��������.
	/// </summary>
	/// <param name="control">�������, ���� �� ������� �������������.</param>
	/// <param name="confirm">����� �������������.</param>
	public void UnregisterClickEvents( WebControl control )
	{
		if(control == null) return;
		control.Attributes.Remove( "Onclick" );
	}
	/// <summary>
	/// ����������� �������������, ������������ ��� ����� �� ��������.
	/// </summary>
	/// <param name="control">�������, ���� �� ������� �������������.</param>
	/// <param name="confirm">����� �������������.</param>
	public void RegisterConfirm( WebControl control, string confirm )
	{
		if(control == null) return;
		control.Attributes.Add( "Onclick", "return confirm('" + confirm + "');" );
	}
	/// <summary>
	/// ����������� ��������������, ������������ ��� ����� �� ��������.
	/// </summary>
	/// <param name="control">�������, ���� �� ������� �������������.</param>
	/// <param name="alert">����� ��������������.</param>
	public void RegisterAlert( WebControl control, string alert )
	{
		if(control == null) return;
		control.Attributes.Add( "Onclick", "alert('" + alert + "'); return false;" );
	}

	/// <summary>
	///  ����������� �������������� ��� �������� ���������� ��������������.
	/// </summary>
	/// <param name="control">�������, ���� �� ������� �������������.</param>
	/// <param name="grid">����, ��� �������� ����������� ��������.</param>
	/// <param name="confirm">����� ��������������.{0} - ����� � ������� ���������� ���������� ��������.</param>
	public void RegisterGroupConfirm(WebControl control, Control grid, string confirm)
	{
		if (control == null) return;
		control.Attributes.Add("Onclick", " var id = '" + grid.ClientID + "_totalSelected' ; var mess = '" + confirm + "';return confirm(mess.replace('{0}',document.getElementById(id).value));");
	}

	#endregion

	#region ������ ��� ���������� � ������ � Url
	/// <summary>
	/// ���������� �������� �� �������� ��������, � ������� �� ����� �� ������.
	/// ������� �������� ������� �� �������� ReturnUrl. ���� ReturnUrl �� �����, �������� ������� �� UrlReferrer.
	/// � ������ �������, �������� �� ���� �� ��������.
	/// </summary>
	public void RedirectToReferrer()
	{
		if(ReturnUrl != null)
		{
			Response.Redirect( ReturnUrl );
		}
		else
		{
			if(Request.UrlReferrer != null)
			{
				Response.Redirect( Request.UrlReferrer.PathAndQuery );
			}
		}
	}

	/// <summary>
	/// ���������� �������� �� �� �� ��������.
	/// </summary>
	public void RedirectToMySelf()
	{
		//Response.Redirect( Request.Url.PathAndQuery );
        Response.Redirect(Request.RawUrl);
	}

	/// <summary>
	/// ���������� �������� �� ��������� Url. 
	/// ����� ���� ��� ���������� (www.google.com/index.html), 
	/// ��� � ������������� (~/UserManager/UserList.aspx).
	/// </summary>
	public void RedirectToUrl(string url)
	{
		Response.Redirect( url );
	}

	/// <summary>
	/// ���������� �������� �� ��������� Url � ��������� ���������� � ������� ��������, �� ������� ����� ���������� ������ Back
	/// </summary>
	/// <param name="url"></param>
	public void RedirectToUrlWithReturn( string url )
	{
		Response.Redirect( WebHelper.ReplaceUrlQueryParameter( url, "ReturnURL", HttpUtility.UrlEncode( Request.RawUrl.ToString() ) ) );
	}

	#endregion
}

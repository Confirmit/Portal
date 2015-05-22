using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Core;
using Core.Security;

using EPAMSWeb.UI;
using UIPProcess.UIP.Views.Page;
using UlterSystems.PortalLib.BusinessObjects;

/// <summary>
/// Базовый класс для формы
/// </summary>
public abstract class BaseWebPage : PageViewBase // Page
{
	#region Обработчики жизненного цикла

	protected override void  OnPreInit(EventArgs e)
	{
		if( IsInPrintMode )
			MasterPageFile = "~/MasterPages/PrintMode.master";

 		base.OnPreInit(e);
	}

	protected override void OnLoad( EventArgs e )
	{
		base.OnLoad( e );

        WebHelpers.SetInputControlsHighlight(this, "highlight", true);
	}

    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);
        // пробегаемся по всем контролам и проверяем их доступность 
        // (в зависимости от прав текущего пользователя)
        CheckControlsAccessibility(Controls);
        
        // определяем контролы для режима печати
        if (IsInPrintMode)
        {
            ContentPlaceHolder content = FindContent(Controls);
            if (content != null)
            {
                ExcludePrintControls(content.Controls);
            }
        }
    }

    /// <summary>
	/// Проверяет доступность контролов на странице и изменяет их состояние.
	/// </summary>
	/// <param name="controls">Коллекция контролов.</param>
	private void CheckControlsAccessibility( ControlCollection controls )
	{
		foreach (Control control in controls)
		{
			// если контрол скрыт, то нет смысла проверять его доступность.
			if(!control.Visible) continue;

			bool isAccessible = true;

			IAccessible accessControl = control as IAccessible;
			if(accessControl!=null)
			{
				isAccessible = accessControl.CheckAccessibilityToUser( Core.Security.User.Current );
			}

			// проверяем его дочерние контролы, только если контрол доступен
			if(isAccessible && control.HasControls())
			{
				CheckControlsAccessibility( control.Controls );
			}
		}
	}

	/// <summary>
	/// Находит и возвращает ContentPlaceHolder с контентом.
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
	/// Отключает все контролы, которые не лежат в специальных плейсолдерах PrintPlaceHolder.
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

	#region Общие свойства страницы

    /// <summary>
    /// Current user of system.
    /// </summary>
    public Person CurrentUser
    {
        get
        {
            return (Person)CurrentWorkingUser;
        }
    }

    /// <summary>
    /// Current user of system.
    /// </summary>
    public override object CurrentWorkingUser
    {
        get
        {
            if (Session["CurrentPerson"] == null)
                return null;

            Person curUser = Session["CurrentPerson"] as Person;
            return curUser;
        }
    }

	private string m_caption = null;
	/// <summary>
	/// Заголовок основного блока страницы. Выбирается из локальных ресурсов локализации (ключ Caption).
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
	/// URL страницы, с которой мы зашли на текущую, или null, если не задан.
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
	/// Подготавливает страницу для использования ReturnUrl.
	/// Для этого определяет адресс ссылающейся страницы и добавляет его в свой Url.
	/// ВНИМАНИЕ: Использовать только в методе Page_Load().
	/// </summary>
	public virtual void PrepareReturnUrl()
	{
		// если мы только что зашли на эту страницу, и у нас нет параметра ReturnUrl, 
		// то добавляем с Url параметр ReturlUrl и делаем редирект на себя.
		// причем если есть реферрер, то устанавливаем ReturnUrl на него, иначе на себя.
		if(ReturnUrl == null)
		{
			ReturnUrl = (Request.UrlReferrer != null)
				? HttpUtility.UrlEncode( Request.UrlReferrer.PathAndQuery )
				: HttpUtility.UrlEncode( Request.RawUrl );
		}
	}

	/// <summary>
	/// URL для возврата на предыдущую страницу.
	/// В случае, если параметр не указан, используется ReturnUrl.
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
	/// Доступен ли режим печати для страницы
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
	/// Находится ли страница в режиме печати
	/// </summary>
	public bool IsInPrintMode
	{
		get
		{
			return Request.Params["print_mode"] != null;
		}
	}

	#endregion

	#region Свойства и методы для работы с ошибками формы

	/// <summary>
	/// Ошибки формы, которые были зарегистрированы во время серверной отбработки введенных данных.
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
	/// Добавить сообщение об ошибке в список ошибок формы.
	/// </summary>
	/// <param name="message">Сообщение об ошибке.</param>
	/// <returns></returns>
	public void ReportError( string message )
	{
		Errors.Add( new Message( message, MessageType.Error ) );
	}

	#endregion

	#region Методы регистрации скриптов
	/// <summary>
	/// Регистрация подтверждения, возникаемого при клике на контроле.
	/// </summary>
	/// <param name="control">Контрол, клик на котором анализируется.</param>
	/// <param name="confirm">Текст подтверждения.</param>
	public void UnregisterClickEvents( WebControl control )
	{
		if(control == null) return;
		control.Attributes.Remove( "Onclick" );
	}
	/// <summary>
	/// Регистрация подтверждения, возникаемого при клике на контроле.
	/// </summary>
	/// <param name="control">Контрол, клик на котором анализируется.</param>
	/// <param name="confirm">Текст подтверждения.</param>
	public void RegisterConfirm( WebControl control, string confirm )
	{
		if(control == null) return;
		control.Attributes.Add( "Onclick", "return confirm('" + confirm + "');" );
	}
	/// <summary>
	/// Регистрация предупреждения, возникаемого при клике на контроле.
	/// </summary>
	/// <param name="control">Контрол, клик на котором анализируется.</param>
	/// <param name="alert">Текст предупреждения.</param>
	public void RegisterAlert( WebControl control, string alert )
	{
		if(control == null) return;
		control.Attributes.Add( "Onclick", "alert('" + alert + "'); return false;" );
	}

	/// <summary>
	///  Регистрация поддтверждения для операции группового редактирования.
	/// </summary>
	/// <param name="control">Контрол, клик на котором анализируется.</param>
	/// <param name="grid">Грид, для которого проводиться операция.</param>
	/// <param name="confirm">Текст предупреждения.{0} - место в которое необходимо подставить значение.</param>
	public void RegisterGroupConfirm(WebControl control, Control grid, string confirm)
	{
		if (control == null) return;
		control.Attributes.Add("Onclick", " var id = '" + grid.ClientID + "_totalSelected' ; var mess = '" + confirm + "';return confirm(mess.replace('{0}',document.getElementById(id).value));");
	}

	#endregion

	#region Методы для редиректов и работы с Url
	/// <summary>
	/// Производит редирект на исходную страницу, с которой мы зашли на данную.
	/// Сначала пытаемся перейти на страницу ReturnUrl. Если ReturnUrl не задан, пытаемся перейти на UrlReferrer.
	/// В случае неудачи, остаемся на этой же странице.
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
	/// Производит редирект на ту же страницу.
	/// </summary>
	public void RedirectToMySelf()
	{
		//Response.Redirect( Request.Url.PathAndQuery );
        Response.Redirect(Request.RawUrl);
	}

	/// <summary>
	/// Производит редирект на указанный Url. 
	/// Может быть как абсолютным (www.google.com/index.html), 
	/// так и относительным (~/UserManager/UserList.aspx).
	/// </summary>
	public void RedirectToUrl(string url)
	{
		Response.Redirect( url );
	}

	/// <summary>
	/// Производит редирект на указанный Url с передачей информации о текущей странице, на которую будет возвращать кнопка Back
	/// </summary>
	/// <param name="url"></param>
	public void RedirectToUrlWithReturn( string url )
	{
		Response.Redirect( WebHelper.ReplaceUrlQueryParameter( url, "ReturnURL", HttpUtility.UrlEncode( Request.RawUrl.ToString() ) ) );
	}

	#endregion

    /// <summary>
    /// Is current user in specific role.
    /// </summary>
    /// <param name="role">Role.</param>
    /// <returns>Is current user in specific role.</returns>
    public bool IsInRole(string role)
    {
        if (CurrentUser == null)
            return false;

        return CurrentUser.IsInRole(role);
    }
}

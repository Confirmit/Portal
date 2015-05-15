<%@ Application Language="C#" %>
<%@ Import Namespace="UlterSystems.PortalLib.BusinessObjects" %>

<script RunAt="server">
	
    protected void Application_Start(object sender, EventArgs e)
    {
        log4net.Config.XmlConfigurator.Configure();
        ConfirmIt.PortalLib.Logger.Logger.Instance.SplitLogFile = true;
        ConfirmIt.PortalLib.Logger.Logger.Instance.Info("PortalWeb ???????.");

        // Initialize DB connection.
        Core.DB.ConnectionManager.DefaultConnectionString =
            ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
        Core.DB.ConnectionManager.ConnectionTypeResolve += delegate { return Core.DB.ConnectionType.SQLServer; };

        // Specify procedure for storing current user culture.
        Core.MLText.PersistCurrentCultureID += delegate(string currentCultureID)
                                                   {
                                                       if (HttpContext.Current != null)
                                                       {
                                                           if (
                                                               HttpContext.Current.Response.Cookies["CurrentCultureID"] ==
                                                               null)
                                                               HttpContext.Current.Response.Cookies.Add(
                                                                   new HttpCookie("CurrentCultureID", currentCultureID));
                                                           else
                                                               HttpContext.Current.Response.Cookies["CurrentCultureID"].
                                                                   Value = currentCultureID;
                                                       }
                                                   };

        // Specify procedure for getting current user culture.
        Core.MLText.RequestCurrentCultureID += delegate()
                                                   {
                                                       if (HttpContext.Current == null)
                                                       {
                                                           return "en";
                                                       }
                                                       HttpCookie cookie =
                                                           HttpContext.Current.Request.Cookies["CurrentCultureID"];
                                                       return cookie != null ? cookie.Value : "en";
                                                   };

        //???????? ? ?????????? ???????????? ?? ????
        UlterSystems.PortalLib.BusinessObjects.OldDictionaries Dicts = new UlterSystems.PortalLib.BusinessObjects.OldDictionaries();
        //?????????? ???????????? ? ??????????
        Application["Dictionaries"] = Dicts;
        //???????? ? ?????????? ?????? ?????????????? ??????
        UlterSystems.PortalLib.BusinessObjects.Languages Langs = new UlterSystems.PortalLib.BusinessObjects.Languages();
        //?????????? ?????? ?????????????? ?????? ? ??????????
        Application["AvailableInterfaceLanguages"] = Langs;
    }

	protected void Session_Start( object sender, EventArgs e )
	{
		// Store information about current user.
		if (HttpContext.Current != null)
		{
            UlterSystems.PortalLib.BusinessObjects.Person.RequestUser = (
              () =>
                  (UlterSystems.PortalLib.BusinessObjects.Person)System.Threading.Thread.CurrentPrincipal
              );

			// update sl cookie expire date
			CookiesHelper.UpdateUseSLCookieExpireDate(5);
		}

            // Store information about users culture.
            /*if( HttpContext.Current != null )
            {
                // May be information was stored at users side.
                HttpCookie cookie = HttpContext.Current.Request.Cookies[ "CurrentCultureID" ];
                if( cookie == null )
                {
                    if( ( HttpContext.Current.Request.UserLanguages != null ) && ( HttpContext.Current.Request.UserLanguages.Length > 0 ) )
                    {
                        string currentCultureID = HttpContext.Current.Request.UserLanguages[ 0 ];
                        if( currentCultureID.Length > 2 )
                        { currentCultureID = currentCultureID.Substring( 0, 2 ); }
                        Core.MLText.CurrentCultureID = currentCultureID;

                        // Set culture for current thread.
                        SetThreadCulture();
                    }
                }
            }*/
        if (UlterSystems.PortalLib.BusinessObjects.Person.Current.IsAuthenticated)
            Core.MLText.CurrentCultureID = UlterSystems.PortalLib.BusinessObjects.Person.Current.PersonSettings.DefaultCulture;
		SetThreadCulture();
	}

    /// <summary>
    /// Sets culture of current thread according to current culture of MLText.
    /// </summary>
    protected static void SetThreadCulture()
    {
        try
        {
            var ci = new System.Globalization.CultureInfo(Core.MLText.CurrentCultureID);
            if (ci.IsNeutralCulture)
                ci = System.Globalization.CultureInfo.CreateSpecificCulture(ci.Name);

            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
        }
        catch (Exception ex)
        {
            ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
        }
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        // Set culture of current thread according to user preferences.
        SetThreadCulture();

        UlterSystems.PortalLib.BusinessObjects.Navigator.Redirect();
    }

    protected void Application_EndRequest(object sender, EventArgs e)
    { }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        if (HttpContext.Current.User == null)
            return;
        var currentUser = new UlterSystems.PortalLib.BusinessObjects.Person(HttpContext.Current.User.Identity);
        string domainName = HttpContext.Current.User.Identity.Name.ToLowerInvariant();
        if (!string.IsNullOrEmpty(domainName))
            if (!(currentUser.LoadByDomainName(domainName)))
            {
                currentUser = new UlterSystems.PortalLib.BusinessObjects.Person(System.Security.Principal.WindowsIdentity.GetAnonymous());
            }

        HttpContext.Current.User = currentUser;
    }

    protected void Application_Error(object sender, EventArgs e)
    {
        ConfirmIt.PortalLib.Logger.Logger.Instance.Error("В приложении PortalWeb произошла ошибка.");
        Exception ex = Server.GetLastError();
        while (ex != null)
        {
            ConfirmIt.PortalLib.Logger.Logger.Instance.Error(string.Empty, ex);

            // Redirect when potentially dengerous data is entered in portal forms.
            Type type = ex.GetType();
            if (type == typeof(HttpRequestValidationException))
                Response.Redirect("~/NewsTape/ValidationError.aspx");


            ex = ex.InnerException;
        }
    }

    protected void Session_End(object sender, EventArgs e)
    { }

    protected void Application_End(object sender, EventArgs e)
    {
        ConfirmIt.PortalLib.Logger.Logger.Instance.Info("PortalWeb о�?тановлен.");
    }
</script>

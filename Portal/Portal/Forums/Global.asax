<%@ Import Namespace="AspNetForums" %>

<script language="C#" runat="server">
    
    
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        log4net.Config.XmlConfigurator.Configure();
        UlterSystems.PortalLib.Logger.Log.Info("Общедоступный PortalWeb запущен.");

        //Инициализировать соединение с базой данных.
        Core.DB.ConnectionManager.ConnectionTypeResolve += new Core.DB.ConnectionManager.ConnectionTypeResolveCallback(ConnectionTypeResolver);
        Core.DB.ConnectionManager.DefaultConnectionString = ConfigurationManager.AppSettings["DBConnStr"];
    }

    /// <summary>
    /// Процедура привязки соединения к типу сервера.
    /// </summary>
    /// <param name="kind">Тип соединения.</param>
    /// <returns>Тип сервера.</returns>
    protected Core.DB.ConnectionType ConnectionTypeResolver(Core.DB.ConnectionKind kind)
    {
        return Core.DB.ConnectionType.SQLServer;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        UlterSystems.PortalLib.Logger.Log.Info("Общедоступный PortalWeb остановлен.");
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        UlterSystems.PortalLib.Logger.Log.Error("В общедоступном PortalWeb произошла ошибка.");
        Exception ex = Server.GetLastError();
        while (ex != null)
        {
            UlterSystems.PortalLib.Logger.Log.Error(string.Empty, ex);
            ex = ex.InnerException;
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Session.Remove("UserID");
    }


    /// <summary>
    /// Процедура аутентификации пользователя.
    /// </summary>
    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        /*
        UserRoles forumRoles = new UserRoles();
        forumRoles.GetUserRoles();
         */
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

</script>
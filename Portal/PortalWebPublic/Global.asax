<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
		 log4net.Config.XmlConfigurator.Configure();
		 UlterSystems.PortalLib.Logger.Log.Info("Общедоступный PortalWeb запущен.");

		 //Инициализировать соединение с базой данных.
		 Core.DB.ConnectionManager.ConnectionTypeResolve += new Core.DB.ConnectionManager.ConnectionTypeResolveCallback(ConnectionTypeResolver);
		 Core.DB.ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
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

             // Redirect when potentially dengerous data is entered in portal forms.
             Type type = ex.GetType();
             if (type == typeof(HttpRequestValidationException))
                 Response.Redirect("~/Secure/ValidationError.aspx");
             
			 ex = ex.InnerException;
		 }
 }

    void Session_Start(object sender, EventArgs e) 
    {
       // Code that runs when a new session is started
        if (User != null)
        {
            Session["UserID"] = UlterSystems.PortalLib.DB.DBManager.GetInternetUserID(User.Identity.Name);
        }
    }

    void Session_End(object sender, EventArgs e) 
    {
 		 // Code that runs when a session ends. 
		 // Note: The Session_End event is raised only when the sessionstate mode
		 // is set to InProc in the Web.config file. If session mode is set to StateServer 
		 // or SQLServer, the event is not raised.
		 Session.Remove("UserID");
    }
       
</script>

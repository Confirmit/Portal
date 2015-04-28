using System;
using System.Configuration;
using System.Web.Services;
using Core.DB;
using UlterSystems.PortalLib;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : WebService
{
	/*#region Fields
	private AuthHeader m_AuthHeader;
	#endregion

	#region Properties
	/// <summary>
	/// Header for authentication.
	/// </summary>
	public AuthHeader AuthenticationHeader
	{
		get { return m_AuthHeader; }
		set { m_AuthHeader = value; }
	}
	#endregion*/

	#region Конструкторы
	/// <summary>
	/// Конструктор.
	/// </summary>
	public Service () 
	{
		// Инициализировать логгер.
		log4net.Config.XmlConfigurator.Configure();
		Logger.Log.Info(String.Format(Resources.Strings.ServiceStarted, GetOfficeName()));

		// Инициализировать соединение с базой данных.
		ConnectionManager.ConnectionTypeResolve += new ConnectionManager.ConnectionTypeResolveCallback(ConnectionTypeResolver);
		ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;

		//Uncomment the following line if using designed components 
		//InitializeComponent();
	}
	#endregion

	#region Методы
	/// <summary>
	/// Процедура привязки соединения к типу сервера.
	/// </summary>
	/// <param name="kind">Тип соединения.</param>
	/// <returns>Тип сервера.</returns>
	protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
	{
		return ConnectionType.SQLServer;
	}
	#endregion

	//#region Web-методы
	/// <summary>
	/// Метод, возвращающий имя офиса.
	/// </summary>
	/// <returns>Имя офиса.</returns>
	[WebMethod(Description="Returns name of office.")]
	public string GetOfficeName()
	{
		string officeName = string.Empty;
		try
		{ officeName = ConfigurationManager.AppSettings["OfficeName"]; }
		catch (Exception ex)
		{ Logger.Log.Error(Resources.Strings.GetOfficeNameError, ex); }

		return officeName;
	}
/*
	/// <summary>
	/// Метод, возвращающий статусы пользователей.
	/// </summary>
	/// <returns>Статусы пользователей.</returns>
	[WebMethod(Description="Returns statuses of users.")]
	[SoapHeader("AuthenticationHeader")]
	public XMLSerializableUserStatusInfo[] GetUserStatuses()
	{
		if ((AuthenticationHeader.UserName != ConfigurationManager.AppSettings["UserName"])
			|| (AuthenticationHeader.Password != ConfigurationManager.AppSettings["Password"]))
		{ throw new HttpException(401, Resources.Strings.AuthenticationFail); }

		try
		{ 
			UserStatusInfo[] usInfos = UserList.GetStatusesList(DateTime.Today);
			if( usInfos == null )
				return null;

			List<XMLSerializableUserStatusInfo> coll = new List<XMLSerializableUserStatusInfo>();
			foreach (UserStatusInfo usInfo in usInfos)
			{
				XMLSerializableUserStatusInfo item = new XMLSerializableUserStatusInfo(usInfo);
				coll.Add(item);
			}

			return coll.ToArray();
		}
		catch( Exception ex )
		{
			Logger.Log.Error(Resources.Strings.GetUsersStatusesError, ex);
			return null;
		}
	}
	#endregion*/
}

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

using ConfirmIt.Portal.WcfServiceLibrary;
using ConfirmIt.Portal.WcfServiceLibrary.Resources;

using Core;
using Core.DB;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
	public class PortalService : AuthService, IPortalService
	{
		#region Конструкторы
		/// <summary>
		/// Конструктор.
		/// </summary>
		public PortalService()
		{
			log4net.Config.XmlConfigurator.Configure();
			Logger.Log.Info(String.Format(Strings.ServiceStarted, GetOfficeName()));

			// Инициализировать соединение с базой данных.
			ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
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

		#region Web-методы
		/// <summary>
		/// Метод, возвращающий имя офиса.
		/// </summary>
		/// <returns>Имя офиса.</returns>
		public string GetOfficeName()
		{
			if( !CheckAuthentication() )
				throw new HttpException(401, Strings.AuthenticationFail);

			string officeName = string.Empty;
			try
			{
				officeName = ConfigurationManager.AppSettings["OfficeName"];
			}
			catch (Exception ex)
			{
				Logger.Log.Error(Strings.GetOfficeNameError, ex);
			}

			return officeName;
		}

		/// <summary>
		/// Метод, возвращающий статусы пользователей.
		/// </summary>
		/// <returns>Статусы пользователей.</returns>
		public IEnumerable<XMLSerializableUserStatusInfo> GetUserStatuses()
		{
			if (!CheckAuthentication())
				throw new HttpException(401, Strings.AuthenticationFail);

			try
			{
				UserStatusInfo[] usInfos = UserList.GetStatusesList(DateTime.Today);

				if (usInfos == null)
					return null;

				List<XMLSerializableUserStatusInfo> coll = new List<XMLSerializableUserStatusInfo>();
				foreach (UserStatusInfo usInfo in usInfos)
				{
					XMLSerializableUserStatusInfo item = new XMLSerializableUserStatusInfo(usInfo);
					coll.Add(item);
				}

				return coll;
			}
			catch (Exception ex)
			{
				Logger.Log.Error(Strings.GetUsersStatusesError, ex);
				return null;
			}
		}

		#endregion
	}
}
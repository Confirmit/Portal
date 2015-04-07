using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Diagnostics;
using System.Web.Services.Description;

using ConfirmIt.PortalLib.Logger;

namespace UlterSystems.PortalLib.WebServiceSupport
{
	/// <summary>
	/// Класс-обертка для Web-сервиса, предоставляющего информацию о статусах пользователей.
	/// </summary>
	[WebServiceBinding(Name = "PulsarWebServiceSoap", Namespace = "http://tempuri.org/")]
	public class UsersStatusesService : SoapHttpClientProtocol
	{
	/*	#region Fields
		private AuthHeader m_authHeaderValue;
		#endregion

		#region Properties
		/// <summary>
		/// Value of authentication header.
		/// </summary>
		public AuthHeader AuthHeaderValue
		{
			get { return m_authHeaderValue; }
			set { m_authHeaderValue = value; }
		}
		#endregion*/

		#region Конструкторы
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceURL">URL Web-сервиса.</param>
		[DebuggerStepThrough]
		public UsersStatusesService(string serviceURL) : base()
		{
			if (string.IsNullOrEmpty(serviceURL))
				throw new ArgumentNullException("serviceURL", "URL Web-сервиса не задан.");

			this.Url = serviceURL;
		}
		#endregion

		#region Методы
		/// <summary>
		/// Метод, возвращающий имя офиса.
		/// </summary>
		/// <returns>Имя офиса.</returns>
		[DebuggerStepThrough]
		[SoapDocumentMethod("http://tempuri.org/GetOfficeName", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string GetOfficeName()
		{
			try
			{
				object[] res = this.Invoke("GetOfficeName", new object[0]);
				return (string) res[0];
			}
			catch( Exception ex )
			{
				Logger.Instance.Error("Ошибка при получении имени офиса.", ex);
				return null;
			}
		}

		/// <summary>
		/// Метод, возвращающий статусы пользователей.
		/// </summary>
		/// <returns>Статусы пользователей.</returns>
	/*	[DebuggerStepThrough]
		[SoapHeader("AuthHeaderValue")]
		[SoapDocumentMethod("http://tempuri.org/GetUserStatuses", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public XMLSerializableUserStatusInfo[] GetUserStatuses()
		{
			try
			{
				object[] res = this.Invoke("GetUserStatuses", new object[0]);
				return (XMLSerializableUserStatusInfo[])res[0];
			}
			catch (Exception ex)
			{
				Logger.Log.Error("Ошибка при возвращении статусов пользователей.", ex);
				return null;
			}
		}*/
		#endregion
	}
}

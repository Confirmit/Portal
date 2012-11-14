using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Core
{
	/// <summary>
	/// Вспомогательные методы.
	/// </summary>
	public static class WebHelper
	{
		#region Методы GetRequest()
		/// <summary>
		/// Возвращает результат http-запроса в виде строки.
		/// </summary>
		/// <param name="url">адрес запрашиваемой страницы.</param>
		/// <param name="cookie">коллекция cookie</param>
		/// <returns></returns>
		public static string GetRequest( string url, ref CookieCollection cookie )
		{
			return GetRequest( url, Encoding.GetEncoding( 1251 ), ref cookie );
		}

		/// <summary>
		/// Возвращает результат http-запроса в виде строки.
		/// </summary>
		/// <param name="url">адрес запрашиваемой страницы.</param>
		/// <param name="encoding">Кодировка для response.</param>
		/// <param name="cookie">коллекция cookie</param>
		/// <returns></returns>
		public static string GetRequest( string url, Encoding encoding, ref CookieCollection cookie )
		{
			HttpWebRequest httprequest;
			HttpWebResponse httpresponse;
			string bodytext = String.Empty;
			Stream responsestream;

			try
			{
				httprequest = (HttpWebRequest)WebRequest.Create( url );

				// прокси настраивается через system.net/defaultProxy
				// httprequest.Proxy = ...

				httprequest.ContentType = "application/x-www-form-urlencoded";
				httprequest.Method = "GET";
				httprequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/pdf, application/x-shockwave-flash, application/x-gsarcade-launch, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
				httprequest.ProtocolVersion = new Version( 1, 1 );
				httprequest.Timeout = (int)new TimeSpan( 0, 0, 30 ).TotalMilliseconds;
				httprequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";

				httprequest.CookieContainer = new CookieContainer();
				if(cookie != null)
				{
					httprequest.CookieContainer.Add( cookie );
				}

				httpresponse = (HttpWebResponse)httprequest.GetResponse();
				httpresponse.Cookies =
					httprequest.CookieContainer.GetCookies( httprequest.RequestUri );
				cookie = httpresponse.Cookies;

				responsestream = httpresponse.GetResponseStream();

				if(responsestream != null)
				{
					using(StreamReader bodyreader = new StreamReader( responsestream, encoding ))
					{
						bodytext = bodyreader.ReadToEnd();
					}
				}
				httpresponse.Close();
				httpresponse = null;
				httprequest = null;
			}
			catch
			{
				throw;
			}
			return bodytext;
		}

		/// <summary>
		/// Возвращает поток http-запроса к указанному ресурсу
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cookie"></param>
		/// <returns></returns>
		public static Stream GetRequestStream( Uri url, ref CookieCollection cookie )
		{
			HttpWebRequest httprequest;
			HttpWebResponse httpresponse;
			Stream responsestream = null;

			try
			{
				httprequest = (HttpWebRequest)WebRequest.Create( url );

				// прокси настраивается через system.net/defaultProxy
				// httprequest.Proxy = ...

				httprequest.ContentType = "application/x-www-form-urlencoded";
				httprequest.Method = "GET";
				httprequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/pdf, application/x-shockwave-flash, application/x-gsarcade-launch, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
				httprequest.ProtocolVersion = new Version( 1, 1 );
				httprequest.Timeout = (int)new TimeSpan( 0, 0, 30 ).TotalMilliseconds;
				httprequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";

				httprequest.CookieContainer = new CookieContainer();
				if(cookie != null)
				{
					httprequest.CookieContainer.Add( cookie );
				}

				httpresponse = (HttpWebResponse)httprequest.GetResponse();
				httpresponse.Cookies =
					httprequest.CookieContainer.GetCookies( httprequest.RequestUri );
				cookie = httpresponse.Cookies;

				responsestream = httpresponse.GetResponseStream();
			}
			catch
			{
				throw;
			}
			return responsestream;
		}

		#endregion

		#region Методы для работы с URL
		/// <summary>
		/// Заменяет/добавляет параметр к Url.
		/// </summary>
		/// <param name="url">Исходный Url.</param>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		/// <returns></returns>
		public static string ReplaceUrlQueryParameter( string url, string name, string value )
		{
			string tmp = url;

			string template = @"([\?]|[\&])" + name + "=[^&]*(&|$)";

			if(Regex.IsMatch( url, template, RegexOptions.IgnoreCase ))
			{
				tmp = Regex.Replace( tmp, template, "$1" + name + "=" + value + "$2", RegexOptions.IgnoreCase );
				tmp = tmp.TrimEnd( '&' );
			}
			else
			{
				tmp += (tmp.IndexOf( "?" ) > -1) ? "&" : "?";
				tmp += String.Format( "{0}={1}", name, value );
			}
			return tmp;
		}

		#endregion
	}
}

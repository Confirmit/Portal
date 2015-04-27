using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Core
{
	/// <summary>
	/// ��������������� ������.
	/// </summary>
	public static class WebHelper
	{
		#region ������ GetRequest()
		/// <summary>
		/// ���������� ��������� http-������� � ���� ������.
		/// </summary>
		/// <param name="url">����� ������������� ��������.</param>
		/// <param name="cookie">��������� cookie</param>
		/// <returns></returns>
		public static string GetRequest( string url, ref CookieCollection cookie )
		{
			return GetRequest( url, Encoding.GetEncoding( 1251 ), ref cookie );
		}

		/// <summary>
		/// ���������� ��������� http-������� � ���� ������.
		/// </summary>
		/// <param name="url">����� ������������� ��������.</param>
		/// <param name="encoding">��������� ��� response.</param>
		/// <param name="cookie">��������� cookie</param>
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

				// ������ ������������� ����� system.net/defaultProxy
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
		/// ���������� ����� http-������� � ���������� �������
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

				// ������ ������������� ����� system.net/defaultProxy
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

		#region ������ ��� ������ � URL
		/// <summary>
		/// ��������/��������� �������� � Url.
		/// </summary>
		/// <param name="url">�������� Url.</param>
		/// <param name="name">��� ���������.</param>
		/// <param name="value">�������� ���������.</param>
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

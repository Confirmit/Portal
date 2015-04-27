using System;
using System.Collections;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Web;

using UlterSystems.PortalLib.BusinessObjects;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// ����� ���������.
	/// </summary>
	public class Navigator
	{
		public static void Redirect()
		{
			HttpContext context = HttpContext.Current;
			string requestPath = context.Request.Path; 
			string appPath = context.Request.ApplicationPath; 
			string relativePath = requestPath.Substring(appPath.Length, requestPath.Length - appPath.Length); 
			string lang;
			Languages langs = (Languages) context.Application["AvailableInterfaceLanguages"];
			if( relativePath.IndexOf('/')==2)
			{
				lang=relativePath.Substring(0, 2);
				context.RewritePath(appPath + relativePath.Substring(3, relativePath.Length - 3)); 
			}
			else
			{
				//�������� ������� ����� �� ��������� �������� ������� ����� �������������� ������
				try
				{
					if (langs[context.Request.UserLanguages[0].Substring(0, 2)] != null)
						lang = context.Request.UserLanguages[0].Substring(0, 2);
					else
						lang = "ru";
				}
				catch
				{ lang = "ru"; }
			}
			//����� �������� ��� �������� ������ ������� ������������ 
			//Thread.CurrentThread.CurrentCulture=CultureInfo.CreateSpecificCulture(lang);

			//����� ��������, ������������ ResourceManager ��� ������ �������������� ��������
			//Thread.CurrentThread.CurrentUICulture=CultureInfo.CreateSpecificCulture(lang);

			//����� ��������� �������� ������ �� ������ ������������         
			//context.Response.ContentEncoding=Encoding.GetEncoding(Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage);
		}
	}
}

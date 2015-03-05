using System;
using System.Web;

public static class CookiesHelper
{
	#region [ Fields ]

	private static readonly string use_silverlight_cookie_name = "use_silverlight_main_control";

	#endregion

	#region [ Properties ]

	

	public static void UpdateUseSLCookieExpireDate(int days)
	{
		if (HttpContext.Current == null)
			return;

		// cookies support
		if (HttpContext.Current.Request.Cookies != null)
		{
			var reqCookie = HttpContext.Current.Request.Cookies[use_silverlight_cookie_name];
			if (reqCookie != null)
			{
				reqCookie.Expires = DateTime.Now.AddDays(days);
				HttpContext.Current.Response.Cookies.Add(reqCookie);
			}
		}
	}

	#endregion
}
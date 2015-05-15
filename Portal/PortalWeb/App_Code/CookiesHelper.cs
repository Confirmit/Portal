using System;
using System.Web;

public static class CookiesHelper
{
	#region [ Fields ]

	private static readonly string use_silverlight_cookie_name = "use_silverlight_main_control";

	#endregion

	#region [ Properties ]

	public static bool IsUsingSLControl
	{
		get
		{
			if (HttpContext.Current == null || !HttpContext.Current.Request.Browser.Cookies)
				return true;

			var cookie = HttpContext.Current.Request.Cookies.Get(use_silverlight_cookie_name);
			return cookie == null ? true : bool.Parse(cookie.Value);
		}
		set
		{
			if (HttpContext.Current == null || !HttpContext.Current.Request.Browser.Cookies)
				return;

			var useSLCookie = new HttpCookie(use_silverlight_cookie_name)
			                  	{
			                  		Value = value.ToString(),
			                  		Expires = DateTime.Now.AddDays(5)
			                  	};
			HttpContext.Current.Response.Cookies.Set(useSLCookie);
		}
	}

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
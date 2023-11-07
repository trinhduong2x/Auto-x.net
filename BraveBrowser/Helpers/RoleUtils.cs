using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BraveBrowser.Helpers
{
	public static class RoleUtils
	{
		public static bool IsAdmin()
		{
			bool isAdmin = false;
			var cookie = System.Web.HttpContext.Current.Request.Cookies["AdminLogin"];
			if (cookie != null)
				isAdmin = bool.Parse(CookieSecurityProvider.Unprotect(cookie.Values[Constants.COOKIE_FRONTEND_IS_ADMIN], Constants.COOKIE_FRONTEND_IS_ADMIN));

			return isAdmin;
		}
	}
}
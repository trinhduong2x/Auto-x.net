using BraveBrowser.Helpers;
using BraveBrowser.Logs;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BraveBrowser.Areas.Admin.Common
{
	public class AuthorizationAdmin : ActionFilterAttribute, IActionFilter
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var cookie = HttpContext.Current.Request.Cookies["AdminLogin"];
			if (cookie != null)
			{
				bool isAdmin = false;
				try
				{
					isAdmin = bool.Parse(CookieSecurityProvider.Unprotect(cookie.Values[Constants.COOKIE_FRONTEND_IS_ADMIN], Constants.COOKIE_FRONTEND_IS_ADMIN));
				}
				catch { }

				if (isAdmin == false)
				{
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
					{
						{"Controller", "Login" },
						{"Action", "Index" }
					});
					return;
				}
			}
			else
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
				{
					{"Controller", "Login" },
					{"Action", "Index" }
				});
				return;
			}
		}
	}
}
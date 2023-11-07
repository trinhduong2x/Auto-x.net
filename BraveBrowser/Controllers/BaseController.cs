using BraveBrowser.Helpers;
using BraveBrowser.Logs;
using System.Web.Mvc;
using System.Web.Routing;

namespace BraveBrowser.Controllers
{
	public class BaseController : Controller
    {
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			try
			{
				var cookie = Request.Cookies["AdminLogin"];
				if (cookie == null)
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
					   {
						   { "action", "Index" },
						   { "controller", "Login" },
						   { "Area", "Admin" }
					   });

				base.OnActionExecuting(filterContext);
			}
			catch (System.Exception ex)
			{
				Loggers.BraveMVCLog.Exception("Method: OnActionExecuting", ex);
			}
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			try
			{
				var cookie = Request.Cookies["AdminLogin"];
				if (cookie != null)
					ViewBag.Username = CookieSecurityProvider.Unprotect(cookie.Values[Constants.COOKIE_FRONTEND_KEY_EMAIL], Constants.COOKIE_FRONTEND_KEY_EMAIL);
				base.OnActionExecuted(filterContext);
			}
			catch (System.Exception ex)
			{
				Loggers.BraveMVCLog.Exception("Method: OnActionExecuted", ex);
			}
		}
	}
}
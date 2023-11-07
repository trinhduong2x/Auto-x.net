using BraveBrowser.Helpers;
using BraveBrowser.Logs;
using BraveBrowser.Models;
using log4net;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BraveBrowser
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			ControllerBuilder.Current.DefaultNamespaces.Add("BraveBrowser.Controllers");
			var _logFileMVC = Path.Combine(Server.MapPath("~"), "Logs/BraveMVCLog", "Brave_");
			GlobalContext.Properties["LogFileNameMVC"] = _logFileMVC;

			var _logFileAPI = Path.Combine(Server.MapPath("~"), "Logs/BraveAPILog", "Brave_");
			GlobalContext.Properties["LogFileNameAPI"] = _logFileAPI;

			log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(Server.MapPath("~"), "log4net.config")));
			Loggers.BraveMVCLog = new Log4netLogging("BraveMVCLog");
			Loggers.BraveAPILog = new Log4netLogging("BraveAPILog");
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			var newCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
			newCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
			newCulture.DateTimeFormat.DateSeparator = "/";
			Thread.CurrentThread.CurrentCulture = newCulture;
		}
	}
}

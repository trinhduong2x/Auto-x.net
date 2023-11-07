using System.Web.Mvc;
using BraveBrowser.Helpers;

namespace BraveBrowser.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var ip = NetworkUtil.GetIPAddress();
			ViewBag.NetworkInfo = $"{ip} - {NetworkUtil.GetClientMAC()}";
			return View();
		}
	}

    
}
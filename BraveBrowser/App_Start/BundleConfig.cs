using System.Web;
using System.Web.Optimization;

namespace BraveBrowser
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/lib/jquery/jquery.js",
						"~/lib/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.js",
						"~/lib/jquery-validate/jquery.validate.js",
						"~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/lib/twitter-bootstrap/js/bootstrap.bundle.js"));

			// js & css
			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/lib/twitter-bootstrap/css/bootstrap.css",
					  "~/lib/bootstrap-table/bootstrap-table.css",
					  "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/js").Include(
					  "~/lib/bootstrap-table/bootstrap-table.js"));
#if DEBUG
			BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
		}
	}
}
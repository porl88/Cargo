namespace Cargo
{
	using System.Web.Mvc;
	using System.Web.Routing;

	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			AddSeoOptimisation(routes);

			routes.MapRoute(
				name: "Feeds",
				url: "feed/{action}/{feed}",
				defaults: new {  controller = "Feed", action = "Index" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}

		private static void AddSeoOptimisation(RouteCollection routes)
		{
			// remove alternative paths for SEO purposes
			routes.IgnoreRoute("home");
			routes.IgnoreRoute("home/{action}");
			routes.IgnoreRoute("{controller}/index");

			// create all routes in lower case
			routes.LowercaseUrls = true;
		}
	}
}
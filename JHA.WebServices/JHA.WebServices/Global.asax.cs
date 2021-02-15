using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using JHA.WebServices.DependencyInjection;

namespace JHA.WebServices
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			// Configure dependency injection container.
			ConfigureContainer();

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		private void ConfigureContainer()
		{
			// Create the DI container.
			Container container = new Container();

			// Configure providers and repositories.
			DependencyInjection.InjectDependencies.ConfigureTiers(container);

			// Register concrete types.
			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

			// Set dependency resolver.
			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.BusinessLogic;
using JHA.WebServices.Repository.Interface;
using JHA.WebServices.Repository.SqlServer;
using JHA.WebServices.Repository.InMemory;

namespace JHA.WebServices.DependencyInjection
{
	/// <summary>
	/// Injects dependencies into the framework.
	/// </summary>
	public static class InjectDependencies
    {
		/// <summary>
		/// Configures the tiers/layers of our framework.  Here we provide the interfaces with concrete implementations.
		/// </summary>
		/// <remarks>
		/// We are using SimpleInjector here.  As the name implies, it is simple and in that regard it demands that the objects it deals with have
		/// only a single constructor.  Even though we do not show any constructor arguments in the Register() statements below, SimpleInjector
		/// will create new objects with the appropriate arguments.
		/// </remarks>
		/// <param name="container">The container.</param>
		public static void ConfigureTiers(Container container)
		{
			// Database dependencies

			// BusinessLogic Provider dependencies
			container.Register<JHA.WebServices.BusinessLogic.Interface.ITwitterProvider, JHA.WebServices.BusinessLogic.TwitterProvider>();

			// Repository dependencies
			container.Register<JHA.WebServices.Repository.Interface.ITwitterRepository, JHA.WebServices.Repository.InMemory.TwitterRepository>();
		}
	}
}
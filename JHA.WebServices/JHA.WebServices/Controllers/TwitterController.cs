using System.Threading.Tasks;
using System.Web.Http;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.Contract.Twitter;

namespace JHA.WebServices.Controllers
{
	/// <summary>
	/// The TwitterController.  Contains endpoints to:
	///     - create and consume a sample twitter stream
	///     - report various statistics from the captured stream
	/// </summary>
	/// <seealso cref="JHA.WebServices.Controllers.ControllerBase" />
	[RoutePrefix("api/twitter")]
    public class TwitterController : ControllerBase
    {
        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterController"/> class.
        /// </summary>
        /// <remarks>
        /// The class property, TwitterProvider, is injected with a concrete implementation
        /// by our DependencyInjection.InjectDependencies class.
        /// </remarks>
        /// <param name="provider">The provider.</param>
        public TwitterController(ITwitterProvider provider)
		{
            this.TwitterProvider = provider;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the twitter provider.
		/// </summary>
		public ITwitterProvider TwitterProvider { get; set; }

        #endregion

        #region Public Methods

        [Route("stream/start")]
        [HttpPost]
        // POST: api/Twitter
        public string Post([FromBody] TwitterConfiguration config)
        {
            var consumeTask = Task.Run(() => this.TwitterProvider.CreateAndConsumeStream(config));
            return "Twitter Client has been created and is consuming sampe tweet stream ...";
        }

        [Route("report")]
        [HttpGet]
        public TwitterStatistics ReportStatistics()
        {
            var stats = this.TwitterProvider.GetStatistics();
            return stats;
        }

        [Route("rules")]
        [HttpPost]
        public string PostRules(string filter)
        {
            return $"{filter} applied!";
        }

        #endregion
    }
}
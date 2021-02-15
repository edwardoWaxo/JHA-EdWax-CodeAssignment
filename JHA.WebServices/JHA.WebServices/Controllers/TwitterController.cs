using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.BusinessLogic;
using JHA.WebServices.Contract.Twitter;

namespace JHA.WebServices.Controllers
{
    [RoutePrefix("api/twitter")]
    public class TwitterController : ControllerBase
    {
		#region Constructor(s)

		/// <summary>
		/// Initializes a new instance of the <see cref="TwitterController"/> class.
		/// </summary>
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
        
        // GET: api/Twitter
        public string Get()
        {
            var consumeTask = Task.Run(() => this.TwitterProvider.CreateAndConsumeStream());
            return "Twitter Client has been created and is consuming sampe tweet stream ...";
        }

        [Route("{id}")]
        // GET: api/Twitter/5
        public string Get(int id)
        {
            return $"value {id}";
        }

        // POST: api/Twitter
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Twitter/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Twitter/5
        public void Delete(int id)
        {
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
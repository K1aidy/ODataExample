using System;
using System.Linq;
using Microsoft.AspNet.OData;
using ODataExample.Context;
using ODataExample.Context.Entity;
using Microsoft.AspNet.OData.Routing;

namespace ODataExample.Controllers
{
	[ODataRoutePrefix("Users")]
	public class UsersController : ODataController
	{
		private readonly ApiContext apiContext;

		public UsersController(ApiContext apiContext)
		{
			this.apiContext = apiContext ??
				throw new ArgumentNullException(nameof(apiContext));
		}

		[EnableQuery]
		public IQueryable<User> Get() =>
			apiContext.Users;
	}
}

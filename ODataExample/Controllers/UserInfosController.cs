using System;
using System.Linq;
using Microsoft.AspNet.OData;
using ODataExample.Context;
using ODataExample.Context.Entity;
using Microsoft.AspNet.OData.Routing;

namespace ODataExample.Controllers
{
	[ODataRoutePrefix("UserInfos")]
	public class UserInfosController : ODataController
	{
		private readonly ApiContext apiContext;

		public UserInfosController(ApiContext apiContext)
		{
			this.apiContext = apiContext ??
				throw new ArgumentNullException(nameof(apiContext));
		}

		[EnableQuery]
		public IQueryable<UserInfo> Get() =>
			apiContext.UserInfos;
	}
}

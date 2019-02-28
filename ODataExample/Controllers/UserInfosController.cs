using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODataExample.Context;
using ODataExample.Context.Entity;

namespace ODataExample.Controllers
{
	//[EnableQuery]
	public class UserInfosController : ODataController
	{
		private readonly ApiContext apiContext;

		public UserInfosController(ApiContext apiContext)
		{
			this.apiContext = apiContext ??
				throw new ArgumentNullException(nameof(apiContext));
		}

		[HttpGet]
		public IQueryable Get(ODataQueryOptions<UserInfo> options)
		{
			var userInfos = apiContext.UserInfos
				.Include(ui => ui.City);

			return options.ApplyTo(userInfos);
		}
	}
}

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
	public class UsersController : ODataController
	{
		private readonly ApiContext apiContext;

		public UsersController(ApiContext apiContext)
		{
			this.apiContext = apiContext ??
				throw new ArgumentNullException(nameof(apiContext));
		}

		[HttpGet]
		public IQueryable Get(ODataQueryOptions<User> options)
		{
			var users = apiContext.Users
				.Include(u => u.UserInfo)
					.ThenInclude(ui => ui.City);

			return options.ApplyTo(users);
		}
	}
}

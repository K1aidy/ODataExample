using Microsoft.EntityFrameworkCore;
using ODataExample.Context.Entity;

namespace ODataExample.Context
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }

		public DbSet<UserInfo> UserInfos { get; set; }

		public DbSet<City> Cities { get; set; }
	}
}

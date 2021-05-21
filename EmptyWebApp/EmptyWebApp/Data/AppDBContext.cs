using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmptyWebApp.Data
{
	public class AppDBContext : IdentityDbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{
		}
	}
}

using System;
using FirstProject.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FirstProject.Areas.Identity.IdentityHostingStartup))]
namespace FirstProject.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
			builder.ConfigureServices((context, services) =>
			{
				services.AddDbContext<FirstProjectContext>(options =>
					options.UseSqlServer(
						context.Configuration.GetConnectionString("FirstProjectContextConnection")));

				services.AddDefaultIdentity<IdentityUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.SignIn.RequireConfirmedAccount = false;
				})
					.AddEntityFrameworkStores<FirstProjectContext>();
			});
		}
    }
}
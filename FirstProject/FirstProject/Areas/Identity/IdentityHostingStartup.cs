using System;
using FirstProject.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FirstProject.Models;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

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

				var _context = services.BuildServiceProvider()
					   .GetService<FirstProjectContext>();
				var locales = _context.Locales.ToListAsync();
				string defaulLocaleCode = "en";
				var supportedCultures = new List<CultureInfo>();

				foreach (var locale in locales.Result)
				{
					if (locale.Enabled)
					{
						supportedCultures.Add(new CultureInfo(locale.LocaleCode));
						if (locale.IsDefault)
						{
							defaulLocaleCode = locale.LocaleCode;
						}
					}
				}

				services.Configure<RequestLocalizationOptions>(options =>
				{
					options.DefaultRequestCulture = new RequestCulture(defaulLocaleCode);
					options.SupportedCultures = supportedCultures;
					options.SupportedUICultures = supportedCultures;
					options.RequestCultureProviders = new List<IRequestCultureProvider>
					{
						new QueryStringRequestCultureProvider(),
						new CookieRequestCultureProvider()
					};
				});

				services.AddDefaultIdentity<ExtendedUserModel>(options =>
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.SignIn.RequireConfirmedAccount = false;
					options.Password.RequireDigit = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireLowercase = false;
				})
					.AddRoles<IdentityRole<System.Guid>>()
					.AddEntityFrameworkStores<FirstProjectContext>()
					.AddDefaultTokenProviders();
			});
		}
    }
}
using FirstProject.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using FirstProject.SignalRHubs;
using FirstProject.Services;

namespace FirstProject
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSignalR();
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

			services.AddRazorPages();
			services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
			{
				microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
				microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
				microsoftOptions.CallbackPath = "/signin-microsoft";
			});

			services.AddScoped<PollsService>();
			services.AddScoped<RollesService>();
			services.AddScoped<ChatService>();
			services.AddScoped<PrivateInfoService>();
			services.AddScoped<CultureService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FirstProjectContext context)
		{
			//ChatService chat = new ChatService();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
				app.UseForwardedHeaders();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseForwardedHeaders();
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseRequestLocalization(app.ApplicationServices
				.GetRequiredService<IOptions<RequestLocalizationOptions>>()
				.Value);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				endpoints.MapHub<UpdateHub>("/update");
				endpoints.MapHub<ChatHub>("/chat");
			});
		}
	}
}

using FirstProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly SignInManager<IdentityUser> _signInManager;
		public HomeController(SignInManager<IdentityUser> signInManager,
			ILogger<HomeController> logger,
			UserManager<IdentityUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public async Task<IActionResult> PrivateInfo()
		{
			var user = await _userManager.GetUserAsync(User);
			var claims = User.Claims;
			PrivateInfoModel model = new PrivateInfoModel(user);
			if (user != null)
			{

				model.currentLogins = await _userManager.GetLoginsAsync(user);
				foreach (var claim in claims)
				{
					model.claims.Add(new PrivateInfoModel.OutputFormatClaims
					{
						Name = claim.Subject.Name,
						Issuer = claim.Issuer,
						Type = claim.Type.ToString().Substring(claim.Type.ToString().LastIndexOfAny(new char[] { '/', '.' }) + 1),
						Value = claim.Value
					});
				}
				model.IsAuthenticated = true;
			}
			else
			{
				model.IsAuthenticated = false;
			}
			return View(model);
		}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

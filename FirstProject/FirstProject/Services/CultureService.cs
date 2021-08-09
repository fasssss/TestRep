using FirstProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using FirstProject.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Services
{
	public class CultureService : Services
	{
		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly SignInManager<ExtendedUserModel> _signInManager;
		private readonly RoleManager<IdentityRole<System.Guid>> _roleManager;
		private readonly IWebHostEnvironment _appEnvironment;

		public CultureService(FirstProjectContext context,
			SignInManager<ExtendedUserModel> signInManager,
			UserManager<ExtendedUserModel> userManager,
			RoleManager<IdentityRole<System.Guid>> roleManager,
			IWebHostEnvironment appEnvironment) : base(context)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_appEnvironment = appEnvironment;
		}

		public async Task<string> Culture(string culture, ClaimsPrincipal User)
		{
			var cultureId = int.Parse(culture);
			var user = await _userManager.GetUserAsync(User);
			var oldClaim = new Claim("CultureID", user.LocaleID.ToString());
			var newClaim = new Claim("CultureID", culture.ToString());
			user.LocaleID = cultureId;
			_ = await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
			_ = await _userManager.UpdateAsync(user);
			await _signInManager.RefreshSignInAsync(user);
			user.LocaleModel = await _context.Locales.FindAsync(cultureId);
			return user.LocaleModel.LocaleCode;
		}
	}
}

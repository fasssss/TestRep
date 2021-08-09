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
	public class PrivateInfoService : Services
	{
		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly SignInManager<ExtendedUserModel> _signInManager;
		private readonly RoleManager<IdentityRole<System.Guid>> _roleManager;
		private readonly IWebHostEnvironment _appEnvironment;

		public PrivateInfoService(FirstProjectContext context,
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

		public async Task<PrivateInfoModel> PrivateInfo(ClaimsPrincipal User)
		{
			var user = await _userManager.GetUserAsync(User);
			var claims = User.Claims;
			PrivateInfoModel model = new PrivateInfoModel(user);
			if (user != null)
			{
				model.CurrentLogins = await _userManager.GetLoginsAsync(user);
				foreach (var claim in claims)
				{
					model.Claims.Add(new PrivateInfoModel.OutputFormatClaims
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

			return model;
		}
	}
}

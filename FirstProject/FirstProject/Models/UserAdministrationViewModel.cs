using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class UserAdministrationViewModel
	{
		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public UserAdministrationViewModel(UserManager<ExtendedUserModel> userManager)
		{
			_userManager = userManager;
			var userList = userManager.Users.ToList();
			UserList = new List<UserPresentation>();
			foreach (var user in userList)
			{
				var role = GetRole(user).Result;
				var date = GetDate(user).Result;
				UserList.Add(new UserPresentation(user.ToString(), role.First().ToString(), date));
			}
		}

		public UserAdministrationViewModel(UserManager<ExtendedUserModel> userManager,
			RoleManager<IdentityRole> roleManager) : this(userManager)
		{
			_roleManager = roleManager;
			RoleList = roleManager.Roles.ToList();
			RoleList.Add(new IdentityRole("Guest"));
		}

		private async Task<IList<string>> GetRole(ExtendedUserModel user)
		{
			var a = await _userManager.GetRolesAsync(user);
			if (a.Count == 0)
			{
				a.Add("Guest");
			}

			return a;
		}

		private async Task<string> GetDate(ExtendedUserModel user)
		{
			var claims = await _userManager.GetClaimsAsync(user);
			string date = "---";
			foreach (var claim in claims)
			{
				if (claim.Type == "RegisterDate")
				{
					date = claim.Value;
				}
			}
			return date;
		}

		public List<UserPresentation> UserList { get; set; }
		public List<IdentityRole> RoleList { get; set; }

		public class UserPresentation
		{
			public string UserName { get; set; }
			public string Role { get; set; }
			public string RegistrationDate { get; set; }

			public UserPresentation(string userName = "Anonom", string role = "Guest", string registrationDate = "---")
			{
				UserName = userName;
				Role = role;
				RegistrationDate = registrationDate;
			}

			public override string ToString()
			{
				return ("UserName: " + UserName + ", Role: " + Role + ", Register Date: " + RegistrationDate);
			}
		}
	}
}

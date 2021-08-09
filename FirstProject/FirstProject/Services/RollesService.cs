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
using FirstProject.Data;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FirstProject.Services;

namespace FirstProject.Services
{
	public class RollesService : Services
	{

		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly SignInManager<ExtendedUserModel> _signInManager;
		private readonly RoleManager<IdentityRole<System.Guid>> _roleManager;
		private readonly IWebHostEnvironment _appEnvironment;
		public RollesService(SignInManager<ExtendedUserModel> signInManager,
			UserManager<ExtendedUserModel> userManager,
			FirstProjectContext context,
			RoleManager<IdentityRole<System.Guid>> roleManager,
			IWebHostEnvironment appEnvironment) : base(context)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_appEnvironment = appEnvironment;
		}

		public bool IsUserUnderConsideration(IdentityUser<System.Guid> user)
		{
			if (_context.Files.ToList().Find(x => x.UserID == user.Id) != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public List<IdentityUser<System.Guid>> GetReperesentativeAuthority(IdentityUser<System.Guid> user)
		{
			return _context.AuthorityDependencies.Where(x => x.AuthrityId == user.Id)
				.Select(x => x.RepresentativeAuthorityModel).Cast<IdentityUser<System.Guid>>().ToList();
		}

		public List<FileModel> GetFilesList()
		{
			return _context.Files.ToList().Select(x => x).ToList();
		}

		public async Task AdministratorsModeration(string userName, string role, string action = "Role Update")
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user != null)
			{
				if (action == "Delete")
				{
					_ = await _userManager.DeleteAsync(user);
					_context.AuthorityDependencies.Remove(_context.AuthorityDependencies
						.ToList().Find(x => x.RepresentativeAuthrityId == user.Id));
					_context.SaveChanges();
				}
				else
				{
					var currentRole = await _userManager.GetRolesAsync(user);
					_ = await _userManager.RemoveFromRolesAsync(user, currentRole);
					if (role != "Guest")
					{
						_ = await _userManager.AddToRoleAsync(user, role);
					}
				}
			}
		}

		public async Task RepresentativeAuthorityModeration(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user != null)
			{
				await _signInManager.RefreshSignInAsync(user);
			}
		}

		public async Task AuthorityModeration(string userName, string action, ClaimsPrincipal User)
		{
			var userRepresentativeAuthority = await _userManager.FindByNameAsync(userName);
			var userAuthority = await _userManager.GetUserAsync(User);
			if (userRepresentativeAuthority != null && userAuthority != null)
			{
				if (action == "Rely")
				{
					_context.AuthorityDependencies.Add(
						new AuthorityDependenciesModel { RepresentativeAuthrityId = userRepresentativeAuthority.Id, AuthrityId = userAuthority.Id });
					_context.SaveChanges();
				}

				if (action == "StopRely")
				{

					_context.AuthorityDependencies.Remove(_context.AuthorityDependencies.Find(userAuthority.Id, userRepresentativeAuthority.Id));
					_context.SaveChanges();
				}
				await _signInManager.RefreshSignInAsync(userAuthority);
			}
		}

		public async Task LeadManagersModeration(string userName, string role, string authority)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user != null)
			{
				if (role == "Authority" || role == "RepresentativeAuthority")
				{
					var currentRole = await _userManager.GetRolesAsync(user);
					_ = await _userManager.RemoveFromRolesAsync(user, currentRole);
					_ = await _userManager.AddToRoleAsync(user, role);
				}
			}
		}

		public async Task AddFile(ClaimsPrincipal User, IFormFile uploadedFile, string path = null)
		{
			var user = await _userManager.GetUserAsync(User);
			if (path != null)
			{
				if (user != null)
				{
					if (uploadedFile != null && path != null)
					{
						path = path + User.Identity.Name + "/";
						if (!Directory.Exists(_appEnvironment.WebRootPath + path + User.Identity.Name + "/"))
						{
							Directory.CreateDirectory(_appEnvironment.WebRootPath + path + User.Identity.Name + "/");
						}

						path += uploadedFile.FileName;
						FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path, UserID = user.Id };
						_context.Files.Add(file);
						_context.SaveChanges();
						using (var fileStream = new FileStream(_appEnvironment.WebRootPath + _context.Files.ToList().Last().Path, FileMode.Create))
						{
							await uploadedFile.CopyToAsync(fileStream);
						}
					}
				}
			}
		}

		public (string filePath, string contentType, string fileName) InspectingUserFiles(string fileName, string path)
		{
			var requestedFile = _context.Files.Where(x => x.Name == fileName && x.Path == (path + fileName)).Select(x => x).ToList();
			string filePath = _appEnvironment.WebRootPath + requestedFile.First().Path;
			string contentType;
			if (new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType) == false)
			{
				throw (new Exception("File extention was not recognized"));
			}

			return (filePath, contentType, fileName);
		}
	}
}

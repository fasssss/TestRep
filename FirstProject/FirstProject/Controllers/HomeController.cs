﻿using FirstProject.Models;
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

namespace FirstProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly SignInManager<ExtendedUserModel> _signInManager;
		private readonly FirstProjectContext _context;
		private readonly IHtmlLocalizer<HomeController> _localizer;
		private readonly RoleManager<IdentityRole<System.Guid>> _roleManager;
		private readonly IWebHostEnvironment _appEnvironment;

		public HomeController(SignInManager<ExtendedUserModel> signInManager,
			ILogger<HomeController> logger,
			UserManager<ExtendedUserModel> userManager,
			FirstProjectContext context,
			IHtmlLocalizer<HomeController> localizer,
			RoleManager<IdentityRole<System.Guid>> roleManager,
			IWebHostEnvironment appEnvironment)
		{
			_roleManager = roleManager;
			_localizer = localizer;
			_context = context;
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
			_appEnvironment = appEnvironment;
		}

		public async Task<IActionResult> Voting(int questionId, int voteType)
		{
			var user = await _userManager.GetUserAsync(User);
			if (_context.Votes.Find(questionId, user.Id) != null)
			{
				_context.Votes.Remove(_context.Votes.Find(questionId, user.Id));
			}
			_context.Votes.Add(new VoteModel { UserId = user.Id, QuestionId = questionId, VoteTypeId = voteType });
			_context.SaveChanges();
			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Poll(int pollId)
		{
			int Id = pollId;
			var user = await _userManager.GetUserAsync(User);
			return View(new PollesViewModel(_context, user, pollId));
		}

		public IActionResult PollesList()
		{
			return View(new PollesViewModel(_context));
		}

		public IActionResult PollsManagement()
		{
			return View(new PollesViewModel(_context));
		}

		[HttpPost]
		public async Task<IActionResult> PollManagement(string action, int? pollId = null, string newPollDescription = null)
		{
			var user = await _userManager.GetUserAsync(User);
			if (action == "delete")
			{
				_context.Polles.Remove(_context.Polles.Find(pollId));
				_context.SaveChanges();
			}

			if (action == "change")
			{
				return View("PollAddingChangingPage", new PollesViewModel(_context, pollId));
			}

			if (action == "add")
			{
				int statusId = _context.StatusTypes.Where(x => x.StatusName == "Opend").Select(x => x.Id).ToList().First();  // FIND BETTER SOLUTION
				_context.Polles.Add(new PolleModel { Description = newPollDescription, StatusId = statusId });
				await _context.SaveChangesAsync();
				return View("PollAddingChangingPage", new PollesViewModel(_context));
			}

			return View("PollsManagement", new PollesViewModel(_context));
		}

		public IActionResult PollSaveChanges(int? pollId, ICollection<string> questionsText
			, string action, ICollection<int> questionsId = null)
		{
			var questionIdList = questionsId.ToList();
			var questionTextList = questionsText.ToList();
			if (action == "change")
			{
				for (int i = 0; i < questionsId.Count; i++)
				{
					if (string.IsNullOrEmpty(questionTextList[i]))
					{
						if (_context.Questions.Find(questionIdList[i]).FileId != null)
						{
							_context.FilesInDb.Remove(
								_context.FilesInDb.Find(
									_context.Questions.Find(questionIdList[i]).FileId));
						}

						_context.Questions.Remove(_context.Questions.Find(questionIdList[i]));
					}

					if (_context.Questions.Find(questionIdList[i]) != null)
					{
						_context.Questions.Find(questionIdList[i]).Question = questionTextList[i];
					}
				}
			}

			if (pollId != null && action == "add")
			{
				_context.Questions.Add(new QuestionModel { PolleId = (int)pollId, Question = questionTextList[0]});
			}

			_context.SaveChanges();
			return View("PollAddingChangingPage", new PollesViewModel(_context, pollId));
		}

		public async Task<IActionResult> RoleCapabilities()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user != null)
			{
				var role = await _userManager.GetRolesAsync(user);
				if (role.Count > 0)
				{
					switch (role.First<string>())
					{
						case "Administrator":
							{
								return View("Administrator", new UserAdministrationViewModel(_userManager, _roleManager));
							}
						case "Authority":
							{
								var representativeAuthorityModel = _context.AuthorityDependencies.Where(x => x.AuthrityId == user.Id)
									.Select(x => x.RepresentativeAuthorityModel).ToList();
								if (representativeAuthorityModel.Count > 0)
								{
									TempData["AppliedRepresentativeAuthority"] = representativeAuthorityModel.Last().UserName;
								}
								return View("Authority", new UserAdministrationViewModel(_userManager, _roleManager));
							}
						case "LeadManager":
							{
								TempData["Files"] = _context.Files.ToList().Select(x => x).ToList();
								return View("LeadManager", new UserAdministrationViewModel(_userManager, _roleManager));
							}
						case "RepresentativeAuthority":
							{
								return View("RepresentativeAuthority", new UserAdministrationViewModel(_userManager, _roleManager));
							}
					}
				}
				if (_context.Files.ToList().Find(x => x.UserID == user.Id) != null)
				{
					TempData["request"] = "already under consideration";
				}
				else
				{
					TempData["request"] = "new request";
				}
			}
			return View("Guest");
		}

		[HttpPost]
		public async Task<IActionResult> AdministratorsModeration(string userName, string role, string action = "Role Update")
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
					return RedirectToAction("RoleCapabilities");
				}

				var currentRole = await _userManager.GetRolesAsync(user);
				_ = await _userManager.RemoveFromRolesAsync(user, currentRole);
				if (role != "Guest")
				{
					_ = await _userManager.AddToRoleAsync(user, role);
				}
			}
			return RedirectToAction("RoleCapabilities");
		}

		[HttpPost]
		public async Task<IActionResult> RepresentativeAuthorityModeration(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user != null)
			{
				await _signInManager.RefreshSignInAsync(user);
			}
			return RedirectToAction("RoleCapabilities");
		}
		[HttpPost]
		public async Task<IActionResult> AuthorityModeration(string userName, string action)
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
			return RedirectToAction("RoleCapabilities");
		}

		[HttpPost]
		public async Task<IActionResult> LeadManagersModeration(string userName, string role, string authority)
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
			return RedirectToAction("RoleCapabilities");
		}

		[HttpPost]
		public IActionResult AddFileToDB(IFormFile uploadedFile, int questionId)
		{
			if (_context.Questions.Find(questionId).FileId != null)
			{
				_context.FilesInDb.Remove(_context.FilesInDb.Find(_context.Questions.Find(questionId).FileId));
				_context.Questions.Find(questionId).FileId = null;
			}

			using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
			{
				byte[] byteFile = binaryReader.ReadBytes((int)uploadedFile.Length);
				_context.FilesInDb.Add(
					new FileInDbModel {
						File = byteFile
						, ContentType = uploadedFile.ContentType
						, FileName = uploadedFile.FileName
						, QuestionId = questionId});

				_context.SaveChanges();
				int id = _context.FilesInDb.Where(x => x.File == byteFile).Select(x => x).First().Id;
				_context.Questions.Find(questionId).FileId = id;
				_context.SaveChanges();
			}

			return View("PollAddingChangingPage", new PollesViewModel(_context, _context.Questions.Find(questionId).PolleId));
		}

		[HttpPost]
		public async Task<IActionResult> AddFile(IFormFile uploadedFile, string path = null)
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

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult InspectingUserFilesInDB(int questionId)
		{
			QuestionModel question = _context.Questions.Find(questionId);
			_context.FilesInDb.Where(x => x.Id == question.FileId).Load();
			byte[] fileInBytes = question.File.File;
			string fileType = question.File.ContentType;
			string fileName = question.File.FileName;
			return File(fileInBytes, fileType, fileName);
		}

		[HttpPost]
		public IActionResult InspectingUserFiles(string fileName, string path)
		{
			var requestedFile = _context.Files.Where(x => x.Name == fileName && x.Path == (path + fileName)).Select(x => x).ToList();
			string filePath = _appEnvironment.WebRootPath + requestedFile.First().Path;
			string contentType;
			if (new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType) == false)
			{
				throw (new Exception("File extention was not recognized"));
			}

			return PhysicalFile(filePath, contentType, fileName);
		}

		public IActionResult Index()
		{
			ViewData["Welcome"] = _localizer["Welcome"];
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		private void PrivateInfoCulture()
		{
			ViewData["Value"] = _localizer["Value"];
			ViewData["NONE"] = _localizer["NONE"];
			ViewData["Authenticated User"] = _localizer["Authenticated User"];
			ViewData["Username"] = _localizer["Username"];
			ViewData["Email"] = _localizer["Email"];
			ViewData["Phone number"] = _localizer["Phone number"];
			ViewData["Private Information"] = _localizer["Private Information"];
			ViewData["External login providers"] = _localizer["External login providers"];
			ViewData["Claims"] = _localizer["Claims"];
			ViewData["Subject"] = _localizer["Subject"];
			ViewData["Issuer"] = _localizer["Issuer"];
			ViewData["Type"] = _localizer["Type"];
		}
		public async Task<IActionResult> PrivateInfo()
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

			PrivateInfoCulture();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Culture(string culture)
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
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(user.LocaleModel.LocaleCode)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
				);
			return RedirectToAction(nameof(Index));
		}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

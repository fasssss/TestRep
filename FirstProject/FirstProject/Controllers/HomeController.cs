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
using FirstProject.Data;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using FirstProject.Services;

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
		private readonly ChatService _chatService;
		private readonly RollesService _rollesService;
		private readonly PollsService _pollsService;
		private readonly PrivateInfoService _privateInfoService;
		private readonly CultureService _cultureService;


		public HomeController(SignInManager<ExtendedUserModel> signInManager,
			ILogger<HomeController> logger,
			UserManager<ExtendedUserModel> userManager,
			FirstProjectContext context,
			IHtmlLocalizer<HomeController> localizer,
			RoleManager<IdentityRole<System.Guid>> roleManager,
			IWebHostEnvironment appEnvironment,
			ChatService chatService,
			PollsService pollsService,
			RollesService rollesService,
			PrivateInfoService privateInfoService,
			CultureService cultureService)
		{
			_roleManager = roleManager;
			_localizer = localizer;
			_context = context;
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
			_appEnvironment = appEnvironment;
			_chatService = chatService;
			_pollsService = pollsService;
			_rollesService = rollesService;
			_privateInfoService = privateInfoService;
			_cultureService = cultureService;
		}


		public async Task<IActionResult> Voting(int questionId, int voteType)
		{
			QuestionViewModel question = await _pollsService.Voting(questionId, voteType, User);
			return PartialView("QuestionPartial", question);
		}

		public async Task<IActionResult> Poll(int pollId)
		{
			if (_pollsService.IsPollInProgress(pollId))
			{
				return await OpenedPollAsync(pollId);
			}

			return PollSummarizing(pollId);
		}

		public async Task<IActionResult> OpenedPollAsync(int pollId)
		{
			var user = await _userManager.GetUserAsync(User);
			return View("Poll", new PollesViewModel(_context, user, pollId));
		}

		public IActionResult PollSummarizing(int pollId)
		{
			return View("PollSummarizing", new SummarizingViewModel(_context, pollId));
		}

		[HttpPost]
		public IActionResult History()
		{
			return View(_pollsService.History());
		}

		[HttpPost]
		public IActionResult HistoryCleaner()
		{
			_pollsService.HistoryCleaner();
			return View("History", _pollsService.History());
		}

		public IActionResult PollesList()
		{
			return View(new PollesViewModel(_context));
		}

		public IActionResult PollsManagement()
		{
			return View(new PollesViewModel(_context));
		}

		public async Task<IActionResult> PollChangeState(int statusTypeId, int pollId)
		{
			PollesViewModel poll = await _pollsService.PollChangeState(statusTypeId, pollId);
			return View("PollsManagement", poll);
		}
		[HttpPost]
		public async Task<IActionResult> PollManagement(string action, int? pollId = null, string newPollDescription = null)
		{
			var user = await _userManager.GetUserAsync(User);
			if (action == "delete" && pollId != null)
			{
				_pollsService.PollDelete((int)pollId);
			}

			if (action == "change" && pollId != null)
			{
				return View("PollAddingChangingPage", new PollesViewModel(_context, pollId));
			}

			if (action == "changeState" && pollId != null)
			{
				return View("PollSummarizing", new SummarizingViewModel(_context, (int)pollId));
			}

			if (action == "add")
			{
				int newPollId = _pollsService.PollAdd(newPollDescription);
				return View("PollAddingChangingPage", new PollesViewModel(_context, newPollId));
			}

			return View("PollsManagement", new PollesViewModel(_context));
		}

		public IActionResult PollSaveChanges(int? pollId, ICollection<string> questionsText
			, string action, ICollection<int> questionsId = null
			, ICollection<IFormFile> uploadedFiles = null
			, ICollection<int> questionIdForFiles = null)
		{
			_pollsService.PollSaveChanges(pollId, questionsText, action, questionsId, uploadedFiles, questionIdForFiles);
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
								var representativeAuthorityModel = _rollesService.GetReperesentativeAuthority(user);
								if (representativeAuthorityModel.Count > 0)
								{
									TempData["AppliedRepresentativeAuthority"] = representativeAuthorityModel.Last().UserName;
								}
								return View("Authority", new UserAdministrationViewModel(_userManager, _roleManager));
							}
						case "LeadManager":
							{
								TempData["Files"] = _rollesService.GetFilesList();
								return View("LeadManager", new UserAdministrationViewModel(_userManager, _roleManager));
							}
						case "RepresentativeAuthority":
							{
								return View("RepresentativeAuthority", new UserAdministrationViewModel(_userManager, _roleManager, _context));
							}
					}
				}
				if (_rollesService.IsUserUnderConsideration(user))
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
			await _rollesService.AdministratorsModeration(userName, role, action);
			return RedirectToAction("RoleCapabilities");
		}

		[HttpPost]
		public async Task<IActionResult> RepresentativeAuthorityModeration(string userName)
		{
			await _rollesService.RepresentativeAuthorityModeration(userName);
			return RedirectToAction("RoleCapabilities");
		}
		[HttpPost]
		public async Task<IActionResult> AuthorityModeration(string userName, string action)
		{
			await _rollesService.AuthorityModeration(userName, action, User);
			return RedirectToAction("RoleCapabilities");
		}

		[HttpPost]
		public async Task<IActionResult> LeadManagersModeration(string userName, string role, string authority)
		{
			await _rollesService.LeadManagersModeration(userName, role, authority);
			return RedirectToAction("RoleCapabilities");
		}

		[HttpPost]
		public async Task<IActionResult> AddFile(IFormFile uploadedFile, string path = null)
		{
			await _rollesService.AddFile(User, uploadedFile, path);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult InspectingUserFilesInDB(int questionId)
		{
			var fileProperties = _pollsService.GetQuestionWithFilesLoaded(questionId);
			return File(fileProperties.fileInBytes, fileProperties.fileType, fileProperties.fileName);
		}

		[HttpPost]
		public IActionResult InspectingUserFiles(string fileName, string path)
		{
			var fileProperties = _rollesService.InspectingUserFiles(fileName, path);
			return PhysicalFile(fileProperties.filePath, fileProperties.contentType, fileName);
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
			var model = await _privateInfoService.PrivateInfo(User);
			PrivateInfoCulture();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Culture(string culture)
		{
			var localeCode = await _cultureService.Culture(culture, User);
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(localeCode)),
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

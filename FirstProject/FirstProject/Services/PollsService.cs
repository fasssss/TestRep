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
	public class PollsService : Services
	{
		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly SignInManager<ExtendedUserModel> _signInManager;
		private readonly RoleManager<IdentityRole<System.Guid>> _roleManager;
		private readonly IWebHostEnvironment _appEnvironment;
		public PollsService(SignInManager<ExtendedUserModel> signInManager,
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

		public async Task<QuestionViewModel> Voting(int questionId, int voteType, ClaimsPrincipal User)
		{
			var user = await _userManager.GetUserAsync(User);
			var pollId = _context.Questions.Where(x => x.Id == questionId).Select(x => x)?.First()?.PolleId;
			var actualPoll = _context.Polles.Find(pollId);
			var actualStatusName = _context.StatusTypes.Find(actualPoll.StatusId);
			if (actualStatusName.StatusName == "Opened")
			{
				if (_context.Votes.Find(questionId, user.Id) != null)
				{
					_context.Votes.Remove(_context.Votes.Find(questionId, user.Id));
				}
				_context.Votes.Add(new VoteModel { UserId = user.Id, QuestionId = questionId, VoteTypeId = voteType });
				_context.SaveChanges();
			}

			return new QuestionViewModel
			{ Question = _context.Questions.Find(questionId), User = user, Votes = _context.Votes.ToList(), VotesTypes = _context.VotesTypes.ToList() };
		}

	public bool IsPollInProgress(int pollId)
		{
		int status = _context.Polles.Find(pollId).StatusId;
		if (status == _context.StatusTypes.Where(x => x.StatusName == "Opened").Select(x => x.Id).ToList().First())
		{
			return true;
		}

		return false;
	}

		public HistoryViewModel History()
		{
			return new HistoryViewModel { PollsHistoryList = _context.PollsHistory.Include(x => x.Votes).ToList() };
		}

		public void HistoryCleaner()
		{
			foreach (var historyPoll in _context.PollsHistory)
			{
				_context.Remove(historyPoll);
			}

			_context.SaveChanges();
		}

		public async Task<PollesViewModel> PollChangeState(int statusTypeId, int pollId)
		{
			if (_context.StatusTypes.Find(statusTypeId).StatusName == "Stopped"
				|| _context.StatusTypes.Find(statusTypeId).StatusName == "Opened")
			{
				_context.Polles.Find(pollId).StatusId = statusTypeId;
				await _context.SaveChangesAsync();
			}

			if (_context.StatusTypes.Find(statusTypeId).StatusName == "Closed")
			{
				_context.PollsHistory.Add(
					new PollsHistoryModel { PollName = _context.Polles.Find(pollId).Description });
				await _context.SaveChangesAsync();
				var lastPollInHistory = _context.PollsHistory.ToList().Last();

				var votesTypes = _context.VotesTypes.ToList();
				//_context.Votes.Where(x => x.QuestionModel.PolleId == pollId).Load();
				var votes = _context.Votes.Include(x => x.QuestionModel).ToList();
				foreach (var type in votesTypes)
				{
					List<VoteModel> currentPollAndTypeVotes = votes.FindAll(x => x.VoteTypeId == type.Id && x.QuestionModel.PolleId == pollId);
					int amount = currentPollAndTypeVotes.Count();
					foreach (var question in _context.Questions.Where(x => x.PolleId == pollId).Select(x => x).ToList())
					{
						foreach (var authoritieCouple in _context.AuthorityDependencies.ToList())
						{
							if (!_context.Votes.ToList().Exists(x => x.QuestionId == question.Id && x.UserId == authoritieCouple.AuthrityId)
								&& _context.Votes.ToList().Exists(x => x.QuestionId == question.Id && x.VoteTypeId == type.Id
															&& x.UserId == authoritieCouple.RepresentativeAuthrityId))
							{
								amount++;
							}
						}
					}

					_context.VotesInPollsHistory.Add(
						new VotesHistoryModel { VoteSummary = amount, PollHistoryId = lastPollInHistory.Id, VoteName = type.VoteName });
				}

				_context.Polles.Find(pollId).StatusId = statusTypeId;
				await _context.SaveChangesAsync();
			}

			return new PollesViewModel(_context);
		}

		public void PollDelete(int pollId)
		{
			_context.Polles.Remove(_context.Polles.Find(pollId));
			_context.SaveChanges();
		}

		public int PollAdd(string newPollDescription = null)
		{
			int statusId = _context.StatusTypes.Where(x => x.StatusName == "Opened").Select(x => x.Id).ToList().First();
			_context.Polles.Add(new PolleModel { Description = newPollDescription, StatusId = statusId });
			_context.SaveChanges();
			int id = _context.Polles.Where(x => x.Description == newPollDescription).Single().Id;
			return id;
		}

		public void PollSaveChanges(int? pollId, ICollection<string> questionsText
			, string action, ICollection<int> questionsId = null
			, ICollection<IFormFile> uploadedFiles = null
			, ICollection<int> questionIdForFiles = null)
		{
			var questionIdList = questionsId.ToList();
			var questionTextList = questionsText.ToList();
			var questionIdForFilesList = questionIdForFiles.ToList();
			var uploadedFilesList = uploadedFiles.ToList();
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

				for (int i = 0; i < questionIdForFilesList.Count; i++)
				{
					AddFileToDB(uploadedFilesList[i], questionIdForFilesList[i]);
				}
			}

			if (pollId != null && action == "add")
			{
				_context.Questions.Add(new QuestionModel { PolleId = (int)pollId, Question = questionTextList[0] });
			}

			_context.SaveChanges();
		}

		private void AddFileToDB(IFormFile uploadedFile, int questionId)
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
					new FileInDbModel
					{
						File = byteFile
						,
						ContentType = uploadedFile.ContentType
						,
						FileName = uploadedFile.FileName
						,
						QuestionId = questionId
					});

				_context.SaveChanges();
				int id = _context.FilesInDb.Where(x => x.File == byteFile).Select(x => x).ToList().Last().Id;
				_context.Questions.Find(questionId).FileId = id;
				_context.SaveChanges();
			}
		}

		public (byte[] fileInBytes, string fileType, string fileName) GetQuestionWithFilesLoaded(int questionId)
		{
			QuestionModel question = _context.Questions.Find(questionId);
			_context.FilesInDb.Where(x => x.Id == question.FileId).Load();
			byte[] fileInBytes = question.File.File;
			string fileType = question.File.ContentType;
			string fileName = question.File.FileName;
			return (fileInBytes, fileType, fileName);
		}
	}
}

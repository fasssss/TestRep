using FirstProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class SummarizingViewModel
	{
		private FirstProjectContext _context;
		private List<AuthorityDependenciesModel> _AuthorityDependencies { get; set; }
		public SummarizingViewModel(FirstProjectContext context, int pollId)
		{
			_context = context;
			this.pollId = pollId;
			_AuthorityDependencies = _context.AuthorityDependencies.ToList();
			AllVotes = _context.Votes.ToList();
			VotesTypes = _context.VotesTypes.ToList();
			PollStatusList = _context.StatusTypes.ToList();
			CurrentPollQuestions = _context.Questions.Where(x => x.PolleId == pollId).Select(x => x).ToList();
		}

		public int pollId;
		public List<VoteModel> AllVotes { get; set; }
		public List<VotesTypesModel> VotesTypes { get; set; }
		public List<QuestionModel> CurrentPollQuestions { get; set; }
		public List<StatusTypesModel> PollStatusList { get; set; }

		public string QuestionSummarizing(QuestionModel question)
		{
			string votesString = String.Empty;
			foreach (var type in VotesTypes)
			{
				List<VoteModel> currentQuestionAndTypeVotes = AllVotes.FindAll(x => x.VoteTypeId == type.Id && x.QuestionId == question.Id);
				int amount = currentQuestionAndTypeVotes.Count();
				foreach (var authoritieCouple in _AuthorityDependencies)
				{
					if (!AllVotes.Exists(x => x.QuestionId == question.Id && x.UserId == authoritieCouple.AuthrityId)
						&& AllVotes.Exists(x => x.QuestionId == question.Id && x.VoteTypeId == type.Id
													&& x.UserId == authoritieCouple.RepresentativeAuthrityId))
					{
						amount++;
					}
				}

				votesString += type.VoteName + " = " + amount + "  ";
			}

			return votesString;
		}
	}
}

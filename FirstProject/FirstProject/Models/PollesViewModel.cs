using FirstProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class PollesViewModel
	{
		private FirstProjectContext _context;
		public PollesViewModel(FirstProjectContext context, int? PollId = null)
		{
			_context = context;
			PollesList = _context.Polles.ToList();
			QuestionsList = _context.Questions.ToList();
			VotesTypes = _context.VotesTypes.ToList();
			Votes = _context.Votes.Select(x => x).ToList();
			this.PollId = PollId;
			if (this.PollId != null)
			{
				CurrentQuestions = QuestionsList.Where(x => x.PolleId == PollId).Select(x => x).ToList();
			}
		}

		public PollesViewModel(FirstProjectContext context, ExtendedUserModel user, int? PollId = null)
			: this(context, PollId)
		{
			this.User = user;
		}

		public ExtendedUserModel User { get; set; }
		public int? PollId { get; set; }
		public List<VotesTypesModel> VotesTypes { get; set; }
		public List<QuestionModel> CurrentQuestions { get; set; }
		public List<PolleModel> PollesList { get; private set; }
		public List<QuestionModel> QuestionsList { get; private set; }
		public List<VoteModel> Votes { get; set; }

		public void AddOrChangeQuestion(int questionId, string question)
		{
			_context.Questions.Find(question).Question = question;
			_context.SaveChanges();
		}
	}
}

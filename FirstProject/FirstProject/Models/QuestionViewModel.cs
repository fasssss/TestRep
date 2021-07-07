using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class QuestionViewModel
	{
		public QuestionModel Question { get; set; }
		public List<VotesTypesModel> VotesTypes { get; set; }
		public List<VoteModel> Votes { get; set; }
		public ExtendedUserModel User { get; set; }
	}
}

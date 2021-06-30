using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class VotesHistoryModel
	{
		[Key]
		public int VoteTypeId { get; set; }
		public int QuestionId { get; set; }
		public int VoteSummary { get; set; }

		[ForeignKey("QuestionId")]
		public QuestionModel QuestionModel { get; set; }
		[ForeignKey("VoteTypeId")]
		public VotesTypesModel VoteTypeModel { get; set; }
	}
}

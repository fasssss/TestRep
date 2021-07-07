using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class VoteModel
	{
		[Key]
		public System.Guid UserId { get; set; }
		public int QuestionId { get; set; }
		public int VoteTypeId { get; set; }

		[ForeignKey("UserId")]
		public ExtendedUserModel ExtendedUserModel { get; set; }
		[ForeignKey("QuestionId")]
		public QuestionModel QuestionModel { get; set; }
		[ForeignKey("VoteTypeId")]
		public VotesTypesModel VoteTypeModel { get; set; }
	}
}

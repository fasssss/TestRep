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
		public int Id { get; set; }
		[StringLength(450)]
		public string UserId { get; set; }
		public int QuestionId { get; set; }
		[StringLength(20)]
		public string Vote { get; set; }
		[StringLength(50)]
		public string VoterName { get; set; }
		[StringLength(50)]
		public string VotedForPerson { get; set; }
		public string FilePath { get; set; }

		[ForeignKey("UserId")]
		public ExtendedUserModel ExtendedUserModel { get; set; }
		[ForeignKey("QuestionId")]
		public QuestionModel QuestionModel { get; set; }
	}
}

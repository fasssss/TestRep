using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class QuestionModel
	{
		[Key]
		public int Id { get; set; }
		public int PolleId { get; set; }
		public string Question { get; set; }
		public int? FileId { get; set; }

		[ForeignKey("FileId")]
		public FileInDbModel File { get; set; }
		[ForeignKey("PolleId")]
		public PolleModel PolleModel { get; set; }

		ICollection<VoteModel> VoteModels { get; set; }
	}
}

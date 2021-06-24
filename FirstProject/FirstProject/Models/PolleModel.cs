using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class PolleModel
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
		public string FinalResult { get; set; }
		[StringLength(50)]
		public string Status { get; set; }

		ICollection<QuestionModel> QuestionModels { get; set; }
	}
}

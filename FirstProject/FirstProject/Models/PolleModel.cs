using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class PolleModel
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
		public int StatusId { get; set; }

		[ForeignKey("StatusId")]
		public StatusTypesModel StatusType { get; set; }

		ICollection<QuestionModel> QuestionModels { get; set; }
	}
}

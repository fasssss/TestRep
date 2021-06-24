using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class VotesTypesModel
	{
		[Key]
		public int Id { get; set; }
		[StringLength(20)]
		public string VoteName { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class PollsHistoryModel
	{
		[Key]
		public System.Guid Id { get; set; }
		public string PollName { get; set; }

		public ICollection<VotesHistoryModel> Votes { get; set; }
	}
}

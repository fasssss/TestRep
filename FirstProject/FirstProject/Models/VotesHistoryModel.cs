using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class VotesHistoryModel
	{
		public System.Guid Id { get; set; }
		public string VoteName { get; set; }
		public int VoteSummary { get; set; }

		public System.Guid PollHistoryId { get; set; }

		[ForeignKey("PollHistoryId")]
		public PollsHistoryModel PollHistory { get; set; }
	}
}

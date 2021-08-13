using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class ChatHistoryModel
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public System.Guid UserID { get; set; }

		[ForeignKey("UserID")]
		public ExtendedUserModel ExtendedUserModel { get; set; }
	}
}

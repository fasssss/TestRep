using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class FileInDbModel
	{
		[Key]
		public int Id { get; set; }
		public byte[] File { get; set; }
		[StringLength(20)]
		public string ContentType { get; set; }
		public string FileName { get; set; }

		public int QuestionId { get; set; }
		[ForeignKey("QuestionId")]
		public QuestionModel Question { get; set; }

	}
}

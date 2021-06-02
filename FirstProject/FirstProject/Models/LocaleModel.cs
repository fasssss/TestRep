using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class LocaleModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		[StringLength(20)]
		public string Name { get; set; }
		[StringLength(5)]
		public string LocaleCode { get; set; }
		public bool Enabled { get; set; }
		public bool IsDefault { get; set; }

		public ICollection<ExtendedUserModel> ExtendedUserModels { get; set; }
	}
}

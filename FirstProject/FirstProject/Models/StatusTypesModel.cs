﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class StatusTypesModel
	{
		[Key]
		public int Id { get; set; }
		[StringLength(50)]
		public string StatusName { get; set; }
	}
}

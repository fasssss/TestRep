﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class ExtendedUserModel : IdentityUser
	{
		public int? LocaleID { get; set; }

		[ForeignKey("LocaleID")]
		public LocaleModel LocaleModel { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class AuthorityDependenciesModel
	{
		[Key]
		public System.Guid AuthrityId { get; set; }
		[ForeignKey("AuthrityId")]
		public ExtendedUserModel AuthorityModel { get; set; }

		public System.Guid RepresentativeAuthrityId { get; set; }
		[ForeignKey("RepresentativeAuthrityId")]
		public ExtendedUserModel RepresentativeAuthorityModel { get; set; }
	}
}

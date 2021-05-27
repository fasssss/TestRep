using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
	public class PrivateInfoModel
	{
		public IList<UserLoginInfo> currentLogins { get; set; }
		public IdentityUser user { get; }
		public List<OutputFormatClaims> claims { get; set; }
		public bool IsAuthenticated { get; set; }
		public PrivateInfoModel(IdentityUser user = null) : this()
		{
			this.user = user;
		}
		public PrivateInfoModel()
		{
			currentLogins = new List<UserLoginInfo>();
			claims = new List<OutputFormatClaims>();
		}
		public class OutputFormatClaims
		{
			public string Name { get; set; }
			public string Issuer { get; set; }
			public string Type { get; set; }
			public string Value { get; set; }
		}
	}
}

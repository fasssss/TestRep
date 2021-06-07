using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstProject.Models;

namespace FirstProject.Models
{
	public class PrivateInfoModel
	{
		public IList<UserLoginInfo> CurrentLogins { get; set; }
		public ExtendedUserModel User { get; }
		public List<OutputFormatClaims> Claims { get; set; }
		public bool IsAuthenticated { get; set; }
		public PrivateInfoModel(ExtendedUserModel user = null) : this()
		{
			this.User = user;
		}
		public PrivateInfoModel()
		{
			CurrentLogins = new List<UserLoginInfo>();
			Claims = new List<OutputFormatClaims>();
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

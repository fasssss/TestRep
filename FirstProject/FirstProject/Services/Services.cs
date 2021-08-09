using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FirstProject.Data;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FirstProject.Services;
namespace FirstProject.Services
{
	public class Services
	{
		protected FirstProjectContext _context;

		public Services(FirstProjectContext context)
		{
			_context = context;
		}
	}
}

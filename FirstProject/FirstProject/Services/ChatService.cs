using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstProject.Data;

namespace FirstProject.Services
{
	public class ChatService
	{
		private FirstProjectContext _context;

		public ChatService(FirstProjectContext context)
		{
			_context = context;
		}
	}
}

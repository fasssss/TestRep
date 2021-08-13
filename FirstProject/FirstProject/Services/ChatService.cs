using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstProject.Data;
using FirstProject.Models;
using FirstProject.SignalRHubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Services
{
	public class ChatService : Services
	{
		private readonly UserManager<ExtendedUserModel> _userManager;
		private readonly IHubContext<ChatHub> _hubContext;

		public ChatService(FirstProjectContext context,
			UserManager<ExtendedUserModel> userManager) : base(context)
		{
			_userManager = userManager;
		}

		public string ChatLoading(int messageCount)
		{
			List<string> chatPart = _context.ChatHistory.Include(x => x.ExtendedUserModel)
				.Select(x => x.ExtendedUserModel.UserName + ": " + x.Message).ToList();
			chatPart.Reverse();
			chatPart = chatPart.Skip(messageCount).Take(30).ToList();
			chatPart.Reverse();
			string messagesString = string.Join("", chatPart);
			return messagesString;
		}

		public async Task SendingMessageAsync(string message, ClaimsPrincipal User)
		{
			var user = await _userManager.GetUserAsync(User);
			_context.ChatHistory.Add(new ChatHistoryModel { UserID = user.Id, Message = message + '\n' }); // MOVE TO SERVICE ASAP
			_context.SaveChanges();
			message = user.UserName + ": " + message + '\n';
			await _hubContext.Clients.All.SendAsync("Send", message);
		}
	}
}

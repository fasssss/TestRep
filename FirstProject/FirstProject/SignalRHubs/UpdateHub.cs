using FirstProject.Data;
using FirstProject.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace FirstProject.SignalRHubs
{
	public class UpdateHub : Hub
	{

		public async Task Send(string message)
		{
			await this.Clients.Others.SendAsync("Send", message);
		}

		public async Task UpdatePollStatus(string message)
		{
			//Thread.Sleep(500);
			await this.Clients.Others.SendAsync("UpdatePollStatus", message);
		}
	}
}

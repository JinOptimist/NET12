using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services;

namespace WebMaze.SignalRHubs
{
    public class DocumentPreparationHub : Hub
    {
        public async Task Notification(string percent, string pages)
        {
            await Clients.All.SendAsync("Notification", percent, pages);
        }
    }
}

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
        public async Task Notification(int id, string percent, string pages)
        {
            await Clients.All.SendAsync("Notification",id, percent, pages);
        }
        public async Task stopNotification(int id)
        {
            await Clients.All.SendAsync("stopNotification", id);
        }
        public async Task NewDocument(int id)
        {
            await Clients.All.SendAsync("NewDocument", id);
        }        
    }
}

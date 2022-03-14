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
        public async Task UpdateStatus(int id, string percent, string pages)
        {
            await Clients.All.SendAsync("UpdateStatus", id, percent, pages);
        }
        public async Task CancelPreparation(int id)
        {
            await Clients.All.SendAsync("CancelPreparation", id);
        }
        public async Task ReadyDocument(int id, string percent, string pages)
        {
            await Clients.All.SendAsync("ReadyDocument", id, percent, pages);
        }
        public async Task NewDocument(int id)
        {
            await Clients.All.SendAsync("NewDocument", id);
        }        
    }
}

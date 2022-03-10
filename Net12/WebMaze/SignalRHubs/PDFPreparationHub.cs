using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services;

namespace WebMaze.SignalRHubs
{
    public class PDFPreparationHub : Hub
    {
        public async Task Progres(string percent)
        {
            await Clients.All.SendAsync("Progres", percent);
        }
    }
}

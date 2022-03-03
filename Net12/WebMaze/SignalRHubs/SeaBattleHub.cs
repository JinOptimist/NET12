using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.SignalRHubs
{
    public class SeaBattleHub : Hub
    {

        public async Task AIShooting()
        {

            //var name = _userService.GetCurrentUser().Name;
            await Clients.All.SendAsync("NewMessage", "message");

        }
    }
}

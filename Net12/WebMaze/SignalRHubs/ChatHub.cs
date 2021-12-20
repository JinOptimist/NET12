using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services;

namespace WebMaze.SignalRHubs
{
    public class ChatHub : Hub
    {
        private UserService _userService;

        public ChatHub(UserService userService)
        {
            _userService = userService;
        }

        public async Task NewMessage(string message)
        {
            var name = _userService.GetCurrentUser().Name;
            await Clients.All.SendAsync("NewMessage", message, name);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.SignalRHubs
{
    public class MazeHub : Hub
    {
        private UserService _userService;

        public MazeHub(UserService userService)
        {
            _userService = userService;
        }

        public async Task ChangingMazeCells(MazeLevelViewModel mazeView)
        {
            var s = Context.User.Identity.Name;
            await Clients.All.SendAsync("ChangingMazeCells", mazeView);
        }
    }
}

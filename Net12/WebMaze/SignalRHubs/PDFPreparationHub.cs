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
        public async Task Progres(string percent, int pdfId)
        {
            await Clients.All.SendAsync("Progres", percent, pdfId);
        }
        public async Task stopProgres(int pdfId)
        {
            await Clients.All.SendAsync("stopProgres", pdfId);
        }
        public async Task downloadPDF(int pdfId, string percent)
        {
            await Clients.All.SendAsync("downloadPDF", pdfId, percent);
        }
    }
}

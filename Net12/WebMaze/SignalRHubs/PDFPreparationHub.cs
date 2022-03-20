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
        public async Task Progres(int pdfId, string pdfName, string percent)
        {
            await Clients.All.SendAsync("Progres", pdfId, pdfName, percent);
        }
        public async Task StopProgres(int pdfId, string pdfName)
        {
            await Clients.All.SendAsync("StopProgres", pdfId, pdfName);
        }

        public async Task ReadyDocument(int pdfId, string pdfName)
        {
            await Clients.All.SendAsync("ReadyPDF", pdfId, pdfName);
        }

        public async Task DownloadPDF(int pdfId, string pdfName, string percent)
        {
            await Clients.All.SendAsync("DownloadPDF", pdfId, pdfName, percent);
        }
    }
}

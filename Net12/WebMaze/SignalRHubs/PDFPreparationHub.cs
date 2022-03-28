using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebMaze.SignalRHubs
{
    public class PDFPreparationHub : Hub
    {
        public async Task Progres(int pdfId, string pdfName, string progress)
        {
            await Clients.All.SendAsync("Progres", pdfId, pdfName, progress);
        }
        public async Task StopProgres(int pdfId, string pdfName)
        {
            await Clients.All.SendAsync("StopProgres", pdfId, pdfName);
        }
        public async Task ReadyDocument(int pdfId, string pdfName)
        {
            await Clients.All.SendAsync("ReadyPDF", pdfId, pdfName);
        }
    }
}

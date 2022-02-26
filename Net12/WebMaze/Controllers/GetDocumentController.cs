using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.Models.GenerationDocument;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class GetDocumentController : Controller
    {
        private IHubContext<DocumentPreparationHub> _documentPreparationHub;       

        public GetDocumentController(IHubContext<DocumentPreparationHub> documentPreparationHub)
        {
            _documentPreparationHub = documentPreparationHub;
        }

        public static List<DocumentStatus> DocumentPreparationTasks = new List<DocumentStatus>();

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult StartPreparation()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            DocumentStatus document;

            lock (DocumentPreparationTasks)
            {
                document = new DocumentStatus
                {
                    Id = 1,
                    Percent = 0,
                    Pages = 100,
                    Document = "Text",
                    CancellationTokenSource = cancelTokenSource
                };

                DocumentPreparationTasks.Add(document);
            }

            Task task = new Task(() => DocumentPreparation(document), token);
            task.Start();

            return View();
        }

        public IActionResult GetStatus(int documentId)
        {
            var doc = DocumentPreparationTasks.First(x => x.Id == documentId);
            return View();
        }

        public IActionResult DownloadDocument()
        {

            return View();
        }       

        public IActionResult StopPreparation(int documentId)
        {
            var document = DocumentPreparationTasks.First(x => x.Id == documentId);
            document.CancellationTokenSource.Cancel();
            lock (DocumentPreparationTasks)
            {
                DocumentPreparationTasks.Remove(document);
            }

            return Json(true);
        }

        private void DocumentPreparation(DocumentStatus document)
        {
            for (int i = 0; i < document.Pages; i++)
            {
                document.Document += $" {document.Percent} ";
                document.Percent++;
                document
                    .CancellationTokenSource
                    .Token
                    .ThrowIfCancellationRequested();
                Thread.Sleep(1000);

                _documentPreparationHub.Clients.All.SendAsync("Notification", document.Percent, document.Pages);
            }
        }
    }
}

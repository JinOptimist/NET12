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

        [HttpGet]
        public IActionResult StartPreparation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartPreparation(DocumentStatus documentViewModel)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            DocumentStatus document;

            lock (DocumentPreparationTasks)
            {
                document = new DocumentStatus
                {
                    Id = DocumentPreparationTasks.Any()
                        ? DocumentPreparationTasks.Max(x => x.Id) + 1
                        : 1,
                    Percent = 0,
                    Pages = documentViewModel.Pages,
                    Document = documentViewModel.Document,
                    CancellationTokenSource = cancelTokenSource
                };

                DocumentPreparationTasks.Add(document);
            }

            Task task = new Task(() => DocumentPreparation(document), token);
            task.Start();

            return RedirectToAction("GetStatus");
        }

        public IActionResult GetStatus()
        {
            return View();
        }

        public void StopPreparation(int documentId)
        {
            var document = DocumentPreparationTasks.First(x => x.Id == documentId);
            document.CancellationTokenSource.Cancel();
            lock (DocumentPreparationTasks)
            {
                DocumentPreparationTasks.Remove(document);
            }
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
                _documentPreparationHub.Clients.All.SendAsync("Notification", document.Percent, document.Pages);

                Thread.Sleep(1000);                
            }

        }
    }
}

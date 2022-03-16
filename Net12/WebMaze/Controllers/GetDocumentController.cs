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
            var documentViewModels = new List<DocumentStatus>();
            foreach (var document in DocumentPreparationTasks)
            {
                var documentViewModel = new DocumentStatus();
                documentViewModel.Id = document.Id;
                documentViewModel.Pages = document.Pages;
                documentViewModel.Percent = document.Percent;

                documentViewModels.Add(documentViewModel);
            }

            return View(documentViewModels);
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

            _documentPreparationHub.Clients.All.SendAsync("NewDocument", document.Id);

            return RedirectToAction("GetStatus", new {id = document.Id });
        }

        public IActionResult GetStatus(int id)
        {
            if (!DocumentPreparationTasks.Any(x => x.Id == id))
            {
                return RedirectToAction("Index");
            }
            var documentId = id;
            return View(documentId);
        }

        public void CancelPreparation(int documentId)
        {
            var document = DocumentPreparationTasks.First(x => x.Id == documentId);
            document.CancellationTokenSource.Cancel();
            lock (DocumentPreparationTasks)
            {
                DocumentPreparationTasks.Remove(document);
            }
            _documentPreparationHub.Clients.All.SendAsync("CancelPreparation", document.Id);
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
                _documentPreparationHub.Clients.All.SendAsync("UpdateStatus", document.Id, document.Percent, document.Pages);
                
                Thread.Sleep(1000);                
            }

            _documentPreparationHub.Clients.All.SendAsync("ReadyDocument", document.Id, document.Percent, document.Pages);
        }
    }
}


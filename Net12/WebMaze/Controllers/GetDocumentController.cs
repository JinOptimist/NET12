using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.Models.GenerationDocument;

namespace WebMaze.Controllers
{
    public class GetDocumentController : Controller
    {
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
                    //Id = DocumentPreparationTasks.Any()
                    //    ? DocumentPreparationTasks.Max(x => x.Id) + 1
                    //    : 1,
                    Percent = 0,
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
            return Json(doc.Document);
        }

        public IActionResult DownloadDocument()
        {

            return View();
        }

        private void DocumentPreparation(DocumentStatus document)
        {
            for (int i = 0; i < 50; i++)
            {
                document.Document += $" {document.Percent} ";
                document.Percent++;
                document
                    .CancellationTokenSource
                    .Token
                    .ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }

            //return RedirectToAction("DownloadDocument");
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
    }
}

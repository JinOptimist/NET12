using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.Models.GenerationDocument;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class GetPDFController : Controller
    {
        private IHubContext<PDFPreparationHub> _pdfPreparationHub;

        public GetPDFController(IHubContext<PDFPreparationHub> pdfPreparationHub)
        {
            _pdfPreparationHub = pdfPreparationHub;
        }

        public static List<PDFGenerationTaskInfo> PDFPreparationTasks = new List<PDFGenerationTaskInfo>();

        public IActionResult Index()
        {
            var pdfViewModels = new List<PDFGenerationTaskInfo>();
            foreach (var pdf in PDFPreparationTasks)
            {
                var pdfViewModel = new PDFGenerationTaskInfo();
                pdfViewModel.Id = pdf.Id;
                pdfViewModel.Percent = pdf.Percent;

                pdfViewModels.Add(pdfViewModel);
            }
            return View(pdfViewModels);
        }

        [HttpGet]
        public IActionResult StartPreparationPDF()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartPreparationPDF(PDFGenerationTaskInfo pdfGenerationTaskInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(pdfGenerationTaskInfo);
            }
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            PDFGenerationTaskInfo pdf;

            lock (PDFPreparationTasks)
            {
                pdf = new PDFGenerationTaskInfo
                {

                    Id = PDFPreparationTasks.Any()
                        ? PDFPreparationTasks.Max(x => x.Id) + 1
                        : 1,
                    Percent = 0,
                    Url = pdfGenerationTaskInfo.Url,
                    Name = pdfGenerationTaskInfo.Name,               
                    CancellationTokenSource = cancelTokenSource
                };

                PDFPreparationTasks.Add(pdf);
            }

            //var task = new Task<byte[]>(() => PDFPreparation(pdf), token);
            //pdf.Task = task;
            Task task = new Task(() => PDFPreparation(pdf), token);
            task.Start();            

            return RedirectToAction($"{nameof(GetPDFController.GetStatusPDF)}", new { pdfId = pdf.Id });
        }

        public IActionResult GetStatusPDF(int pdfId)
        {
            if (!PDFPreparationTasks.Any(x => x.Id == pdfId))
            {
                return RedirectToAction("Index");
            }   
            
            return View(pdfId);
        }

        public void StopPreparationPDF(int pdfId)
        {
            var pdf = PDFPreparationTasks.First(x => x.Id == pdfId);
            pdf.CancellationTokenSource.Cancel();
            lock (PDFPreparationTasks)
            {
                PDFPreparationTasks.Remove(pdf);
            }
            _pdfPreparationHub.Clients.All.SendAsync("StopProgres", pdf.Id, pdf.Name);
        }

        private void PDFPreparation(PDFGenerationTaskInfo pdf)
        {
           const int InterestWhenDone = 100;            


            for (int i = 0; i < InterestWhenDone; i++)
            {
                pdf.Percent++;

                if (pdf.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                _pdfPreparationHub.Clients.All.SendAsync("Progres", pdf.Id, pdf.Name, pdf.Percent);

                Thread.Sleep(100);
            }
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdfReady = new PdfDocument(writer);
            var document = new Document(pdfReady);

            Paragraph header = new Paragraph($"{pdf.Name}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20);
            document.Add(header);

            Image img = new Image(ImageDataFactory
            .Create($"{pdf.Url}"))
            .SetTextAlignment(TextAlignment.CENTER);
            document.Add(img);
            document.Close();

            byte[] byte1 = stream.ToArray();

            pdf.ReadyPDF = byte1;

                _pdfPreparationHub.Clients.All.SendAsync("ReadyPDF", pdf.Id, pdf.Name, pdf.Percent);
           
        }
        public IActionResult DownloadPDF(int pdfId)
        {
            var pdf = PDFPreparationTasks.First(x => x.Id == pdfId);

            var file = pdf.ReadyPDF;

            {
               
                return File(file, "application/pdf", $"{pdf.Name}.pdf");
            }
        }
    }
}
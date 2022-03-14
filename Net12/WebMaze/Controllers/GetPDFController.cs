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

            Task task = new Task(() => PDFPreparation(pdf), token);
            task.Start();

            _pdfPreparationHub.Clients.All.SendAsync("NewPdf", pdf.Id);

            return RedirectToAction($"{nameof(GetPDFController.GetStatusPDF)}",  pdf );
        }

        public IActionResult GetStatusPDF(PDFGenerationTaskInfo pdf)
        {
            if (!PDFPreparationTasks.Any(x => x.Id == pdf.Id))
            {
                return RedirectToAction("Index");
            }
            return View(pdf);
        }        

        public void StopPreparationPDF(int pdfId)
        {
            var pdf = PDFPreparationTasks.First(x => x.Id == pdfId);
            pdf.CancellationTokenSource.Cancel();
            lock (PDFPreparationTasks)
            {
                PDFPreparationTasks.Remove(pdf);
            }
            _pdfPreparationHub.Clients.All.SendAsync("stopNotification", pdf.Id);
        }

        private void PDFPreparation(PDFGenerationTaskInfo pdf)
        {
            for (int i = 0; i < 100; i++)
            {
                _pdfPreparationHub.Clients.All.SendAsync("Progres", pdf.Id, pdf.Percent);
                pdf.Percent++;
                if (pdf.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                Thread.Sleep(1000);
            }
            _pdfPreparationHub.Clients.All.SendAsync("Progres", pdf.Id, pdf.Percent);
        }
        public IActionResult DownlodPDF(int pdfId)
        {
            var pdf = PDFPreparationTasks.First(x => x.Id == pdfId);

            {
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
                return File(byte1, "application/pdf", $"{pdf.Name}.pdf");
            }
        }
    }
}


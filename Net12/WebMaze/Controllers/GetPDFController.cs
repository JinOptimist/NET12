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

        [HttpGet]
        public IActionResult StartPreparationPDF()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartPreparationPDF(PDFGenerationTaskInfo pdfGenerationTaskInfo)
        {
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

            return RedirectToAction($"{nameof(GetPDFController.GetStatusPDF)}", new { id = pdf.Id });
        }

        public IActionResult GetStatusPDF(int id)
        {
            var pdfId = id;
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
        }

        private void PDFPreparation(PDFGenerationTaskInfo pdf)
        {
            for (int i = 0; i < 100; i++)
            {
                pdf.Percent++;                
                if (pdf.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                _pdfPreparationHub.Clients.All.SendAsync("Progres", pdf.Percent);
                Thread.Sleep(1000);

            }
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


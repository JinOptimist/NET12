using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
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
        private IWebHostEnvironment _hostEnvironment;

        public GetPDFController(IHubContext<PDFPreparationHub> pdfPreparationHub,
            IWebHostEnvironment hostEnvironment)
        {
            _pdfPreparationHub = pdfPreparationHub;
            _hostEnvironment = hostEnvironment;
        }

        public static List<PDFGenerationTaskInfoViewModel> PDFPreparationTasks = new List<PDFGenerationTaskInfoViewModel>();

        public IActionResult Index()
        {

            return View(PDFPreparationTasks);
        }

        [HttpGet]
        public IActionResult StartPreparationPDF()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartPreparationPDF(PDFGenerationTaskInfoViewModel pdfGenerationTaskInfoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pdfGenerationTaskInfoViewModel);
            }
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            PDFGenerationTaskInfoViewModel pdf;

            lock (PDFPreparationTasks)
            {
                pdf = new PDFGenerationTaskInfoViewModel
                {

                    Id = PDFPreparationTasks.Any()
                        ? PDFPreparationTasks.Max(x => x.Id) + 1
                        : 1,
                    Progress = 0,
                    Url = pdfGenerationTaskInfoViewModel.Url,
                    Name = pdfGenerationTaskInfoViewModel.Name,
                    CancellationTokenSource = cancelTokenSource
                };

                PDFPreparationTasks.Add(pdf);
            }

            Task task = new Task(() => PDFPreparation(pdf), token);
            task.Start();

            return RedirectToAction($"{nameof(GetPDFController.GetStatusPDF)}", new { pdfId = pdf.Id });
        }

        public IActionResult GetStatusPDF(int pdfId)
        {
            var model = PDFPreparationTasks.FirstOrDefault(x => x.Id == pdfId);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
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

        private void PDFPreparation(PDFGenerationTaskInfoViewModel pdf)
        {
            const int PDFWhenDone = 100;

            for (int i = 0; i < PDFWhenDone; i++)
            {
                pdf.Progress++;

                if (pdf.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                _pdfPreparationHub.Clients.All.SendAsync("Progres", pdf.Id, pdf.Name, pdf.Progress);

                Thread.Sleep(500);
            }

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, "createdDocuments", $"{pdf.Name}-{pdf.Id}.pdf");

            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdfReady = new PdfDocument(writer);
            Document document = new Document(pdfReady);

            Paragraph header = new Paragraph($"{pdf.Name}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20);
            document.Add(header);

            Image img = new Image(ImageDataFactory
            .Create($"{pdf.Url}"))
            .SetTextAlignment(TextAlignment.CENTER);
            document.Add(img);
            document.Close();

            pdf.ReadyToDownload = true;
            pdf.ReadyPDF = filePath;
            
            _pdfPreparationHub.Clients.All.SendAsync("ReadyPDF", pdf.Id, pdf.Name);

        }
        public IActionResult DownloadPDF(int pdfId)
        {
            var pdf = PDFPreparationTasks.First(x => x.Id == pdfId);

            var file = pdf.ReadyPDF;

            var stream = new FileStream($"{file}", FileMode.Open);
            return File(stream, "application/pdf");

        }

    }
}
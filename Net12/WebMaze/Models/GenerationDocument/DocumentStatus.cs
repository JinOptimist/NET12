using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebMaze.Models.GenerationDocument
{
    public class DocumentStatus
    {
        public int Id { get; set; }

        public int Percent { get; set; }
        public int Pages { get; set; }
        public string Document { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public List<PDFGenerationTaskInfo> PDFGenerationTasks { get; set; }
    }
}

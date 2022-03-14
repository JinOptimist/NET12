using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebMaze.Models.GenerationDocument
{
    public class PDFGenerationTaskInfo
    {
        
        public int Id { get; set; }

        public int Percent { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
   
    }
}

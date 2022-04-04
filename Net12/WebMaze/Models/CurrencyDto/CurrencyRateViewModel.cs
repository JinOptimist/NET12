using System;

namespace WebMaze.Models.CurrencyDto
{
    public class CurrencyRateViewModel
    {
        public int Cur_ID { get; set; }
        public string Cur_Name { get; set; }
        public int Cur_Scale { get; set; }
        public double Cur_OfficialRate { get; set; }
        public DateTime Date { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
}

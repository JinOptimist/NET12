using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.CurrencyDto
{
    public class CurrencyRateDto
    {
        public int Cur_ID { get; set; }

        public string Cur_Abbreviation { get; set; }

        public string Cur_Name { get; set; }

        public int Cur_Scale { get; set; }

        public double Cur_OfficialRate { get; set; }
    }
}

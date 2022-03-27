using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebMaze.Models.CurrencyDto;

namespace WebMaze.Services
{
    public class CurrenceService 
    {
        public async Task<List<CurrencyRateDto>> GetRates()
        {
            try
            {
                var dateStr = DateTime.Now.ToString("yyyy-M-d");//2016-7-6
                var uri = $"https://www.nbrb.by/api/exrates/rates?ondate={dateStr}&periodicity=0";

                var rates = await CallAPI<List<CurrencyRateDto>>(uri);

                return rates;

            }
            catch
            {
                return new List<CurrencyRateDto>();
            }
        }

        public async Task<List<CurrencyRate>> GetAllCurrencies()
        {
            var uri = "https://www.nbrb.by/api/exrates/rates?periodicity=0";

            var currencies = await CallAPI<List<CurrencyRate>>(uri);

            return currencies;
        }

        public async Task<CurrencyRate> GetRateById(int cur_Id)
        {
            var uri = $"https://www.nbrb.by/api/exrates/rates/{cur_Id}";

            var rate = await CallAPI<CurrencyRate>(uri);

            return rate;
        }

        private async Task<Template> CallAPI<Template>(string uri)
        {
            var client = new HttpClient();

            var response = await client.GetAsync(uri);

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Template>(json);

            return result;
        }
    }
}
    }
}

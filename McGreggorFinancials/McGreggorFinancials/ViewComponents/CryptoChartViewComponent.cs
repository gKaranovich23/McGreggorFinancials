using Avapi;
using Avapi.AvapiDIGITAL_CURRENCY_DAILY;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Crypto;
using McGreggorFinancials.Models.Crypto.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class CryptoChartViewComponent : ViewComponent
    {
        private ICoinRepository _repo;
        private ICryptoCurrencyRepository _stockRepo;

        public CryptoChartViewComponent(ICoinRepository repo, ICryptoCurrencyRepository stockRepo)
        {
            _repo = repo;
            _stockRepo = stockRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Coin> coins = _repo.Coins.Where(e => e.Date.Year == Convert.ToDateTime(date).Year ? (e.Date.Month == Convert.ToDateTime(date).Month ?
                e.Date.Day <= Convert.ToDateTime(date).Day : e.Date.Month <= Convert.ToDateTime(date).Month) : e.Date.Year <= Convert.ToDateTime(date).Year).ToList();

            List<CryptoCurrency> cryptos = _stockRepo.CryptoCurrencies.ToList();

            List<LineChartData> lineData = new List<LineChartData>();

            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            int year = now.Year;
            double currentValue = 0;

            DateTime currentDate = new DateTime(now.Year, now.Month, 1);
            while (currentDate <= now)
            {
                double total = 0;
                DateTime valueDate;

                if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    valueDate = currentDate.AddDays(-1);
                }
                else if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    valueDate = currentDate.AddDays(-2);
                }
                else
                {
                    valueDate = currentDate;
                }

                foreach (var crypto in cryptos)
                {
                    Dictionary<DateTime, double> cryptoData =
                        JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(crypto.Ticker));

                    var sData = cryptoData.Keys.Where(x => Convert.ToDateTime(x).Day == valueDate.Day &&
                         Convert.ToDateTime(x).Month == valueDate.Month && Convert.ToDateTime(x).Year == valueDate.Year).FirstOrDefault();

                    if (sData != null && sData != DateTime.MinValue)
                    {
                        List<Coin> lineDataShares = coins.Where(e => e.CryptoCurrencyID == crypto.ID).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Year <= currentDate.Year).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Month <= currentDate.Month).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Day <= currentDate.Day).ToList();
                        int totalShares = lineDataShares.Select(e => e.NumOfCoins).Sum();
                        double s = Convert.ToDouble(cryptoData.GetValueOrDefault(sData));
                        double stockValue = s * (double)totalShares;
                        total += stockValue;
                    }
                }

                currentValue = total;

                lineData.Add(new LineChartData
                {
                    XData = currentDate.Day.ToString(),
                    YData = currentValue.ToString()
                });

                currentDate = currentDate.AddDays(1);
            }

            return View(lineData);
        }
    }
}

using Avapi;
using Avapi.AvapiTIME_SERIES_DAILY;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Stocks;
using McGreggorFinancials.Models.Stocks.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class StockChartViewComponent: ViewComponent
    {
        private IShareRepository _repo;
        private IStockRepository _stockRepo;

        public StockChartViewComponent(IShareRepository repo, IStockRepository stockRepo)
        {
            _repo = repo;
            _stockRepo = stockRepo;           
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Share> shares = _repo.Shares.Where(e => e.Date.Year == Convert.ToDateTime(date).Year ? (e.Date.Month == Convert.ToDateTime(date).Month ?
                e.Date.Day <= Convert.ToDateTime(date).Day : e.Date.Month <= Convert.ToDateTime(date).Month) : e.Date.Year <= Convert.ToDateTime(date).Year).ToList();

            List<Stock> stocks = _stockRepo.Stocks.ToList();

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

                foreach (var stock in stocks)
                {
                    Dictionary<DateTime, double> stockData =
                        JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                    var sData = stockData.Keys.Where(x => Convert.ToDateTime(x).Day == valueDate.Day &&
                         Convert.ToDateTime(x).Month == valueDate.Month && Convert.ToDateTime(x).Year == valueDate.Year).FirstOrDefault();

                    if (sData != null && sData != DateTime.MinValue)
                    {
                        List<Share> lineDataShares = shares.Where(e => e.StockID == stock.ID).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Year <= currentDate.Year).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Month <= currentDate.Month).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Day <= currentDate.Day).ToList();
                        int totalShares = lineDataShares.Select(e => e.NumOfShares).Sum();
                        double s = Convert.ToDouble(stockData.GetValueOrDefault(sData));
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

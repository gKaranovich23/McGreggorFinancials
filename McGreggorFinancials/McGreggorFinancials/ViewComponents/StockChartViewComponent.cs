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

            IAvapiConnection connection = AvapiConnection.Instance;
            connection.Connect("Z9HHWNQMIHSAVDKH");

            Int_TIME_SERIES_DAILY time_series_daily = connection.GetQueryObject_TIME_SERIES_DAILY();

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
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    lineData.Add(new LineChartData
                    {
                        XData = currentDate.Day.ToString(),
                        YData = currentValue.ToString()
                    });

                    currentDate = currentDate.AddDays(1);
                }
                else
                {
                    double total = 0;
                    foreach (var stock in stocks)
                    {
                        Dictionary<DateTime, double> stockData =
                            JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                        var sData = stockData.Keys.Where(x => Convert.ToDateTime(x).Day == currentDate.Day &&
                            Convert.ToDateTime(x).Month == currentDate.Month && Convert.ToDateTime(x).Year == currentDate.Year).FirstOrDefault();
                        if (sData != null && sData != DateTime.MinValue)
                        {
                            List<Share> lineDataShares = shares.Where(e => e.StockID == stock.ID &&
                             e.Date.Year == currentDate.Year ? (e.Date.Month == currentDate.Month ? e.Date.Day <= currentDate.Day
                             : e.Date.Month <= currentDate.Month) : e.Date.Year <= currentDate.Year).ToList();
                            int totalShares = lineDataShares.Select(e => e.NumOfShares).Sum();
                            total += Convert.ToDouble(stockData[sData]) * (double)totalShares;
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
            }

            return View(lineData);
        }
    }
}

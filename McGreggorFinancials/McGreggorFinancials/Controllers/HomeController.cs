using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avapi;
using Avapi.AvapiTIME_SERIES_DAILY;
using McGreggorFinancials.Models.Stocks;
using McGreggorFinancials.Models.Stocks.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace McGreggorFinancials.Controllers
{
    public class HomeController : Controller
    {
        private IStockRepository _stockRepo;

        public HomeController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public IActionResult Dashboard()
        {
            string stockDataRetrieved = HttpContext.Session.GetString("StockDataRetrieved");
            if (string.IsNullOrEmpty(stockDataRetrieved))
            {
                RetrieveStockData();
            }

            return View();
        }

        private void RetrieveStockData()
        {
            IAvapiConnection connection = AvapiConnection.Instance;
            connection.Connect("Z9HHWNQMIHSAVDKH");

            Int_TIME_SERIES_DAILY time_series_daily = connection.GetQueryObject_TIME_SERIES_DAILY();

            List<Stock> stocks = _stockRepo.Stocks.ToList();

            foreach (var stock in stocks)
            {
                IAvapiResponse_TIME_SERIES_DAILY time_series_dailyResponse = time_series_daily.Query(
                    stock.Ticker,
                    Const_TIME_SERIES_DAILY.TIME_SERIES_DAILY_outputsize.compact
                    );

                Dictionary<DateTime, double> stockData = new Dictionary<DateTime, double>();

                foreach (var d in time_series_dailyResponse.Data.TimeSeries)
                {
                    stockData.Add(Convert.ToDateTime(d.DateTime), Convert.ToDouble(d.open ?? d.close));
                }

                string stockDataString = JsonConvert.SerializeObject(stockData);
                HttpContext.Session.SetString(stock.Ticker, stockDataString);

            }

            HttpContext.Session.SetString("StockDataRetrieved", "true");
        }
    }
}
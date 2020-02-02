using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avapi;
using Avapi.AvapiDIGITAL_CURRENCY_DAILY;
using Avapi.AvapiTIME_SERIES_DAILY;
using McGreggorFinancials.Models.Crypto;
using McGreggorFinancials.Models.Crypto.Repository;
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
        private ICryptoCurrencyRepository _cryptoRepo;

        public HomeController(IStockRepository stockRepo, ICryptoCurrencyRepository cryptoRepo)
        {
            _stockRepo = stockRepo;
            _cryptoRepo = cryptoRepo;
        }

        public IActionResult Dashboard()
        {
            string investmentDataRetrieved = HttpContext.Session.GetString("InvestmentDataRetrieved");
            if (string.IsNullOrEmpty(investmentDataRetrieved))
            {
                IAvapiConnection connection = AvapiConnection.Instance;
                connection.Connect("BXGO930UI9P053HT");

                string stockDataRetrieved = HttpContext.Session.GetString("StockDataRetrieved");
                if(string.IsNullOrEmpty(stockDataRetrieved))
                {
                    RetrieveStockData(connection);
                }

                string cryptoDataRetrieved = HttpContext.Session.GetString("CryptoDataRetrieved");
                if(string.IsNullOrEmpty(cryptoDataRetrieved))
                {
                    RetrieveCryptoData(connection);
                }

                HttpContext.Session.SetString("InvestmentDataRetrieved", "true");
            }

            return View();
        }

        private void RetrieveStockData(IAvapiConnection connection)
        {
            Int_TIME_SERIES_DAILY time_series_daily = connection.GetQueryObject_TIME_SERIES_DAILY();

            List<Stock> stocks = _stockRepo.Stocks.ToList();

            foreach (var stock in stocks)
            {
                try
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
                catch(Exception e)
                {

                }

            }

            HttpContext.Session.SetString("StockDataRetrieved", "true");
        }

        private void RetrieveCryptoData(IAvapiConnection connection)
        {
            Int_DIGITAL_CURRENCY_DAILY time_series_daily = connection.GetQueryObject_DIGITAL_CURRENCY_DAILY();

            List<CryptoCurrency> stocks = _cryptoRepo.CryptoCurrencies.ToList();

            foreach (var stock in stocks)
            {
                try
                {
                    IAvapiResponse_DIGITAL_CURRENCY_DAILY time_series_dailyResponse = time_series_daily.QueryPrimitive(
                        stock.Ticker,
                        "USD"
                    );

                    Dictionary<DateTime, double> stockData = new Dictionary<DateTime, double>();

                    foreach (var d in time_series_dailyResponse.Data.TimeSeries)
                    {
                        stockData.Add(Convert.ToDateTime(d.DateTime), Convert.ToDouble(d.OpenUSD ?? d.CloseUSD));
                    }

                    string stockDataString = JsonConvert.SerializeObject(stockData);
                    HttpContext.Session.SetString(stock.Ticker, stockDataString);
                }
                catch(Exception e)
                {

                }

            }

            HttpContext.Session.SetString("CryptoDataRetrieved", "true");
        }
    }
}
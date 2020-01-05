using McGreggorFinancials.Models.Stocks;
using McGreggorFinancials.Models.Stocks.Repository;
using McGreggorFinancials.ViewModels.Stocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class StocksViewComponent : ViewComponent
    {
        private IShareRepository _repo;
        private IStockRepository _stockRepo;

        public StocksViewComponent(IShareRepository repo, IStockRepository stockRepo)
        {
            _repo = repo;
            _stockRepo = stockRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Share> shares = _repo.Shares.Where(e => e.Date.Year == Convert.ToDateTime(date).Year ? (e.Date.Month == Convert.ToDateTime(date).Month ?
                e.Date.Day <= Convert.ToDateTime(date).Day : e.Date.Month <= Convert.ToDateTime(date).Month) : e.Date.Year <= Convert.ToDateTime(date).Year).ToList();

            List<StockViewModel> svm = new List<StockViewModel>();
            List<Stock> stocks = _stockRepo.Stocks.ToList();

            foreach (var stock in stocks)
            {
                Dictionary<DateTime, double> stockData =
                    JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                var sData = stockData.First();

                List<Share> listOfShares = shares.Where(e => e.StockID == stock.ID).ToList();
                int totalShares = listOfShares.Select(e => e.NumOfShares).Sum();

                svm.Add(new StockViewModel()
                {
                    Stock = stock,
                    TotalNumOfShares = totalShares,
                    CurrentValue = Convert.ToDouble(sData.Value),
                    TotalValue = Convert.ToDouble(sData.Value) * (double)totalShares
                });
            }

            return View(svm);
        }
    }
}

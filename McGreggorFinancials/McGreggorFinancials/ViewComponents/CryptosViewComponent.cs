using McGreggorFinancials.Models.Crypto;
using McGreggorFinancials.Models.Crypto.Repository;
using McGreggorFinancials.ViewModels.Crypto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class CryptosViewComponent : ViewComponent
    {
        private ICoinRepository _repo;
        private ICryptoCurrencyRepository _stockRepo;

        public CryptosViewComponent(ICoinRepository repo, ICryptoCurrencyRepository stockRepo)
        {
            _repo = repo;
            _stockRepo = stockRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Coin> shares = _repo.Coins.Where(e => e.Date.Year == Convert.ToDateTime(date).Year ? (e.Date.Month == Convert.ToDateTime(date).Month ?
                e.Date.Day <= Convert.ToDateTime(date).Day : e.Date.Month <= Convert.ToDateTime(date).Month) : e.Date.Year <= Convert.ToDateTime(date).Year).ToList();

            List<CryptoViewModel> svm = new List<CryptoViewModel>();
            List<CryptoCurrency> stocks = _stockRepo.CryptoCurrencies.ToList();

            foreach (var stock in stocks)
            {
                Dictionary<DateTime, double> stockData =
                    JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                var sData = stockData.First();

                List<Coin> listOfShares = shares.Where(e => e.CryptoCurrencyID == stock.ID).ToList();
                int totalShares = listOfShares.Select(e => e.NumOfCoins).Sum();

                svm.Add(new CryptoViewModel()
                {
                    Currency = stock,
                    TotalNumOfCoins = totalShares,
                    CurrentValue = Convert.ToDouble(sData.Value),
                    TotalValue = Convert.ToDouble(sData.Value) * (double)totalShares
                });
            }

            return View(svm);
        }
    }
}

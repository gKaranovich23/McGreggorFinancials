using Avapi;
using Avapi.AvapiDIGITAL_CURRENCY_DAILY;
using Avapi.AvapiTIME_SERIES_DAILY;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Crypto;
using McGreggorFinancials.Models.Crypto.Repository;
using McGreggorFinancials.Models.Expenses.Repositories;
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
    public class NetWorthViewComponent : ViewComponent
    {
        private IAccountRepository _saveRepo;
        private IShareRepository _sharesRepo;
        private IStockRepository _stockRepo;
        private IPaymentMethodRepository _payRepo;
        private ICryptoCurrencyRepository _cryptoRepo;
        private ICoinRepository _coinRepo;

        public NetWorthViewComponent(IAccountRepository saveRepo, IShareRepository shareRepo, IPaymentMethodRepository payRepo, 
            IStockRepository stockRepo, ICryptoCurrencyRepository cryptoRepo, ICoinRepository coinRepo)
        {
            _saveRepo = saveRepo;
            _sharesRepo = shareRepo;
            _payRepo = payRepo;
            _stockRepo = stockRepo;
            _cryptoRepo = cryptoRepo;
            _coinRepo = coinRepo;
        }

        public IViewComponentResult Invoke()
        {
            double netWorth = _saveRepo.Accounts.Sum(x => x.Amount);
            netWorth -= _payRepo.PaymentMethods.Where(x => x.IsCredit).Sum(x => x.CreditBalance.Amount);

            IAvapiConnection connection = AvapiConnection.Instance;
            connection.Connect("BXGO930UI9P053HT");

            Int_TIME_SERIES_DAILY time_series_daily = connection.GetQueryObject_TIME_SERIES_DAILY();
            List<Share> shares = _sharesRepo.Shares.ToList();
            List<Stock> stocks = _stockRepo.Stocks.ToList();

            try
            {
                foreach (var stock in stocks)
                {
                    Dictionary<DateTime, double> stockData =
                        JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                    var sData = stockData.First();

                    List<Share> listOfShares = shares.Where(e => e.StockID == stock.ID).ToList();
                    int totalShares = listOfShares.Select(e => e.NumOfShares).Sum();
                    netWorth += Convert.ToDouble(sData.Value) * (double)totalShares;
                }
            }
            catch(Exception e)
            {

            }

            Int_DIGITAL_CURRENCY_DAILY crypto_series_daily = connection.GetQueryObject_DIGITAL_CURRENCY_DAILY();

            List<CryptoCurrency> cryptos = _cryptoRepo.CryptoCurrencies.ToList();
            List<Coin> coins = _coinRepo.Coins.ToList();

            try
            {
                foreach (var crypto in cryptos)
                {
                    Dictionary<DateTime, double> cryptoData =
                        JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(crypto.Ticker));

                    var sData = cryptoData.First();

                    List<Coin> coinList = coins.Where(e => e.CryptoCurrencyID == crypto.ID).ToList();
                    decimal totalCoins = coinList.Select(e => e.NumOfCoins).Sum();
                    netWorth += Convert.ToDouble(sData.Value) * (double)totalCoins;
                }
            }
            catch (Exception e)
            {

            }

            netWorth = Math.Round(netWorth);

            return View(netWorth);
        }
    }
}

using Avapi;
using Avapi.AvapiTIME_SERIES_DAILY;
using McGreggorFinancials.Models.Accounts.Repositories;
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

        public NetWorthViewComponent(IAccountRepository saveRepo, IShareRepository shareRepo, IPaymentMethodRepository payRepo, 
            IStockRepository stockRepo)
        {
            _saveRepo = saveRepo;
            _sharesRepo = shareRepo;
            _payRepo = payRepo;
            _stockRepo = stockRepo;
        }

        public IViewComponentResult Invoke()
        {
            double netWorth = _saveRepo.Accounts.Sum(x => x.Amount);
            netWorth -= _payRepo.PaymentMethods.Where(x => x.IsCredit).Sum(x => x.CreditBalance.Amount);

            IAvapiConnection connection = AvapiConnection.Instance;
            connection.Connect("Z9HHWNQMIHSAVDKH");

            Int_TIME_SERIES_DAILY time_series_daily = connection.GetQueryObject_TIME_SERIES_DAILY();
            List<Share> shares = _sharesRepo.Shares.ToList();
            List<Stock> stocks = _stockRepo.Stocks.ToList();

            foreach (var stock in stocks)
            {
                Dictionary<DateTime, double> stockData =
                    JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                var sData = stockData.First();

                List<Share> listOfShares = shares.Where(e => e.StockID == stock.ID).ToList();
                int totalShares = listOfShares.Select(e => e.NumOfShares).Sum();
                netWorth += Convert.ToDouble(sData.Value) * (double)totalShares;
            }

            return View(netWorth);
        }
    }
}

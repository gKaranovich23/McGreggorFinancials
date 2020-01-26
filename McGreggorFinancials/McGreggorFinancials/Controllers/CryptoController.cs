using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avapi;
using Avapi.AvapiDIGITAL_CURRENCY_MONTHLY;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Crypto;
using McGreggorFinancials.Models.Crypto.Repository;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.Models.Targets;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.Crypto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace McGreggorFinancials.Controllers
{
    public class CryptoController : Controller
    {
        private ICoinRepository _repo;
        private ICryptoCurrencyRepository _stockRepo;
        private ITargetAmountRepository _goalRepo;
        private ITargetTypeRepository _goalTypeRepo;
        private IAccountRepository _saveRepo;
        private IAccountTypeRepository _saveTypeRepo;
        private IIncomeEntryRespository _incomeRepo;

        public CryptoController(ICoinRepository repo, ICryptoCurrencyRepository stockRepo,
            ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo,
            IAccountRepository saveRepo, IAccountTypeRepository saveTypeRepo, IIncomeEntryRespository incomeRepo)
        {
            _repo = repo;
            _stockRepo = stockRepo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
            _saveRepo = saveRepo;
            _saveTypeRepo = saveTypeRepo;
            _incomeRepo = incomeRepo;
        }

        public ViewResult Create()
        {
            ViewBag.FormTitle = "Create Coins";

            return View("Edit", new CoinFormViewModel
            {
                Coin = new Coin(),
                CryptoCurrencies = new SelectList(_stockRepo.CryptoCurrencies.ToList(), "ID", "Name"),
                ReturnUrl = Request.Headers["Referer"].ToString()
            });
        }

        public ViewResult Edit(int coinId)
        {
            ViewBag.FormTitle = "Edit Coins";

            return View(new CoinFormViewModel
            {
                Coin = _repo.Coins.FirstOrDefault(e => e.ID == coinId),
                CryptoCurrencies = new SelectList(_stockRepo.CryptoCurrencies.ToList(), "ID", "Name"),
                ReturnUrl = Request.Headers["Referer"].ToString()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoinFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();

                if (model.Coin.ID == 0)
                {
                    account.Amount = (double)account.Amount - (model.Coin.PurchasePrice * (double)model.Coin.NumOfCoins);
                }
                else
                {
                    Coin s = _repo.Coins.Where(x => x.ID == model.Coin.ID).FirstOrDefault();
                    account.Amount = (double)account.Amount + (s.PurchasePrice * (double)s.NumOfCoins);
                    account.Amount = (double)account.Amount - (model.Coin.PurchasePrice * (double)model.Coin.NumOfCoins);
                }

                _repo.Save(model.Coin);
                _saveRepo.Save(account);
                TempData["message"] = $"Coin #{model.Coin.ID} has been saved";
                return Redirect(model.ReturnUrl);
            }
            else
            {
                if (model.Coin.ID == 0)
                {
                    ViewBag.FormTitle = "Create Coins";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Coins";
                }

                model.CryptoCurrencies = new SelectList(_stockRepo.CryptoCurrencies.ToList(), "ID", "Name");
                return View(model);
            }
        }

        public ViewResult Sell(int coinId)
        {
            Coin s = _repo.Coins.Where(x => x.CryptoCurrencyID == coinId).FirstOrDefault();
            s.NumOfCoins = 0;

            return View("Sell", new CoinFormViewModel
            {
                Coin = s,
                CryptoCurrencies = new SelectList(_stockRepo.CryptoCurrencies.ToList(), "ID", "Name"),
                ReturnUrl = Request.Headers["Referer"].ToString()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sell(CoinFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
                account.Amount = (double)account.Amount + (model.Coin.PurchasePrice * (double)model.Coin.NumOfCoins);

                model.Coin.NumOfCoins = -model.Coin.NumOfCoins;
                _repo.Save(model.Coin);
                _saveRepo.Save(account);
                TempData["message"] = $"Coins have been sold";
                return Redirect(model.ReturnUrl);
            }
            else
            {
                ViewBag.FormTitle = "Sell Coins";

                model.CryptoCurrencies = new SelectList(_stockRepo.CryptoCurrencies.ToList(), "ID", "Name");
                return View(model);
            }
        }

        public ViewResult MonthlyReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<Coin> shares = _repo.Coins.Where(e => e.Date.Year == Convert.ToDateTime(date).Year ? (e.Date.Month == Convert.ToDateTime(date).Month ?
                e.Date.Day <= Convert.ToDateTime(date).Day : e.Date.Month <= Convert.ToDateTime(date).Month) : e.Date.Year <= Convert.ToDateTime(date).Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<CryptoViewModel> svm = new List<CryptoViewModel>();
            List<CryptoCurrency> stocks = _stockRepo.CryptoCurrencies.ToList();

            foreach (var stock in stocks)
            {
                Dictionary<DateTime, double> stockData =
                    JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock.Ticker));

                var sData = stockData.First();

                List<Coin> listOfShares = shares.Where(e => e.CryptoCurrencyID == stock.ID).ToList();
                decimal totalShares = listOfShares.Select(e => e.NumOfCoins).Sum();

                svm.Add(new CryptoViewModel()
                {
                    Currency = stock,
                    TotalNumOfCoins = totalShares,
                    CurrentValue = Convert.ToDouble(sData.Value),
                    TotalValue = Convert.ToDouble(sData.Value) * (double)totalShares
                });
            }

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
                        List<Coin> lineDataShares = shares.Where(e => e.CryptoCurrencyID == stock.ID).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Year <= currentDate.Year).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Month <= currentDate.Month).ToList();
                        lineDataShares = lineDataShares.Where(e => e.Date.Day <= currentDate.Day).ToList();
                        decimal totalShares = lineDataShares.Select(e => e.NumOfCoins).Sum();
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

            List<int> sectors = _stockRepo.CryptoCurrencies.Select(x => x.ID).Distinct().ToList();
            foreach (var sector in sectors)
            {
                List<Coin> sectorShares = shares.Where(e => e.Date.Year == currentDate.Year ? (e.Date.Month == currentDate.Month
                    ? e.Date.Day <= currentDate.Day : e.Date.Month <= currentDate.Month) : e.Date.Year <= currentDate.Year).ToList();
                decimal totalShares = sectorShares.Where(e => e.CryptoCurrency.ID == sector).Select(e => e.NumOfCoins).Sum();
                data.Add(new PieChartData
                {
                    Category = _stockRepo.CryptoCurrencies.Where(x => x.ID == sector).FirstOrDefault().Name,
                    Data = Convert.ToString(totalShares)
                });
            }

            double amountInvested = 0;
            List<Coin> monthlyShares = shares.Where(e => e.Date.Month == currentDate.Month && e.Date.Year == currentDate.Year).ToList();
            foreach (var s in monthlyShares)
            {
                amountInvested += (double)s.PurchasePrice * (double)s.NumOfCoins;
            }

            List<IncomeEntry> incomes = _incomeRepo.IncomeEntries.Where(i => i.Date.Month == date.Value.Month && i.Date.Year == date.Value.Year).ToList();

            TargetAmount investmentGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Investments")).FirstOrDefault();
            TargetAmount cryptoGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Crypto")).FirstOrDefault();

            decimal targetInvestmentAmount = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum()) * investmentGoal.Percentage / 100;

            return View(new CryptoListViewModel
            {
                Stocks = svm,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                AmountInvested = Convert.ToDecimal(shares.Select(e => (double)e.NumOfCoins * e.PurchasePrice).Sum()),
                CryptoGoal = targetInvestmentAmount * cryptoGoal.Percentage / 100,
                CryptoPercentage = cryptoGoal.Percentage
            });
        }

        [HttpPost]
        public RedirectToActionResult MonthlyReport(string dateStr)
        {
            DateTime date = Convert.ToDateTime(dateStr);

            return RedirectToAction("MonthlyReport", new { date });
        }

        public ViewResult YearlyReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            IAvapiConnection connection = AvapiConnection.Instance;
            connection.Connect("Z9HHWNQMIHSAVDKH");

            Int_DIGITAL_CURRENCY_MONTHLY time_series_monthly = connection.GetQueryObject_DIGITAL_CURRENCY_MONTHLY();

            List<Coin> shares = _repo.Coins.Where(e => e.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<CryptoViewModel> svm = new List<CryptoViewModel>();
            List<CryptoCurrency> stocks = _stockRepo.CryptoCurrencies.ToList();

            Dictionary<int, IAvapiResponse_DIGITAL_CURRENCY_MONTHLY> stockData = new Dictionary<int, IAvapiResponse_DIGITAL_CURRENCY_MONTHLY>();

            foreach (var stock in stocks)
            {
                IAvapiResponse_DIGITAL_CURRENCY_MONTHLY time_series_monthlyResponse = time_series_monthly.QueryPrimitive(
                    stock.Ticker,
                    "USD"
                    );

                stockData.Add(stock.ID, time_series_monthlyResponse);

                var sData = time_series_monthlyResponse.Data.TimeSeries.First();

                List<Coin> listOfShares = shares.Where(e => e.CryptoCurrencyID == stock.ID).ToList();
                decimal totalShares = listOfShares.Select(e => e.NumOfCoins).Sum();

                svm.Add(new CryptoViewModel()
                {
                    Currency = stock,
                    TotalNumOfCoins = totalShares,
                    CurrentValue = Convert.ToDouble(sData.CloseUSD),
                    TotalValue = Convert.ToDouble(sData.CloseUSD) * (double)totalShares
                });
            }

            List<LineChartData> lineData = new List<LineChartData>();

            int month = 1;
            int year = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            double currentValue = 0;

            while (month <= currentMonth)
            {
                double total = 0;
                foreach (var stock in stocks)
                {
                    IAvapiResponse_DIGITAL_CURRENCY_MONTHLY time_series_monthlyResponse = stockData.GetValueOrDefault(stock.ID);

                    var sData = time_series_monthlyResponse.Data.TimeSeries.Where(x => Convert.ToDateTime(x.DateTime).Month == month
                        && Convert.ToDateTime(x.DateTime).Year == year).FirstOrDefault();
                    if (sData != null)
                    {
                        List<Coin> listOfShares = shares.Where(e => e.CryptoCurrencyID == stock.ID && e.Date.Month <= month).ToList();
                        decimal totalShares = listOfShares.Select(e => e.NumOfCoins).Sum();
                        total += Convert.ToDouble(sData.Close) * (double)totalShares;
                    }
                }

                currentValue = total;

                lineData.Add(new LineChartData
                {
                    XData = DateTimeFormatInfo.CurrentInfo.GetMonthName(month).Substring(0, 3),
                    YData = currentValue.ToString()
                });

                month++;
            }

            List<int> sectors = _stockRepo.CryptoCurrencies.Select(x => x.ID).Distinct().ToList();
            foreach (var sector in sectors)
            {
                List<Coin> sectorShares = shares.Where(e => e.Date.Year == date.Value.Year ? (e.Date.Month <= date.Value.Month)
                    : e.Date.Year <= date.Value.Year).ToList();
                decimal totalShares = sectorShares.Where(e => e.CryptoCurrency.ID == sector).Select(e => e.NumOfCoins).Sum();
                data.Add(new PieChartData
                {
                    Category = _stockRepo.CryptoCurrencies.Where(x => x.ID == sector).FirstOrDefault().Name,
                    Data = Convert.ToString(totalShares)
                });
            }

            double amountInvested = 0;
            foreach (var s in shares)
            {
                amountInvested += (double)s.PurchasePrice * (double)s.NumOfCoins;
            }

            List<IncomeEntry> incomes = _incomeRepo.IncomeEntries.Where(i => i.Date.Month == date.Value.Month && i.Date.Year == date.Value.Year).ToList();
            int daysInMonth = DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
            double sum = 0;

            for (int j = 1; j <= daysInMonth; j++)
            {
                sum += incomes.Where(i => i.Date.Day == j).Select(e => e.Amount).Sum();
            }

            return View(new CryptoListViewModel
            {
                Stocks = svm,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                AmountInvested = Convert.ToDecimal(shares.Select(e => (double)e.NumOfCoins * e.PurchasePrice).Sum())
            });
        }

        [HttpPost]
        public RedirectToActionResult YearlyReport(string dateStr)
        {
            DateTime date = new DateTime(int.Parse(dateStr), 1, 1);

            return RedirectToAction("YearlyReport", new { date });
        }

        public IActionResult CoinLog()
        {
            List<Coin> shares = _repo.Coins.ToList();
            return View(shares);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avapi;
using Avapi.AvapiTIME_SERIES_MONTHLY;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.Models.Stocks;
using McGreggorFinancials.Models.Stocks.Repository;
using McGreggorFinancials.Models.Targets;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.Stocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace McGreggorFinancials.Controllers
{
    public class StockController : Controller
    {
        private IShareRepository _repo;
        private IStockRepository _stockRepo;
        private ITargetAmountRepository _goalRepo;
        private ITargetTypeRepository _goalTypeRepo;
        private ISectorRepository _sectorRepo;
        private IAccountRepository _saveRepo;
        private IAccountTypeRepository _saveTypeRepo;
        private IIncomeEntryRespository _incomeRepo;

        public StockController(IShareRepository repo, IStockRepository stockRepo,
            ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo, ISectorRepository sectorRepo,
            IAccountRepository saveRepo, IAccountTypeRepository saveTypeRepo, IIncomeEntryRespository incomeRepo)
        {
            _repo = repo;
            _stockRepo = stockRepo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
            _sectorRepo = sectorRepo;
            _saveRepo = saveRepo;
            _saveTypeRepo = saveTypeRepo;
            _incomeRepo = incomeRepo;
        }

        public ViewResult Create()
        {
            ViewBag.FormTitle = "Create Shares";

            return View("Edit", new ShareFormViewModel
            {
                Share = new Share(),
                Stocks = new SelectList(_stockRepo.Stocks.ToList(), "ID", "Company"),
                ReturnUrl = Request.Headers["Referer"].ToString()
            });
        }

        public ViewResult Edit(int shareId)
        {
            ViewBag.FormTitle = "Edit Shares";

            return View(new ShareFormViewModel
            {
                Share = _repo.Shares.FirstOrDefault(e => e.ID == shareId),
                Stocks = new SelectList(_stockRepo.Stocks.ToList(), "ID", "Company"),
                ReturnUrl = Request.Headers["Referer"].ToString()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ShareFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();

                if (model.Share.ID == 0)
                {
                    account.Amount = (double)account.Amount - (model.Share.PurchasePrice * model.Share.NumOfShares);
                }
                else
                {
                    Share s = _repo.Shares.Where(x => x.ID == model.Share.ID).FirstOrDefault();
                    account.Amount = (double)account.Amount + (s.PurchasePrice * s.NumOfShares);
                    account.Amount = (double)account.Amount - (model.Share.PurchasePrice * model.Share.NumOfShares);
                }

                _repo.Save(model.Share);
                _saveRepo.Save(account);
                TempData["message"] = $"Share #{model.Share.ID} has been saved";
                return Redirect(model.ReturnUrl);
            }
            else
            {
                if (model.Share.ID == 0)
                {
                    ViewBag.FormTitle = "Create Shares";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Shares";
                }

                model.Stocks = new SelectList(_stockRepo.Stocks.ToList(), "ID", "Company");
                return View(model);
            }
        }

        public ViewResult Sell(int stockId)
        {
            Share s = _repo.Shares.Where(x => x.StockID == stockId).FirstOrDefault();
            s.NumOfShares = 0;

            return View("Sell", new ShareFormViewModel
            {
                Share = s,
                Stocks = new SelectList(_stockRepo.Stocks.ToList(), "ID", "Company"),
                ReturnUrl = Request.Headers["Referer"].ToString()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sell(ShareFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
                account.Amount = (double)account.Amount + (model.Share.PurchasePrice * model.Share.NumOfShares);

                model.Share.NumOfShares = -model.Share.NumOfShares;
                _repo.Save(model.Share);
                _saveRepo.Save(account);
                TempData["message"] = $"Shares have been sold";
                return Redirect(model.ReturnUrl);
            }
            else
            {
                ViewBag.FormTitle = "Sell Shares";

                model.Stocks = new SelectList(_stockRepo.Stocks.ToList(), "ID", "Company");
                return View(model);
            }
        }

        public ViewResult MonthlyReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<Share> shares = _repo.Shares.Where(e => e.Date.Year == Convert.ToDateTime(date).Year ? (e.Date.Month == Convert.ToDateTime(date).Month ?
                e.Date.Day <= Convert.ToDateTime(date).Day : e.Date.Month <= Convert.ToDateTime(date).Month) : e.Date.Year <= Convert.ToDateTime(date).Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
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
                    foreach (var stock2 in stocks)
                    {
                        Dictionary<DateTime, double> stockData =
                            JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(HttpContext.Session.GetString(stock2.Ticker));

                        var sData = stockData.Keys.Where(x => Convert.ToDateTime(x).Day == currentDate.Day &&
                             Convert.ToDateTime(x).Month == currentDate.Month && Convert.ToDateTime(x).Year == currentDate.Year).FirstOrDefault();

                        if (sData != null && sData != DateTime.MinValue)
                        {
                            List<Share> lineDataShares = shares.Where(e => e.StockID == stock2.ID &&
                             e.Date.Year == currentDate.Year ? (e.Date.Month == currentDate.Month ? e.Date.Day <= currentDate.Day
                             : e.Date.Month <= currentDate.Month) : e.Date.Year <= currentDate.Year).ToList();
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
            }

            List<int> sectors = _stockRepo.Stocks.Select(x => x.SectorID).Distinct().ToList();
            foreach (var sector in sectors)
            {
                List<Share> sectorShares = shares.Where(e => e.Date.Year == currentDate.Year ? (e.Date.Month == currentDate.Month
                    ? e.Date.Day <= currentDate.Day : e.Date.Month <= currentDate.Month) : e.Date.Year <= currentDate.Year).ToList();
                int totalShares = sectorShares.Where(e => e.Stock.SectorID == sector).Select(e => e.NumOfShares).Sum();
                data.Add(new PieChartData
                {
                    Category = _sectorRepo.Sectors.Where(x => x.ID == sector).FirstOrDefault().Name,
                    Data = Convert.ToString(totalShares)
                });
            }

            double amountInvested = 0;
            List<Share> monthlyShares = shares.Where(e => e.Date.Month == currentDate.Month && e.Date.Year == currentDate.Year).ToList();
            foreach (var s in monthlyShares)
            {
                amountInvested += (double)s.PurchasePrice * (double)s.NumOfShares;
            }

            List<IncomeEntry> incomes = _incomeRepo.IncomeEntries.Where(i => i.Date.Month == date.Value.Month && i.Date.Year == date.Value.Year).ToList();

            TargetAmount investmentGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Investments")).FirstOrDefault();
            TargetAmount stockGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Stock")).FirstOrDefault();
            TargetAmount goldGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Gold")).FirstOrDefault();
            TargetAmount bondGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Bond")).FirstOrDefault();

            decimal targetInvestmentAmount = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum());

            return View(new StockListViewModel
            {
                Stocks = svm,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                AmountInvested = Convert.ToDecimal(shares.Select(e => e.NumOfShares * e.PurchasePrice).Sum()),
                StockGoal = targetInvestmentAmount * stockGoal.Percentage / 100,
                GoldGoal = targetInvestmentAmount * goldGoal.Percentage / 100,
                BondGoal = targetInvestmentAmount * bondGoal.Percentage / 100,
                StockGoalPercentage = stockGoal.Percentage,
                GoldGoalPercentage = goldGoal.Percentage,
                BondGoalPercentage = bondGoal.Percentage
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

            Int_TIME_SERIES_MONTHLY time_series_monthly = connection.GetQueryObject_TIME_SERIES_MONTHLY();

            List<Share> shares = _repo.Shares.Where(e => e.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<StockViewModel> svm = new List<StockViewModel>();
            List<Stock> stocks = _stockRepo.Stocks.ToList();

            Dictionary<int, IAvapiResponse_TIME_SERIES_MONTHLY> stockData = new Dictionary<int, IAvapiResponse_TIME_SERIES_MONTHLY>();

            foreach (var stock in stocks)
            {
                IAvapiResponse_TIME_SERIES_MONTHLY time_series_monthlyResponse = time_series_monthly.QueryPrimitive(
                    stock.Ticker
                    );

                stockData.Add(stock.ID, time_series_monthlyResponse);

                var sData = time_series_monthlyResponse.Data.TimeSeries.First();

                List<Share> listOfShares = shares.Where(e => e.StockID == stock.ID).ToList();
                int totalShares = listOfShares.Select(e => e.NumOfShares).Sum();

                svm.Add(new StockViewModel()
                {
                    Stock = stock,
                    TotalNumOfShares = totalShares,
                    CurrentValue = Convert.ToDouble(sData.close),
                    TotalValue = Convert.ToDouble(sData.close) * (double)totalShares
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
                    IAvapiResponse_TIME_SERIES_MONTHLY time_series_monthlyResponse = stockData.GetValueOrDefault(stock.ID);

                    var sData = time_series_monthlyResponse.Data.TimeSeries.Where(x => Convert.ToDateTime(x.DateTime).Month == month
                        && Convert.ToDateTime(x.DateTime).Year == year).FirstOrDefault();
                    if (sData != null)
                    {
                        List<Share> listOfShares = shares.Where(e => e.StockID == stock.ID && e.Date.Month <= month).ToList();
                        int totalShares = listOfShares.Select(e => e.NumOfShares).Sum();
                        total += Convert.ToDouble(sData.close) * (double)totalShares;
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

            List<int> sectors = _stockRepo.Stocks.Select(x => x.SectorID).Distinct().ToList();
            foreach (var sector in sectors)
            {
                List<Share> sectorShares = shares.Where(e => e.Date.Year == date.Value.Year ? (e.Date.Month <= date.Value.Month)
                    : e.Date.Year <= date.Value.Year).ToList();
                int totalShares = sectorShares.Where(e => e.Stock.SectorID == sector).Select(e => e.NumOfShares).Sum();
                data.Add(new PieChartData
                {
                    Category = _sectorRepo.Sectors.Where(x => x.ID == sector).FirstOrDefault().Name,
                    Data = Convert.ToString(totalShares)
                });
            }

            double amountInvested = 0;
            foreach (var s in shares)
            {
                amountInvested += (double)s.PurchasePrice * (double)s.NumOfShares;
            }

            List<IncomeEntry> incomes = _incomeRepo.IncomeEntries.Where(i => i.Date.Month == date.Value.Month && i.Date.Year == date.Value.Year).ToList();
            int daysInMonth = DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
            double sum = 0;

            for (int j = 1; j <= daysInMonth; j++)
            {
                sum += incomes.Where(i => i.Date.Day == j).Select(e => e.Amount).Sum();
            }

            TargetAmount investmentGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Investments")).FirstOrDefault();
            TargetAmount stockGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Stock")).FirstOrDefault();
            TargetAmount goldGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Gold")).FirstOrDefault();
            TargetAmount bondGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Bond")).FirstOrDefault();

            decimal targetInvestmentAmount = (decimal)(sum * investmentGoal.Percentage / 100);

            return View(new StockListViewModel
            {
                Stocks = svm,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                AmountInvested = Convert.ToDecimal(shares.Select(e => e.NumOfShares * e.PurchasePrice).Sum())
            });
        }

        [HttpPost]
        public RedirectToActionResult YearlyReport(string dateStr)
        {
            DateTime date = new DateTime(int.Parse(dateStr), 1, 1);

            return RedirectToAction("YearlyReport", new { date });
        }

        public IActionResult SharesLog()
        {
            List<Share> shares = _repo.Shares.ToList();
            return View(shares);
        }

    }
}
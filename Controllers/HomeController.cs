using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using My5Paisa.Models;
using Hangfire;
using System.Globalization;
using System.Threading;
using Cronos;

namespace My5Paisa.Controllers
{
    public class TaskManager
    {
        public TaskManager() { }
        public static void GetPositions()
        {
            SessionManager.Instance.GetNetPositions();
            SessionManager.Instance.GetOrderBook();
            SessionManager.Instance.GetMargin();
        }

        public static void Scan(string id)
        {
            StrategyManager.GetById(id).Scan();
        }
        public static void Execute(string id)
        {
            StrategyManager.GetById(id).Execute();
        }

        public static void NewDay()
        {
            OrderManager.NewDay();
        }
    }
    public class HomeController : Controller
    {
        static HomeController()
        {
            string newDay = "0 7 * * MON-FRI";
            RecurringJob.AddOrUpdate(() => TaskManager.NewDay(), newDay, INDIAN_ZONE);
            RecurringJob.AddOrUpdate(() => TaskManager.GetPositions(), Cron.Minutely, INDIAN_ZONE);
            foreach (var item in StrategyManager.AllStrategies)
            {
                RecurringJob.AddOrUpdate("Scan-" + item.Name, () => TaskManager.Scan(item.Id), item.ScanCronExpression, INDIAN_ZONE);
                RecurringJob.AddOrUpdate("Execute-" + item.Name, () => TaskManager.Execute(item.Id), item.ExecuteCronExpression, INDIAN_ZONE);
            }
        }


        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hi-IN");
            _logger = logger;
        }


        public IActionResult Index()
        {
            // SessionManager.Instance.ScanScripts();
            return View();
        }

        public IActionResult GoLive()
        {
            SessionManager.Instance.IsLive = true;
            return RedirectToAction("Index");
        }

        public IActionResult Stop()
        {
            SessionManager.Instance.IsLive = false;
            return RedirectToAction("Index");
        }

        public IActionResult Nifty50()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

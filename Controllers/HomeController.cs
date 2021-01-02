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

namespace My5Paisa.Controllers
{
    public class TaskManager
    {
        public TaskManager() { }
        public static void GetPositions()
        {
            OrderManager.Instance.BalanceOrders();
        }

    }
    public class HomeController : Controller
    {
        static HomeController()
        {
            RecurringJob.AddOrUpdate(() => TaskManager.GetPositions(), Cron.Minutely);
            foreach (var item in StrategyManager.AllStrategies)
            {
                RecurringJob.AddOrUpdate(() => item.Run(), item.CronExpression, INDIAN_ZONE);

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

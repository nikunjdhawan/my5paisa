using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using My5Paisa.Models;
using Hangfire;

namespace My5Paisa.Controllers
{
    public class TaskManager
    {
        public TaskManager() { }
        public static void GetPositions()
        {
            OrderManager.Instance.BalanceOrders();
        }
        public static void ScanScripts()
        {
            SessionManager.Instance.ScanScripts();
        }
    }
    public class HomeController : Controller
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            string cron = Cron.Daily(9, 8);
            RecurringJob.AddOrUpdate(() => TaskManager.GetPositions(), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => TaskManager.ScanScripts(), Cron.Daily(9, 8), INDIAN_ZONE);
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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

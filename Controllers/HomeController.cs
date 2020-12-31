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
        public static void AddMessage()
        {
            SessionManager.Instance.AddMessage(SessionManager.Instance.Messages.Count + " : hangfire job " + DateTime.Now.TimeOfDay);
        }
        public static void GetPositions()
        {
            SessionManager.Instance.GetNetPositions();
        }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            SessionManager.Instance.AddMessage(SessionManager.Instance.Messages.Count + " : Before hangfire job " + DateTime.Now.TimeOfDay);
            BackgroundJob.Enqueue(() => TaskManager.AddMessage());
            RecurringJob.AddOrUpdate(() => TaskManager.GetPositions(), Cron.Minutely);
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

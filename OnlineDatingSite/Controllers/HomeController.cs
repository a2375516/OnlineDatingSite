using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineDatingSite.Models;

namespace OnlineDatingSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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

        public IActionResult PersonalInformation()
        {
            return View();
        }
        /*public IActionResult PersonalInformation(string username, string password, string name, string nickname, string sex, string address, string birth, string email, string job, string interest, Bitmap personalphoto)
        {
            ViewData["username"] = username;
            ViewData["password"] = password;
            ViewData["name"] = name;
            ViewData["nickname"] = nickname;
            ViewData["sex"] = sex;
            ViewData["address"] = address;
            ViewData["birth"] = birth;
            ViewData["email"] = email;
            ViewData["job"] = job;
            ViewData["interest"] = interest;
            ViewData["personalphoto"] = personalphoto;
            SqlUpdate update = new SqlUpdate();

            return View(model: update);
        }*/
    }
}

using Microsoft.AspNetCore.Mvc;
using Oficcios360.Models;
using System.Diagnostics;

namespace Oficcios360.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}

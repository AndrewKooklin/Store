using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //YandexKassa/Home/Callback
        public IActionResult Callback()
        {
            return View();
        }
    }
}

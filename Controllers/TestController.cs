using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core_MVC.Controllers
{
    
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult emp()
        {
            return PartialView("Index","Test");
        }

        

        int id=1;
        string name="Saihl";
        public string Display()
        {
            return "Welcome "+name+" is "+id;
        }

        public ContentResult msg()
        {
            var name1="<h1>Hello Sahil</h1>";
            return Content(name1,"text/html");
        }

        public IActionResult sahil()
        {
            var name1="<script>alert('Hello Sahil')</script>";
            return Content(name1,"text/html");
        }

        public JsonResult data()
        {
            var data="[{'ID':'1001'},{'Name':'sahil'}]";
            return Json(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
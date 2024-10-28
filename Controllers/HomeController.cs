using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Core_MVC.Models;

namespace Core_MVC.Controllers;

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
     public IActionResult emp()
    {
            return View("Index","Test");
    }

    public IActionResult ResultView()
    {
            return View("~/View/Home/Privacy.cshtml/");
    }

    public IActionResult Result1()
    {
            return RedirectToAction("Index","Test");
    }

    public RedirectToRouteResult Result()
    {
        return RedirectToRoute(new {Controller = "Test", action = "AboutUs"});
    }

    public JsonResult result3()
    {
        return Json(new { name = "Sahil", age = 21 });
    }

    public int value(int id)
    {
        return id;
    }

    public JsonResult std()
    {
        var student = new List<std>
        {
            new std{id=1,name="Sahil"},
            new std{id=2,name="Rahul"}
        };
        return Json(student);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

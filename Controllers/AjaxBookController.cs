using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core_MVC.BAL;
using Core_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core_MVC.Controllers
{
    //[Route("[controller]")]
    public class AjaxBookController : Controller
    {
        private readonly ILogger<AjaxBookController> _logger;
        
        private readonly BookHelper _bookHelper;

        public AjaxBookController(ILogger<AjaxBookController> logger, BookHelper bookHelper)
        {
            _logger = logger;
            _bookHelper = bookHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GellAllBook()
        {
            var books = _bookHelper.getBook();
            return Json(books);
        }

        [HttpPost]
        public JsonResult AddBook([FromBody] BookModel book)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Message = "Invalid data." });
            }


            _bookHelper.insertBook(book);
            return Json(new { Success = true, Message = "Book Added" });
        }

        [HttpGet]
        public JsonResult GetBookDetails(int id)
        {
            var book = _bookHelper.getSelectBook(id);
            return Json(book);
        }


        [HttpPost]
        public JsonResult UpdateBookData([FromBody] BookModel book)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Message = "Invalid data." });
            }


            _bookHelper.UpdateBook(book);
            return Json(new { Success = true, Message = "Book Updated" });
        }

        [HttpPost]
        public JsonResult DeleteBook(int id)
        {
            _bookHelper.deleteBook(id);
            return Json(new { Success = true, Message = "Book Deleted" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
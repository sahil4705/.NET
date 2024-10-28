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
   // [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly BookHelper _bookHelper;

        public BookController(ILogger<BookController> logger, BookHelper bh)
        {
            _logger = logger;
            _bookHelper = bh;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var books = _bookHelper.getBook();
            return View(books);
        }

        public IActionResult addBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult addBook(BookModel bookModel)
        {
            // if (ModelState.IsValid)
            // {
            //     _bookHelper.insertBook(bookModel);
            //     return RedirectToAction("Index");
            // }
            // return View();   

            if(!ModelState.IsValid)
            {
                return View(bookModel);
            }
            string msg = _bookHelper.insertBook(bookModel);
            TempData["msg"] = msg;
            return RedirectToAction("Index");
        }

        public IActionResult getDeleteBook(int id)
        {
            var bookDelete = _bookHelper.getSelectBook(id);
            return View(bookDelete);
        }

        public IActionResult DeleteBook(int id)
        {
            string result = _bookHelper.deleteBook(id);
            TempData["msg"] = result;
            return RedirectToAction("Index");
        }

        public IActionResult getUpdateBook(int id)
        {
            var bookDelete = _bookHelper.getSelectBook(id);
            return View(bookDelete);
        }

        public IActionResult UpdateBook(BookModel bookModel)
        {
            if(!ModelState.IsValid)
            {
                return View("getUpdateBook");
            }
            string reuslt = _bookHelper.UpdateBook(bookModel);
            TempData["msg"] = reuslt;
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
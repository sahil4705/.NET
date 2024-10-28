using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Core_MVC.BAL;
using Core_MVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace Core_MVC.Controllers
{
    //[Route("[controller]")]
    public class AjaxUserController : Controller
    {
        private readonly ILogger<AjaxUserController> _logger;

        private readonly UserHelper _userHelper;

        public AjaxUserController(ILogger<AjaxUserController> logger, UserHelper userHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                user.c_password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.c_password)));
                if (!_userHelper.getExistEmail(user.c_email))
                {
                    _userHelper.addUser(user);
                    return Json("Registration Successfully");
                }
                else
                {
                    return Json("Email is already registered");
                }
            }
            return Json("false");
        }

        [HttpPost]
        public IActionResult Login(Login user)
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine("Username: " + user.c_username);
                user.c_password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.c_password)));
                bool existingUser = _userHelper.checkLogin(user.c_username, user.c_password);
                if (existingUser == false)
                {
                    TempData["msg"] = "Invalid username or password";
                    return Json("Invalid username or password");
                }
                else
                {
                        HttpContext.Session.SetString("user", user.c_username);
                    return Json("Login successful");
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult checkEmailExist(string email)
        {
            HttpContext.Session.SetString("Email", email);
            if (!ModelState.IsValid) return View("getEmailForgotPassword");
            if (!_userHelper.getExistEmail(email))
            {
                return Json("Email does not exist");
            }
             _userHelper.sendMail(email,"");
            return Json("Check Your Email");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
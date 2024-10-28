using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core_MVC.BAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core_MVC.Models;
using System.Security.Cryptography;
using System.Text;
using AspNetCoreGeneratedDocument;

namespace Core_MVC.Controllers
{
    //[Route("[controller]")]
    public class MVCUserController : Controller
    {
        private readonly ILogger<MVCUserController> _logger;
        private readonly UserHelper _userHelper;

        public MVCUserController(ILogger<MVCUserController> logger, UserHelper userHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.c_password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.c_password)));
                if (!_userHelper.getExistEmail(user.c_email))
                {
                    _userHelper.addUser(user);

                    TempData["Reg_msg"] = "Registration Successfully";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["msg"] = "Email is already registered";
                    return View();
                }
            }
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login user)
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine("email: " + user.c_username);
                user.c_password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.c_password)));
                bool existingUser = _userHelper.checkLogin(user.c_username, user.c_password);
                if (existingUser == false)
                {
                    TempData["msg"] = "Invalid username or password";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["msg"] = "Login successful";
                    HttpContext.Session.SetString("user", user.c_username);
                    return RedirectToAction("GetIntern", "MVCIntern");
                }
            }
            return View();
        }

        public IActionResult getEmailForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult checkEmailExist(string email)
        {
            HttpContext.Session.SetString("Email", email);
            if (!ModelState.IsValid) return View("getEmailForgotPassword");
            if (!_userHelper.getExistEmail(email))
            {
                TempData["msg"] = "Email does not exist";
                return View("getEmailForgotPassword");
            }
            string token = Guid.NewGuid().ToString();
            _userHelper.sendMail(email,token);
            _userHelper.StoreTokenInDatabase(email,token);
            TempData["Send_mail_msg"] = "Check your Email";
            return View("getEmailForgotPassword");
        }

        public IActionResult varifyToken(string email, string token)
        {
            Console.WriteLine("e"+email+" t"+token);
            if (!_userHelper.CheckToken(email, token))
            {
                TempData["msg"] = "Token is invalid or expired";
                return RedirectToAction("Login");
            }
            return View("ForgotPassword");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetEmail(string email)
        {
            if (_userHelper.getExistEmail(email))
            {
                return Json("Email already exists");
            }
            return Json("");
        }


        [HttpPost]
        public IActionResult updatePassword(string NewPassword)
        {
            NewPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(NewPassword)));
            if (!ModelState.IsValid) return View("ForgotPassword");
            string email = HttpContext.Session.GetString("Email")!;
            Console.WriteLine("" + email + " " + NewPassword);
            _userHelper.changePassword(NewPassword, email);
            TempData["Login_msg"] = "Password updated successfully";
            HttpContext.Session.Remove("Email");
            return View("Login");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
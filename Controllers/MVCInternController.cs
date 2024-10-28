using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core_MVC.BAL;
using Core_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Npgsql;

namespace Core_MVC.Controllers
{
    //[Route("[controller]")]
    [ResponseCache(NoStore = true)]
    public class MVCInternController : Controller
    {
        private readonly ILogger<MVCInternController> _logger;
        private readonly InternHelper internHelper;
        public MVCInternController(ILogger<MVCInternController> logger, InternHelper ih)
        {
            _logger = logger;
            internHelper = ih;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetIntern()
        {

            //HttpContext.Session.Remove("user");
            // Console.WriteLine(HttpContext.Session.GetString("user"));

            if (HttpContext.Session.GetString("user") != null)
            {
                ViewBag.user = HttpContext.Session.GetString("user");
                var getInter = internHelper.FetchAllIntern();
                return View(getInter);
            }
            return RedirectToAction("Login", "MVCUser");
        }


        public IActionResult AddIntern()
        {
            List<AssignTaskClass> tasks = new List<AssignTaskClass>();
            string _connectionString = "Server=localhost;Port=5432;Database=Core_Database;User Id=postgres;Password=S@hil@711986;";
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT c_topicid, c_topicname FROM t_assignedtask", connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new AssignTaskClass
                            {
                                c_topicid = (int)reader["c_topicid"],
                                c_topicname = (string)reader["c_topicname"]
                            });
                        }
                    }
                }
            }

            ViewBag.TaskList = new SelectList(tasks, "c_topicid", "c_topicname");
            return View();
        }

        [HttpPost]
        public IActionResult AddIntern(InterClass interClass, IFormFile file)
        {

            if (file != null && file.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var imageUrl = "/images/" + fileName;
                interClass.c_Topic_Image = imageUrl;
            }
            internHelper.AddNewIntern(interClass);

            return View();
        }


        public IActionResult GetOneIntern(int id)
        {
            //ViewBag.Task = id;
            var details = internHelper.FetchInternDetails(id);
            return View(details);
        }

        public IActionResult GetDeleteIntern(int id)
        {
            var detailsDelete = internHelper.FetchInternDetails(id);
            return View(detailsDelete);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "MVCUser");
        }

        public IActionResult GetUpdateIntern(int id)
        {
            List<AssignTaskClass> tasks = new List<AssignTaskClass>();
            string _connectionString = "Server=localhost;Port=5432;Database=Core_Database;User Id=postgres;Password=S@hil@711986;";
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT c_topicid, c_topicname FROM t_assignedtask", connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new AssignTaskClass()
                            {
                                c_topicid = (int)reader["c_topicid"],
                                c_topicname = (string)reader["c_topicname"]
                            });
                        }
                    }
                }
            }

            ViewBag.TaskList = new SelectList(tasks, "c_topicid", "c_topicname");
            var detailsUpdate = internHelper.FetchInternDetails(id);
            return View(detailsUpdate);
        }


        [HttpPost]
        public IActionResult DeleteIntern(int id)
        {
            internHelper.DeleteExistingIntern(id);
            return RedirectToAction("GetIntern");
        }

        [HttpPost]
        public IActionResult UpdateIntern(InterClass Intern)
        {
            var imageFile = HttpContext.Request.Form.Files.FirstOrDefault();

            if (imageFile != null && imageFile.Length > 0)
            {
                // Define the path where the image will be saved (e.g., "wwwroot/images/")
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                // Create a unique file name for the image to avoid overwriting
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
                Intern.c_Topic_Image = "/images/" + uniqueFileName;
            }
            else
            {
                var existingIntern = internHelper.FetchInternDetails(Intern.c_internid1);
                if (existingIntern != null)
                {
                    Intern.c_Topic_Image = existingIntern.c_Topic_Image;
                }
            }

            internHelper.UpdateExistingIntern(Intern);
            return RedirectToAction("GetIntern");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
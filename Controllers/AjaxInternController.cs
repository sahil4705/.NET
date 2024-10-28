using Core_MVC.BAL;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core_MVC.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Core_MVC.Controllers
{
    public class AjaxInternController : Controller
    {
        private readonly InternHelper _internHelper;
        public AjaxInternController(InternHelper ih)
        {
            this._internHelper = ih;
        }
        public IActionResult Index()
        {
            List<AssignTaskClass> assignTasks = _internHelper.GetTopics();
            ViewBag.topics = assignTasks;
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
            _internHelper.AddNewIntern(interClass);
                
            return Json(new{Success = true});
        }


        [HttpGet]
        public IActionResult getIntern()
        { 
            var intern = _internHelper.FetchAllIntern();
            return Json(intern);
        }

        [HttpPost]
        public IActionResult DeleteIntern(int id)
        {
            _internHelper.DeleteExistingIntern(id);
            return Json(new { Success = true, Message = "Intern Deleted" });
        }


        [HttpGet]
        public IActionResult getUpdateIntern(int id)
        {
            var intern = _internHelper.FetchInternDetails(id);
            return Json(intern);
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
                var existingIntern = _internHelper.FetchInternDetails(Intern.c_internid1);
                if (existingIntern != null)
                {
                    Intern.c_Topic_Image = existingIntern.c_Topic_Image; 
                }
            }

            _internHelper.UpdateExistingIntern(Intern);
            return Json(new {success=true, Message="Updated"});
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "AjaxUser");
        }
    }
}

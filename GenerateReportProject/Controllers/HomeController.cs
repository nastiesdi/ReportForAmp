using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewEmployeeDBFinal.Models;
using Microsoft.AspNetCore.Http;

namespace NewEmployeeDBFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var allUserStories = UsModel.GetAllUsFromJson();
            return View(allUserStories);
        }

        public IActionResult OpenForPrinting()
        {
            var allUserStories = UsModel.GetAllUsFromJson();
            return View("Print", allUserStories);
        }

        public IActionResult Delete(int id)
        {
            UsModel.RemoveUserStory(id);
            var allEmployees = UsModel.GetAllUsFromJson();
            return View("Index", allEmployees);
        }

        public IActionResult LoadFromCsv()
        {
            UsModel.GetAllUsFromCsv();
            var allUserStories = UsModel.GetAllUsFromJson();
            return View("Index", allUserStories);
        }

        public IActionResult Edit(int id)
        {
            var employee = UsModel.SelectUserStory(id);
            return View("Edit", employee);
        }

        public IActionResult Save(UsModel updatedUS)
        {
            UsModel.EditUserStory(updatedUS);
            var allUserStories = UsModel.GetAllUsFromJson();
            return View("Index", allUserStories);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpPost]
        public IActionResult UploadFile(IFormFile image)
        {
            UploadedFile file = new UploadedFile();
            if (image!=null)
            {
                //Set Key Name
                string ImageName= "allUS.csv";

                //Get url To Save
                // string SavePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",ImageName);
                string pathForDoc = Path.Combine( Directory.GetCurrentDirectory(), "Files", ImageName);
                using(var stream=new FileStream(pathForDoc, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
            }
            UsModel.GetAllUsFromCsv();
            var allUserStories = UsModel.GetAllUsFromJson();
            return View("Index", allUserStories);
        }
    }
    
    public class UploadedFile
    {
        public IFormFile MyFile { set; get; }
    }
}

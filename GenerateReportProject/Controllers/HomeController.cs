using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewEmployeeDBFinal.Models;

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
            var allUserStories = USModel.GetAllUSFromJson();
            return View(allUserStories);
        }

        public IActionResult OpenForPrinting()
        {
            var allUserStories = USModel.GetAllUSFromJson();
            return View("Print", allUserStories);
        }

        public IActionResult Delete(int id)
        {
            USModel.RemoveUserStory(id);
            var allEmployees = USModel.GetAllUSFromJson();
            return View("Index", allEmployees);
        }

        public IActionResult LoadFromCsv()
        {
            USModel.GetAllUSFromCsv();
            var allUserStories = USModel.GetAllUSFromJson();
            return View("Index", allUserStories);
        }

        public IActionResult Edit(int id)
        {
            var employee = USModel.SelectUserStory(id);
            return View("Edit", employee);
        }

        public IActionResult Save(USModel updatedUS)
        {
            USModel.EditUserStory(updatedUS);
            var allUserStories = USModel.GetAllUSFromJson();
            return View("Index", allUserStories);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

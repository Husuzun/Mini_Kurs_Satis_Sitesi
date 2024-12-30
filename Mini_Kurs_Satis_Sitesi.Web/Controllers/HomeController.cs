using Microsoft.AspNetCore.Mvc;
using Mini_Kurs_Satis_Sitesi.Web.Models;
using Mini_Kurs_Satis_Sitesi.Web.Services;
using System.Diagnostics;

namespace Mini_Kurs_Satis_Sitesi.Web.Controllers
{
    public class HomeController(CoursesService coursesService) : Controller
    {

        public IActionResult Index()
        {
            var response = coursesService.GetCourses();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

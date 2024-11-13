using Microsoft.AspNetCore.Mvc;

namespace ProyectoTesis.Controllers
{
    public class HomeController : Controller
    {
        // VIEW TO CREATE A BUSINESS LANDING PAGE (IT'S EMPTY)
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Error() => View();
    }
}
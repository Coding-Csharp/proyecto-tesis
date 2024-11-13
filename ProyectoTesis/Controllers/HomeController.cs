using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoTesis.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // VIEW TO CREATE A BUSINESS LANDING PAGE (IT'S EMPTY)
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Error() => View();
    }
}
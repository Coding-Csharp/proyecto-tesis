using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoTesis.Controllers
{
    [AllowAnonymous]
    public class AccessController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated is true)
            {
                if (User.IsInRole("ADMIN"))
                    return RedirectToAction("InterfaceAdmin", "Admins");

                else if (User.IsInRole("EMPLOYEE"))
                    return RedirectToAction("InterfaceEmployee", "Employees");
            }

            return View();
        }
    }
}
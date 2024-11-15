using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using ProyectoTesis.Models;

namespace ProyectoTesis.Controllers
{
    [AllowAnonymous]
    public class AccessController
        (TesisContext context,
        IWebHostEnvironment webHostEnvironment) :
        Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated is true)
            {
                if (User.IsInRole("ADMINISTRADOR"))
                    return RedirectToAction("InterfaceAdmin", "Admins");

                else if (User.IsInRole("TRABAJADOR"))
                    return RedirectToAction("InterfaceEmployee", "Employees");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        public async Task<IActionResult> Login
            (User credential)
        {
            if (credential.Role == "ADMINISTRADOR")
            {
                var result = await context.Set<AdminCredential>()
                    .Where(a => a.AdminsId == credential.Username && a.Code == credential.Password)
                    .FirstOrDefaultAsync();

                if (result == null)
                    return Content(JsonConvert.SerializeObject
                        (false), "application/json");
            }
            else if (credential.Role == "TRABAJADOR")
            {
                var result = await context.Set<EmployeeCredential>()
                    .Where(a => a.EmployeesId == credential.Username && a.Code == credential.Password)
                    .FirstOrDefaultAsync();

                if (result == null)
                    return Content(JsonConvert.SerializeObject
                        (false), "application/json");
            }

            List<Claim> claims =
            [
                new(ClaimTypes.Role, credential.Role ?? string.Empty),
                new(ClaimTypes.Name, credential.Username ?? string.Empty)
            ];

            ClaimsIdentity claimsIdentity = new(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }
    }
}
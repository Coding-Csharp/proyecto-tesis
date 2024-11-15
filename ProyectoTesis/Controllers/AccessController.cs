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

        [HttpPost]
        public async Task<IActionResult> Login
            (dynamic credential)
        {
            string role = credential.Role;
            string id = credential.Id;
            string code = credential.Code;

            dynamic? result = role switch
            {
                "ADMINISTRADOR" =>
                await context.Set<AdminCredential>()
                .Where(a => a.AdminsId == id && a.Code == code)
                .FirstOrDefaultAsync(),

                "TRABAJADOR" =>
                await context.Set<EmployeeCredential>()
                .Where(a => a.EmployeesId == id && a.Code == code)
                .FirstOrDefaultAsync(),

                _ => null
            };

            if (result is null)
                return Content(JsonConvert.SerializeObject
                    (false), "application/json");

            List<Claim> claims =
            [
                new(ClaimTypes.Role, role),
                new(ClaimTypes.Name, id)
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
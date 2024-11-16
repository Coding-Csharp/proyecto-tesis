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
        private ClaimsPrincipal? _claimsPrincipal;
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
                var result = await
                    (from ac in context.Set<AdminCredential>()
                     join ad in context.Set<Admin>()
                     on ac.AdminsId equals ad.Id
                     where ac.AdminsId == credential.Username &&
                     ac.Code == credential.Password &&
                     ad.State == "ACTIVO"
                     select ad
                    ).FirstOrDefaultAsync();

                if (result == null)
                    return Content(JsonConvert.SerializeObject
                        (false), "application/json");
            }
            else if (credential.Role == "TRABAJADOR")
            {
                var result = await
                    (from ec in context.Set<EmployeeCredential>()
                     join em in context.Set<Employee>()
                     on ec.EmployeesId equals em.Id
                     where ec.EmployeesId == credential.Username &&
                     ec.Code == credential.Password &&
                     em.State == "ACTIVO"
                     select em
                    ).FirstOrDefaultAsync();

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

        [HttpGet]
        public async Task<IActionResult> GetInformation()
        {
            _claimsPrincipal = HttpContext.User;

            var role = _claimsPrincipal
                .FindFirst(ClaimTypes.Role)?
                .Value.ToString() ?? string.Empty;

            var personId = _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;

            if (role == "ADMINISTRADOR")
            {
                var admin = await context.Set<Admin>()
                    .Where(a => a.Id == personId)
                    .FirstOrDefaultAsync();

                return Content(JsonConvert.SerializeObject
                    (admin), "application/json");
            }
            else if (role == "TRABAJADOR")
            {
                var employee = await context.Set<Employee>()
                    .Where(a => a.Id == personId)
                    .FirstOrDefaultAsync();

                return Content(JsonConvert.SerializeObject
                    (employee), "application/json");
            }

            return Content(JsonConvert.SerializeObject
                (""), "application/json");
        }
    }
}
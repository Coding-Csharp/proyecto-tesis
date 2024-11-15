using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using ProyectoTesis.Models;

namespace ProyectoTesis.Controllers
{
    //[Authorize(Roles = "ADMINISTRADOR")]
    public class AdminsController
        (TesisContext context) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        [HttpGet]
        public IActionResult InterfaceAdmin() => View();

        [HttpGet]
        public IActionResult ListEmployees() => View();

        [HttpGet]
        public IActionResult AttendanceList() => View();

        [HttpGet]
        public async Task<IActionResult> LoadListEmployees()
        {
            var result = await
                (from em in context.Set<Employee>()
                 join es in context.Set<Specialty>()
                 on em.SpecialtiesId equals es.Id
                 join an in context.Set<Assign>()
                 on em.Id equals an.EmployeesId
                 join po in context.Set<Position>()
                 on an.PositionsId equals po.Id
                 join ar in context.Set<Area>()
                 on po.AreasId equals ar.Id
                 select new
                 {
                     em.Id,
                     em.DateEntry,
                     em.TypeDocument,
                     em.Firstname,
                     em.Lastname,
                     em.Birthdate,
                     em.Nationality,
                     em.Genre,
                     em.Phone,
                     em.Email,
                     em.Address,
                     em.ZoneAccess,
                     Area = ar.Name,
                     Position = po.Name,
                     Specialty = es.Name,
                 }
                ).ToListAsync();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> LoadListAttendances()
        {
            var assists = await context.Set<Assist>()
                .Where(a => a.AdminsId == GetPersonId()).ToListAsync();

            return Content(JsonConvert.SerializeObject
                (assists), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> LoadListAttendancesByEmployee
            (string employeeId)
        {
            var assists = await context.Set<Assist>()
                .Where(a => a.EmployeesId == employeeId)
                .ToListAsync();

            return Content(JsonConvert.SerializeObject
                (assists), "application/json");
        }

        private string GetPersonId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }
    }
}
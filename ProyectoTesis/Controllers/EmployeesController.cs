using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using ProyectoTesis.Models;

namespace ProyectoTesis.Controllers
{
    [Authorize(Roles = "TRABAJADOR")]
    public class EmployeesController
        (TesisContext context) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        [HttpGet]
        public async Task<IActionResult> InterfaceEmployee()
        {
            ViewBag.Profile = await
                (from em in context.Set<Employee>()
                 join es in context.Set<Specialty>()
                 on em.SpecialtiesId equals es.Id
                 join an in context.Set<Assign>()
                 on em.Id equals an.EmployeesId
                 join po in context.Set<Position>()
                 on an.PositionsId equals po.Id
                 join ar in context.Set<Area>()
                 on po.AreasId equals ar.Id
                 where em.Id == GetPersonId()
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
                ).FirstOrDefaultAsync();

            return View();
        }

        [HttpGet]
        public IActionResult AttendanceList() => View();

        [HttpGet]
        public IActionResult Maintenance() => View();

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
                     em.Firstname,
                     em.Lastname,
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
            var currentDate = DateTime.Now;

            var result = await
                (from at in context.Set<Assist>()
                 join em in context.Set<Employee>()
                 on at.EmployeesId equals em.Id
                 where em.Id == GetPersonId()
                 && at.CheckIn.HasValue
                 && at.CheckIn.Value.Month == currentDate.Month
                 && at.CheckIn.Value.Year == currentDate.Year
                 select new
                 {
                     at.Id,
                     em.Firstname,
                     em.Lastname,
                     at.CheckIn,
                     at.CheckOut,
                     at.MinuteLate
                 }
                ).ToListAsync();

            return Content(JsonConvert.SerializeObject(result), "application/json");
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
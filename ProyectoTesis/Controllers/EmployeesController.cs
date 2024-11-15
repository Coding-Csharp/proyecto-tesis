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
                     at.CheckIn.Value.Date,
                     at.CheckIn,
                     at.CheckOut,
                     at.MinuteLate
                 }
                ).ToListAsync();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> MarkAttendance
            (string markType)
        {
            var result = false;

            if (markType == "ENTRADA")
            {
                var today = DateTime.Now.Date;

                var assist = await context.Set<Assist>()
                    .Where(a => a.EmployeesId == GetPersonId() &&
                                a.CheckIn.HasValue &&
                                a.CheckIn.Value.Date == today)
                    .FirstOrDefaultAsync();

                if (assist == null)
                {
                    await context.Set<Assist>().AddAsync
                    (new(null, GetPersonId(), DateTime.Now, null, 0, string.Empty));

                    await context.SaveChangesAsync();

                    result = true;
                }
            }
            else if (markType == "SALIDA")
            {
                var assist = await context.Set<Assist>()
                    .Where(a => a.EmployeesId == GetPersonId() &&
                    a.CheckOut == null).FirstOrDefaultAsync();

                if (assist != null)
                {
                    result = await context.Set<Assist>().Where(a => a.Id == assist.Id)
                    .ExecuteUpdateAsync(a => a
                    .SetProperty(u => u.CheckOut, DateTime.Now)) > 0;
                }
            }

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
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
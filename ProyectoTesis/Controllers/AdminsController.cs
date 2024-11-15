using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using ProyectoTesis.Models;

namespace ProyectoTesis.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class AdminsController
        (TesisContext context) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        [HttpGet]
        public async Task<IActionResult> InterfaceAdmin()
        {
            ViewBag.Admin = await
                (from ad in context.Set<Admin>()
                 join es in context.Set<Specialty>()
                 on ad.SpecialtiesId equals es.Id
                 join an in context.Set<Assign>()
                 on ad.Id equals an.AdminsId
                 join po in context.Set<Models.Position>()
                 on an.PositionsId equals po.Id
                 join ar in context.Set<Area>()
                 on po.AreasId equals ar.Id
                 where ad.Id == GetPersonId()
                 select new
                 {
                     ad.Id,
                     ad.DateEntry,
                     ad.TypeDocument,
                     ad.Firstname,
                     ad.Lastname,
                     ad.Birthdate,
                     ad.Nationality,
                     ad.Genre,
                     ad.Phone,
                     ad.Email,
                     ad.Address,
                     Area = ar.Name,
                     Position = po.Name,
                     Specialty = es.Name,
                 }
                ).FirstOrDefaultAsync();

            return View();
        }

        [HttpGet]
        public IActionResult Maintenance() => View();

        [HttpGet]
        public IActionResult ListEmployees() => View();

        [HttpGet]
        public IActionResult AttendanceList() => View();

        [HttpGet]
        public async Task<IActionResult> LoadSpecialties()
        {
            var result = await context.Set<Specialty>().ToListAsync();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> LoadAreas()
        {
            var result = await context.Set<Area>().ToListAsync();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> LoadPositions(int areasId)
        {
            var result = await context.Set<Models.Position>()
                .Where(p => p.AreasId == areasId).ToListAsync();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> LoadListEmployees()
        {
            var result = await
                (from em in context.Set<Employee>()
                 join es in context.Set<Specialty>()
                 on em.SpecialtiesId equals es.Id
                 join an in context.Set<Assign>()
                 on em.Id equals an.EmployeesId
                 join po in context.Set<Models.Position>()
                 on an.PositionsId equals po.Id
                 join ar in context.Set<Area>()
                 on po.AreasId equals ar.Id
                 where em.State == "ACTIVO"
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
            var currentDate = DateTime.Now;

            var result = await
                (from at in context.Set<Assist>()
                 join ad in context.Set<Admin>()
                 on at.AdminsId equals ad.Id
                 where ad.Id == GetPersonId()
                 && at.CheckIn.HasValue
                 && at.CheckIn.Value.Month == currentDate.Month
                 && at.CheckIn.Value.Year == currentDate.Year
                 select new
                 {
                     at.Id,
                     ad.Firstname,
                     ad.Lastname,
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
        public async Task<IActionResult> LoadListAttendancesByPersonId
            (string personId)
        {
           var result = await
                (from at in context.Set<Assist>()
                 where at.EmployeesId == personId ||
                 at.AdminsId == personId
                 select new
                 {
                     at.Id,
                     at.CheckIn.Value.Date,
                     at.CheckIn,
                     at.CheckOut,
                     at.MinuteLate
                 }).ToListAsync();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPerson
            (Employee employee, [FromQuery] string positionId)
        {
            await context.Set<Employee>().AddAsync(employee);

            await context.SaveChangesAsync();

            await context.Set<Assign>().AddAsync
                (new(null, employee.Id, int.Parse(positionId)));

            await context.SaveChangesAsync();

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePerson
            (Employee employee)
        {
            context.Set<Employee>().Update(employee);

            await context.SaveChangesAsync();

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> DeletePerson
            (string id)
        {
            await context.Set<Employee>().Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
            .SetProperty(u => u.State, "ELIMINADO"));

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
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
                    .Where(a => a.AdminsId == GetPersonId() &&
                                a.CheckIn.HasValue &&
                                a.CheckIn.Value.Date == today)
                    .FirstOrDefaultAsync();

                if (assist == null)
                {
                    await context.Set<Assist>().AddAsync
                    (new(GetPersonId(), null, DateTime.Now, null, 0, string.Empty));

                    await context.SaveChangesAsync();

                    result = true;
                }
            }
            else if (markType == "SALIDA")
            {
                var assist = await context.Set<Assist>()
                    .Where(a => a.AdminsId == GetPersonId() &&
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
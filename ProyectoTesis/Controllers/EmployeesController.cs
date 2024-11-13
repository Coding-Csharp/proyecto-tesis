using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoTesis.Models;

namespace ProyectoTesis.Controllers
{
    //[Authorize(Roles = "TRABAJADOR")]
    public class EmployeesController : Controller
    {
        [HttpGet]
        public IActionResult InterfaceEmployee() => View();

        [HttpGet]
        public IActionResult AttendanceList() => View();

        [HttpGet]
        public IActionResult LoadListAttendances()
        {
            List<Attendance> attendances = [];

            attendances.Add(new(4551, "Aaron", "Alarcon", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(4551, "Aaron", "Alarcon", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(4551, "Aaron", "Alarcon", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(4551, "Aaron", "Alarcon", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(4551, "Aaron", "Alarcon", DateTime.Now, DateTime.Now.AddHours(4), 2));

            return Content(JsonConvert.SerializeObject
                (attendances),"application/json");
        }
    }
}
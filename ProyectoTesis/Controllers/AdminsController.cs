using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoTesis.Models;

namespace ProyectoTesis.Controllers
{
    //[Authorize(Roles = "ADMINISTRADOR")]
    public class AdminsController : Controller
    {
        [HttpGet]
        public IActionResult InterfaceAdmin() => View();

        [HttpGet]
        public IActionResult ListEmployees() => View();

        [HttpGet]
        public IActionResult LoadListEmployees()
        {
            List<Employee> employees = [];

            employees.Add(new(4551, "Aaron", "Alarcon", "Administracion", "Supervisor"));
            employees.Add(new(5644, "Alejandro", "Chacon", "Administracion", "Supervisor"));
            employees.Add(new(3452, "Cristiano", "Meza", "Administracion", "Supervisor"));
            employees.Add(new(8766, "Pedro", "Romero", "Administracion", "Supervisor"));
            employees.Add(new(5678, "Juan", "Perez", "Administracion", "Supervisor"));

            return Content(JsonConvert.SerializeObject
                (employees), "application/json");
        }

        [HttpGet]
        public IActionResult LoadListAttendances(int id)
        {
            List<Attendance> attendances = [];

            attendances.Add(new(4551, "Aaron", "Alarcon", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(5644, "Alejandro", "Chacon", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(3452, "Cristiano", "Meza", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(8766, "Pedro", "Romero", DateTime.Now, DateTime.Now.AddHours(4), 2));
            attendances.Add(new(5678, "Juan", "Perez", DateTime.Now, DateTime.Now.AddHours(4), 2));

            return Content(JsonConvert.SerializeObject
                (attendances.Where(a => a.Id == id).ToList()),
                "application/json");
        }
    }
}
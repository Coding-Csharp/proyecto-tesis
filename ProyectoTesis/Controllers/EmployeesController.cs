using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoTesis.Controllers
{
    //[Authorize(Roles = "TRABAJADOR")]
    public class EmployeesController : Controller
    {
        [HttpGet]
        public IActionResult InterfaceEmployee() => View();

        [HttpGet]
        public IActionResult AttendanceList() => View();
    }
}
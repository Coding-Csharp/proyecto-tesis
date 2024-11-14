namespace ProyectoTesis.Models
{
    public class EmployeesCredential
    {
        public string EmployeesId { get; set; } = null!;
        public string Code { get; set; } = null!;

        public virtual Employee Employee { get; } = null!;
    }
}
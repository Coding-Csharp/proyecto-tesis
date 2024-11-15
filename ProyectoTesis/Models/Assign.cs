namespace ProyectoTesis.Models
{
    public class Assign
    {
        public int Id { get; set; }
        public string? AdminsId { get; set; }
        public string? EmployeesId { get; set; }
        public int PositionsId { get; set; }

        public virtual Admin? Admin { get; }
        public virtual Employee? Employee { get; }
        public virtual Position Position { get; } = null!;

        public Assign()
        {
            this.AdminsId = null;
            this.EmployeesId = null;
            this.PositionsId = 0;
        }
        public Assign
            (string? adminId, string? employeeId,
            int positionId)
        {
            this.AdminsId = adminId;
            this.EmployeesId = employeeId;
            this.PositionsId = positionId;
        }
    }
}
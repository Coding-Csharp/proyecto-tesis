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
    }
}
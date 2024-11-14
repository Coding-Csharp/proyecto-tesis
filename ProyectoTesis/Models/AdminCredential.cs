namespace ProyectoTesis.Models
{
    public class AdminCredential
    {
        public string AdminsId { get; set; } = null!;
        public string Code { get; set; } = null!;

        public virtual Admin Admin { get; } = null!;
    }
}
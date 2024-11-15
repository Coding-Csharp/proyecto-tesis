namespace ProyectoTesis.Models
{
    public class Admin
    {
        public string Id { get; set; } = null!;
        public int SpecialtiesId { get; set; }
        public DateOnly DateEntry { get; set; }
        public string TypeDocument { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public DateOnly Birthdate { get; set; }
        public string Nationality { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public int Phone { get; set; }
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;

        public virtual AdminCredential? AdminCredential { get; }
        public virtual Specialty Specialty { get; } = null!;

        public virtual ICollection<Assign> Assigns { get; } = [];
        public virtual ICollection<Assist> Assists { get; } = [];
    }
}
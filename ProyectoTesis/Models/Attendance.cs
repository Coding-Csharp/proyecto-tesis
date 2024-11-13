namespace ProyectoTesis.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int MinuteLate { get; set; }

        public Attendance()
        {
            this.Id = 0;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.MinuteLate = 0;
        }
        public Attendance
            (int id, string firstName, string lastName,
            DateTime checkIn, DateTime checkOut,
            int minuteLate)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CheckIn = checkIn;
            this.CheckOut = checkOut;
            this.MinuteLate = minuteLate;
        }
    }
}
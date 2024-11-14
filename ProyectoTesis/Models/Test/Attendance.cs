namespace ProyectoTesis.Models.Test
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
            Id = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            MinuteLate = 0;
        }
        public Attendance
            (int id, string firstName, string lastName,
            DateTime checkIn, DateTime checkOut,
            int minuteLate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            CheckIn = checkIn;
            CheckOut = checkOut;
            MinuteLate = minuteLate;
        }
    }
}
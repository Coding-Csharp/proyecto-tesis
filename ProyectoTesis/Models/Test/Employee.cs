namespace ProyectoTesis.Models.Test
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobDepartment { get; set; }
        public string JobPosition { get; set; }

        public Employee()
        {
            Id = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            JobDepartment = string.Empty;
            JobPosition = string.Empty;
        }
        public Employee
            (int id, string firstName, string lastName,
            string jobDepartment, string jobPosition)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            JobDepartment = jobDepartment;
            JobPosition = jobPosition;
        }
    }
}
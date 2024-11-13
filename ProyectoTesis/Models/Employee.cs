namespace ProyectoTesis.Models
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
            this.Id = 0;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.JobDepartment = string.Empty;
            this.JobPosition = string.Empty;
        }
        public Employee
            (int id, string firstName, string lastName,
            string jobDepartment, string jobPosition)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.JobDepartment = jobDepartment;
            this.JobPosition = jobPosition;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace MdawemApp.Models
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string SupervisorName { get; set; }
        public string WorkLocation { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string HomeAddress { get; set; }
        public DateTime StartDate { get; set; }
        public string EmploymentStatus { get; set; }
        public string WorkExperience { get; set; }
    }
}

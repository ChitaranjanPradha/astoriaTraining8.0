using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI.Models
{
    public class Employee
    {
        public long Employeekey { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string CompanyName { get; set; }

        public string DesignationName { get; set; }

        public DateTime JoiningDate { get; set; }

        public string Gender { get; set; }

        public bool ActiveStatus { get; set; }

    }
}

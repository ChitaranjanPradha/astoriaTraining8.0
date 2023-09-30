using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace astoriaTrainingAPI.Models
{
    public partial class EmployeeMaster
    {
        public EmployeeMaster()
        {
            EmployeeAllowanceDetals = new HashSet<EmployeeAllowanceDetals>();
            EmployeeAttendance = new HashSet<EmployeeAttendance>();
        }

        public long EmployeeKey { get; set; }
        public string EmployeeId { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public int EmpCompanyId { get; set; }
        public int EmpDesignationId { get; set; }
        public string EmpGender { get; set; }
        public DateTime EmpJoingDate { get; set; }
        public DateTime? EmpResinationDate { get; set; }
        public decimal EmpHourlySalaryRate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual CompanyMaster EmpCompany { get; set; }
        public virtual DesignatioMaster EmpDesignation { get; set; }
        public virtual ICollection<EmployeeAllowanceDetals> EmployeeAllowanceDetals { get; set; }
        public virtual ICollection<EmployeeAttendance> EmployeeAttendance { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI.Models
{
    public class Allowance
    {
        public long EmployeeKey { get; set; }
        public string EmployeeName { get; set; }
        public int AllowanceId { get; set; }
        public decimal AllowanceAmount { get; set; }
        public DateTime ClockDate { get; set; }
    }
}

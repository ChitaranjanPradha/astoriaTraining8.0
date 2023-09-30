using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using astoriaTrainingAPI.Models;


namespace astoriaTrainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly astoriaTraining80Context _context;

        public DashboardController (astoriaTraining80Context context)
        {
            _context = context;
        }


        [HttpGet("employeecount")]
        public IEnumerable<int> GetEmployeeCount()
        {
            int resignedEmployeeCount = _context.EmployeeMaster.Where(e => e.EmpResinationDate < DateTime.Today).Count();
            int activeEmployeeCount = (_context.EmployeeMaster.Count() - resignedEmployeeCount);

            return new int[]
            {
                resignedEmployeeCount,
                activeEmployeeCount
            };

        }

        [HttpGet("workinghoursperday")]
        public IEnumerable<Dashboard> GetEmpWorkingHours()
        {
            var workingHour = _context.EmployeeAttendance.GroupBy(e => e.ClockDate).Select(e => new Dashboard()
            {
                ClockDate = e.Key.Date,
                WorkingHours = e.Sum(e => e.TimeOut.Hour - e.TimeIn.Hour)
            }).OrderByDescending(e => e.ClockDate).Take(5).ToList();
            return workingHour;
        }

        [HttpGet("salaryperday")]
        public IEnumerable<Dashboard> GetEmployeeHourlySalary()
        {

            var salary = (from att in _context.EmployeeAttendance
                          join em in _context.EmployeeMaster on att.EmployeeKey equals em.EmployeeKey
                          select new { att, em }).GroupBy(e => e.att.ClockDate).Select(e => new Dashboard
                          {
                              ClockDate = e.Key.Date,
                              Salary = e.Sum(e => (e.att.TimeOut.Hour - e.att.TimeIn.Hour) * e.em.EmpHourlySalaryRate)
                          }).OrderByDescending(e => e.ClockDate).Take(5);

            var allowance = (from att in _context.EmployeeAttendance
                             join ead in _context.EmployeeAllowanceDetals on new { att.ClockDate, att.EmployeeKey }
                             equals new { ead.ClockDate, ead.EmployeeKey }
                             into grouping
                             from g in grouping.DefaultIfEmpty()
                             select new { att, g }).GroupBy(e => e.att.ClockDate).Select(e => new Dashboard
                             {
                                 ClockDate = e.Key.Date,
                                 Salary = e.Sum(e => e.g.AllowanceAmount == null ? 0 : e.g.AllowanceAmount)
                             }).OrderByDescending(e => e.ClockDate).Take(5);

            var combinedSalary = (from s in salary
                                  from a in allowance
                                  where s.ClockDate == a.ClockDate
                                  select new Dashboard()
                                  {
                                      ClockDate = s.ClockDate,
                                      Salary = s.Salary + a.Salary
                                  }).ToList();
            return combinedSalary;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI.Models;

namespace astoriaTrainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeeAttendancesController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeAttendancesController(astoriaTraining80Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Employees by passing Date and Company ID
        /// </summary>
        /// <param name="FilterClockDate"> Parameter should be time </param>
        /// <param name="FilterCompanyID"> Integer parameter is required </param>
        /// <returns> Returns list of employees by date  </returns>
        [HttpGet("allattendances")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAllAttendances(string FilterClockDate, int FilterCompanyID)
        {
            if (FilterClockDate == null || FilterCompanyID == null)
            {
                return BadRequest();
            }
            try
            {
                var empAtt = from emp in _context.EmployeeMaster
                             join att in _context.EmployeeAttendance.Where(x => x.ClockDate.Date == Convert.ToDateTime(FilterClockDate).Date)
                             on emp.EmployeeKey equals att.EmployeeKey
                             into grouping
                             from g in grouping.DefaultIfEmpty()
                             where emp.EmpCompanyId == FilterCompanyID && (emp.EmpResinationDate > Convert.ToDateTime(FilterClockDate).Date || string.IsNullOrEmpty(emp.EmpResinationDate.ToString()))
                             select new Attendance
                             {
                                 EmployeeKey = emp.EmployeeKey,
                                 EmployeeId = emp.EmployeeId,
                                 EmployeeName = emp.EmpFirstName + " " + emp.EmpLastName,
                                 ClockDate = FilterClockDate,
                                 TimeIn = g.TimeIn == null ? string.Empty : g.TimeIn.ToString("HH:mm"),
                                 TimeOut = g.TimeOut == null ? string.Empty : g.TimeOut.ToString("HH:mm"),
                                 Remarks = g.Remarks == null ? string.Empty : g.Remarks,
                                 CreationDate = g.CreationDate
                             };

                if ((empAtt.ToListAsync()).Result.Count > 0)
                {
                    return await empAtt.ToListAsync();
                }
                else
                {
                    return NoContent();
                }
 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        // GET: api/EmployeeAttendances
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmployeeAttendance>>> GetEmployeeAttendance()
        {
            if ((_context.EmployeeAttendance.ToListAsync()).Result.Count > 0)
            {
                return await _context.EmployeeAttendance.ToListAsync();
            }
            else
            {
                return NoContent();
            }
        }


        // POST: api/EmployeeAttendances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        /// <summary>
        /// Post Attendance
        /// </summary>
        /// <param name="employeeAttendanceList"> Send data type of attendance </param>
        /// <returns> Return bool </returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> PostEmployeeAttendance(List<EmployeeAttendance> employeeAttendanceList)  
        {
            foreach (var employeeAttendance in employeeAttendanceList)
            {
                if ((string.IsNullOrEmpty(employeeAttendance.Remarks)) ||
                    (employeeAttendance.TimeIn > employeeAttendance.TimeOut))
                {
                    return BadRequest();
                }

                bool isEmpKeyExists = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == employeeAttendance.EmployeeKey && e.ClockDate == employeeAttendance.ClockDate);

                if(isEmpKeyExists == true)
                {
                        employeeAttendance.ModifiedDate = DateTime.Now;
                        _context.Entry(employeeAttendance).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                }
                else
                {
                    employeeAttendance.ModifiedDate = DateTime.Now;
                    employeeAttendance.CreationDate = DateTime.Now;
                    _context.EmployeeAttendance.Add(employeeAttendance);
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteEmployeeAttendance(List<EmployeeAttendance> employeeAttendanceList)
        {
           
            foreach (var employeeAttendance in employeeAttendanceList)
            {
                _context.EmployeeAttendance.Remove(employeeAttendance);
                await _context.SaveChangesAsync();
            }
            return true;
        }


                //   private bool EmployeeAttendanceExists(long id)
                //   {
                //       return _context.EmployeeAttendance.Any(e => e.EmployeeKey == id);
                //   }
    }
    
}

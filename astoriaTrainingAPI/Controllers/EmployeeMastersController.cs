using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace astoriaTrainingAPI.Controllers
{
   
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeeMastersController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeMastersController(astoriaTraining80Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet("allemployees")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var emp = (from em in _context.EmployeeMaster
                       join cm in _context.CompanyMaster on em.EmpCompanyId equals cm.CompanyId
                       join dm in _context.DesignatioMaster on em.EmpDesignationId equals dm.DesignationId
                       select new Employee()
                       {
                           Employeekey = em.EmployeeKey,
                           EmployeeId = em.EmployeeId,
                           EmployeeName = em.EmpFirstName + " " + em.EmpLastName,
                           CompanyName = cm.CompanyName,
                           DesignationName = dm.DesignationName,
                           JoiningDate = em.EmpJoingDate,
                           Gender = em.EmpGender,
                           ActiveStatus = string.IsNullOrEmpty(em.EmpResinationDate.ToString()) || em.EmpResinationDate > DateTime.Now.Date
                           //(em.EmpResinationDate == null ? Convert.ToDateTime("01-01-0001") : Convert.ToDateTime(em.EmpResinationDate)).Date > DateTime.Now.Date || String.IsNullOrEmpty(em.EmpResinationDate.ToString())
                       }).ToListAsync();
            if (emp.Result.Count > 0)
            {
                return await emp;
            }
            else
            {
                return NoContent();
            }
        }

        // GET: api/EmployeeMasters
        /// <summary>
        /// Get employees with full data from the 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployeeMaster()
        {
            return await _context.EmployeeMaster.ToListAsync();
        }

        [HttpGet("getallcompany")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<CompanyMaster>>> GetAllCompany()
        {
            //  return await _context.CompanyMaster.ToListAsync();
            if ((_context.CompanyMaster.ToListAsync()).Result.Count > 0)
            {
                return await _context.CompanyMaster.ToListAsync();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("getalldesignation")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<DesignatioMaster>>> GetAllDesignation()
        {
            if (_context.DesignatioMaster.ToListAsync().Result.Count > 0)
            {
                return await _context.DesignatioMaster.ToListAsync();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("isEmployeeIsExits")]

        public async Task<ActionResult<bool>> GetEmployeeIdExists(string EmployeeID, long EmployeeKey)
        {
            bool isEmployeeIsExits = await _context.EmployeeMaster.AnyAsync(e => e.EmployeeKey != EmployeeKey && e.EmployeeId.Trim().ToLower() == EmployeeID.Trim().ToLower());
            return isEmployeeIsExits;
        }

        [HttpGet("checkEmployeeKeyInUse")]
        public async Task<ActionResult<bool>> GetEmployeeKeyInUse(long EmployeeKey)
        {
            bool isEmployeeKeyIsExitsInEmployeeAttendance = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == EmployeeKey);
            bool isEmployeeKeyIsExitsInEmployeeDetails = await _context.EmployeeAllowanceDetals.AnyAsync(e => e.EmployeeKey == EmployeeKey);
            if (isEmployeeKeyIsExitsInEmployeeAttendance || isEmployeeKeyIsExitsInEmployeeDetails)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // GET: api/EmployeeMasters/5

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<EmployeeMaster>> GetEmployeeMaster(long id)
        {
            var employeeMaster = await _context.EmployeeMaster.FindAsync(id);

            if (employeeMaster == null)
            {
                return NotFound();
            }

            return Ok(employeeMaster);
        }

        // PUT: api/EmployeeMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeMaster>> PutEmployeeMaster(long id, EmployeeMaster employeeMaster)
        {
            if ((id != employeeMaster.EmployeeKey) ||
            (string.IsNullOrEmpty(employeeMaster.EmployeeId)) ||
            (string.IsNullOrEmpty(employeeMaster.EmpFirstName)) ||
            (string.IsNullOrEmpty(employeeMaster.EmpLastName)) ||
            (string.IsNullOrEmpty(employeeMaster.EmpGender)) ||
            (employeeMaster.EmployeeId.Length > 20) ||
            (employeeMaster.EmpFirstName.Length > 100) ||
            (employeeMaster.EmpLastName.Length > 100))
            {
                return BadRequest();
            }

            if (employeeMaster.EmpResinationDate < employeeMaster.EmpJoingDate)
            {
                return BadRequest();
            }
            if ((_context.EmployeeMaster.Any(e => e.EmployeeKey != employeeMaster.EmployeeKey &&
                e.EmployeeId.Trim().ToLower() == employeeMaster.EmployeeId.Trim().ToLower())))
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            employeeMaster.ModifiedDate = DateTime.Now;
            _context.Entry(employeeMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(employeeMaster);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/EmployeeMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeMaster>> PostEmployeeMaster(EmployeeMaster employeeMaster)
        {

            if ((string.IsNullOrEmpty(employeeMaster.EmployeeId)) ||
              (string.IsNullOrEmpty(employeeMaster.EmpFirstName)) ||
              (string.IsNullOrEmpty(employeeMaster.EmpLastName)) ||
              (string.IsNullOrEmpty(employeeMaster.EmpGender)) ||
              (employeeMaster.EmployeeId.Length > 20) ||
              (employeeMaster.EmpFirstName.Length > 100) ||
              (employeeMaster.EmpLastName.Length > 100))

            {
                return BadRequest();
            }

            if (employeeMaster.EmpResinationDate < employeeMaster.EmpJoingDate)
            {
                return BadRequest();
            }

            if (_context.EmployeeMaster.Any(emp => emp.EmployeeId == employeeMaster.EmployeeId))
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            employeeMaster.CreationDate = DateTime.Now;
            employeeMaster.ModifiedDate = DateTime.Now;

            _context.EmployeeMaster.Add(employeeMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeMaster", new { id = employeeMaster.EmployeeKey }, employeeMaster);
        }

        //public object PostEmployeeMaster()
        //{
        //    throw new NotImplementedException();
        //}

        // DELETE: api/EmployeeMasters/5

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeMaster>> DeleteEmployeeMaster(long id)
        {

            var employeeMaster = await _context.EmployeeMaster.FindAsync(id);
            if (employeeMaster == null)
            {
                return NotFound();
            }

            _context.EmployeeMaster.Remove(employeeMaster);
            await _context.SaveChangesAsync();

            return Ok(employeeMaster);
        }

        private bool EmployeeMasterExists(long id)
        {
            return _context.EmployeeMaster.Any(e => e.EmployeeKey == id);
        }
    }
}

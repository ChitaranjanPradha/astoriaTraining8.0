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
    public class EmployeeAllowanceDetalsController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;                      
        public EmployeeAllowanceDetalsController(astoriaTraining80Context context)
        {
            _context = context;
        }

        [HttpGet("gettodayattendance")]
        public async Task<ActionResult<IEnumerable<Allowance>>> GetTodayAttendance()
        {
            var empAtt = from emp in _context.EmployeeMaster
                         join att in _context.EmployeeAttendance.Where(x => x.ClockDate.Date == (DateTime.Today).Date)
                         on emp.EmployeeKey equals att.EmployeeKey
                         select new Allowance
                         {
                             EmployeeKey = emp.EmployeeKey,
                             EmployeeName = emp.EmpFirstName + " " + emp.EmpLastName
                         };
            return await empAtt.ToListAsync();
        }

        [HttpGet("gettodaywhodoesnothaveallowance")]
        public async Task<ActionResult<IEnumerable<Allowance>>> GetTodayWhoDoesNotHaveAllowance()
        {
            var empWithAllowance = from em in _context.EmployeeMaster.Where(e => e.EmpResinationDate > DateTime.Now.Date ||
                                    string.IsNullOrEmpty(e.EmpResinationDate.ToString()))
                                   join allow in _context.EmployeeAllowanceDetals.Where(e => e.ClockDate == DateTime.Now.Date)
                                   on em.EmployeeKey equals allow.EmployeeKey
                                   into grouping
                                   from g in grouping.DefaultIfEmpty()
                                   select new Allowance
                                   {
                                       EmployeeName = em.EmpFirstName + " " + em.EmpLastName,
                                       EmployeeKey = em.EmployeeKey,
                                       ClockDate = g.ClockDate,
                                       AllowanceId = g.AllowanceId,
                                       AllowanceAmount = g.AllowanceAmount.ToString() == null ? 0 : g.AllowanceAmount
                                   };

            var empWithNoAllowanc = await empWithAllowance.Where(e => e.AllowanceAmount == 0).ToListAsync();
            if(empWithNoAllowanc.Count > 0)
            {
                return empWithNoAllowanc;
            }
            else
            {
                return NoContent();
            }
        }
        /// <summary>
        /// This Methode is used to add a New user into user info table
        /// </summary>
        /// <param name="userInfo">Parameter Should be userInfo type, Paramer shuld be contains UserName,firstname,LastName,Email,Password</param>
        /// <returns></returns>

        [HttpPost("UserInfoPost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            try
            {
                if (
                  (string.IsNullOrEmpty(userInfo.UserName)) ||
                  (string.IsNullOrEmpty(userInfo.FirstName)) ||
                  (string.IsNullOrEmpty(userInfo.LastName)) ||
                  (string.IsNullOrEmpty(userInfo.Email)) ||
                  (string.IsNullOrEmpty(userInfo.Password)))
                {
                    return BadRequest();
                }
                userInfo.CreationDate = DateTime.Now;

                _context.UserInfo.Add(userInfo);
                await _context.SaveChangesAsync();
                return CreatedAtAction("UserInfo", new { id = userInfo.UserId }, userInfo);
            }
            catch(Exception ex)
            {
               throw new Exception(StatusCodes.Status500InternalServerError.ToString());
            }
        }


        [HttpGet("getallallowances")]
        public async Task<ActionResult<IEnumerable<AllowanceMaster>>> GetAllAllowances()
        {

                var allow = await _context.AllowanceMaster.ToListAsync();
            if(allow != null)
            {
                return Ok(allow);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("isIdPasswordCurrect")]
        public async Task<ActionResult<UserInfo>> GetChekIdPassCurrect(string UserName, string Password)
        {
            try
            {
                var isIdPassCurrect = await _context.UserInfo.FirstAsync(e => e.UserName == UserName && e.Password == Password);
                return isIdPassCurrect;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("isIdPassCurrect")]
        public async Task<ActionResult<bool>> GetCheckIdPass(string userName, string Pass)
        {
            bool iscurrect = await _context.UserInfo.AnyAsync(e => e.UserName == userName && e.Password == Pass);
            return iscurrect;
        }

        // GET: api/EmployeeAllowanceDetals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeAllowanceDetals>>> GetEmployeeAllowanceDetals()
        {
            return await _context.EmployeeAllowanceDetals.ToListAsync();
        }

        // GET: api/EmployeeAllowanceDetals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeAllowanceDetals>> GetEmployeeAllowanceDetals(long id)
        {
            var employeeAllowanceDetals = await _context.EmployeeAllowanceDetals.FindAsync(id);

            if (employeeAllowanceDetals == null)
            {
                return NotFound();
            }

            return employeeAllowanceDetals;
        }

        // PUT: api/EmployeeAllowanceDetals/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAllowanceDetals(long id, EmployeeAllowanceDetals employeeAllowanceDetals)
        {
            if (id != employeeAllowanceDetals.EmployeeKey)
            {
                return BadRequest();
            }

            _context.Entry(employeeAllowanceDetals).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAllowanceDetalsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmployeeAllowanceDetals
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<bool>> PostEmployeeAllowanceDetals(List<EmployeeAllowanceDetals> employeeAllowanceDetalsList)
        {
            foreach (EmployeeAllowanceDetals empAllow in employeeAllowanceDetalsList)
            {
                if (empAllow.AllowanceAmount > 0)
                {
                    EmployeeAllowanceDetals empAlowance = _context.EmployeeAllowanceDetals.Where(e => e.EmployeeKey == empAllow.EmployeeKey
                    && e.AllowanceId == empAllow.AllowanceId && e.ClockDate == empAllow.ClockDate).FirstOrDefault();
                    if (empAlowance != null)
                    {
                        empAlowance.AllowanceAmount = empAllow.AllowanceAmount;
                        empAlowance.ModifiedDate = DateTime.Now;
                        _context.Entry(empAlowance).State = EntityState.Modified;
                    }
                    else
                    {
                        empAllow.CreationDate = empAllow.ModifiedDate = DateTime.Now;
                        _context.EmployeeAllowanceDetals.Add(empAllow);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        // DELETE: api/EmployeeAllowanceDetals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeAllowanceDetals>> DeleteEmployeeAllowanceDetals(long id)
        {
            var employeeAllowanceDetals = await _context.EmployeeAllowanceDetals.FindAsync(id);
            if (employeeAllowanceDetals == null)
            {
                return NotFound();
            }

            _context.EmployeeAllowanceDetals.Remove(employeeAllowanceDetals);
            await _context.SaveChangesAsync();

            return employeeAllowanceDetals;
        }

        private bool EmployeeAllowanceDetalsExists(long id)
        {
            return _context.EmployeeAllowanceDetals.Any(e => e.EmployeeKey == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using test2.Models;

namespace test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewersController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private IConfiguration _configuration;
        private IDbContextTransaction _transaction;

        public InterviewersController(SurveyDBContext context, IConfiguration iconfig, bool embeddedTransaction = false, IDbContextTransaction? contextTransaction = null)
        {
            _context = context;
            _configuration = iconfig;
            _transaction = embeddedTransaction ? contextTransaction! : context.Database.BeginTransaction();
        }

        // GET: api/Interviewers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interviewers>>> GetInterviewers()
        {
          if (_context.Interviewers == null)
          {
              return NotFound();
          }
            return await _context.Interviewers.ToListAsync();
        }

        // GET: api/QAndAs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<QAndA>>> GetQAndA(int id)
        {
            if (_context.QAndA == null)
            {
                return NotFound();
            }
            var qAndA = await _context.QAndA.Where(q => q.Sdid == id).ToListAsync();

            if (qAndA.Last() == null)
            {
                return NotFound();
            }

            return qAndA;
        }

        // PUT: api/Interviewers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterviewers(int id, string apikey, Interviewers interviewers)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            interviewers.InterviewerId = id;

            if (id != interviewers.InterviewerId)
            {
                return BadRequest();
            }

            _context.Entry(interviewers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transaction.RollbackAsync();
                if (!InterviewersExists(id))
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

        // POST: api/Interviewers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{apikey}")]
        public async Task<ActionResult<Interviewers>> PostInterviewers(IEnumerable<Interviewers> interviewers, string apikey)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("GeneralSurveyKey"))) return Unauthorized("Please supply valid API key");
            
            try {
                foreach (Interviewers inter in interviewers)
                {
                    if (_context.Interviewers == null)
                    {
                        return Problem("Entity set 'SurveyDBContext.Interviewers'  is null.");
                    }
                    _context.Interviewers.Add(inter);
                }

                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                await _transaction.RollbackAsync();
                return BadRequest(e);
            }
        }

        private bool InterviewersExists(int id)
        {
            return (_context.Interviewers?.Any(e => e.InterviewerId == id)).GetValueOrDefault();
        }
    }
}

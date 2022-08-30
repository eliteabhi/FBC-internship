using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using test2.Models;

namespace test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QAndAsController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private IConfiguration _configuration;
        private IDbContextTransaction _transaction;

        public QAndAsController(SurveyDBContext context, IConfiguration iconfig, bool embeddedTransaction = false, IDbContextTransaction? contextTransaction = null)
        {
            _context = context;
            _configuration = iconfig;
            _transaction = embeddedTransaction ? contextTransaction! : context.Database.BeginTransaction();
        }

        // GET: api/QAndAs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QAndA>>> GetQAndA()
        {
          if (_context.QAndA == null)
          {
              return NotFound();
          }
            return await _context.QAndA.ToListAsync();
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

        // PUT: api/QAndAs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQAndA(int id, string apikey, QAndA qAndA)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            qAndA.SurveyId = id;
            if (id != qAndA.QaRecord)
            {
                return BadRequest();
            }

            _context.Entry(qAndA).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transaction.RollbackAsync();
                if (!QAndAExists(id))
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

        // POST: api/QAndAs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{apikey}")]
        public async Task<ActionResult<QAndA>> PostQAndA(IEnumerable<QAndA> qAndAs, string apikey)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("GeneralSurveyKey"))) return Unauthorized("Please supply valid API key");

            try
            {
                foreach (QAndA qa in qAndAs)
                {
                    if (_context.QAndA == null)
                    {
                        return Problem("Entity set 'SurveyDBContext.QAndA'  is null.");
                    }
                    _context.QAndA.Add(qa);

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

        private bool QAndAExists(int id)
        {
            return (_context.QAndA?.Any(e => e.QaRecord == id)).GetValueOrDefault();
        }
    }
}

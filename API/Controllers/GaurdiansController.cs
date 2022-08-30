using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using test2.Models;

namespace test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaurdiansController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private IConfiguration _configuration;
        private IDbContextTransaction _transaction;

        public GaurdiansController(SurveyDBContext context, IConfiguration iconfig, bool embeddedTransaction = false, IDbContextTransaction? contextTransaction = null)
        {
            _context = context;
            _configuration = iconfig;
            _transaction = embeddedTransaction ? contextTransaction! : context.Database.BeginTransaction();
        }

        // GET: api/Gaurdians
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gaurdians>>> GetGaurdians()
        {
          if (_context.Gaurdians == null)
          {
              return NotFound();
          }
            return await _context.Gaurdians.ToListAsync();
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

        // PUT: api/Gaurdians/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGaurdians(int id, string apikey, Gaurdians gaurdians)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            gaurdians.GaurdianId = id;

            if (id != gaurdians.GaurdianId)
            {
                return BadRequest();
            }

            _context.Entry(gaurdians).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transaction.RollbackAsync();
                if (!GaurdiansExists(id))
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

        // POST: api/Gaurdians
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{apikey}")]
        public async Task<ActionResult<Gaurdians>> PostGaurdians(IEnumerable<Gaurdians> gaurdians, string apikey)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("GeneralSurveyKey"))) return Unauthorized("Please supply valid API key");

            try {
                foreach (Gaurdians gaurd in gaurdians)
                {
                    if (_context.Gaurdians == null)
                    {
                        return Problem("Entity set 'SurveyDBContext.Gaurdians'  is null.");
                    }
                    _context.Gaurdians.Add(gaurd);

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

        private bool GaurdiansExists(int id)
        {
            return (_context.Gaurdians?.Any(e => e.GaurdianId == id)).GetValueOrDefault();
        }
    }
}

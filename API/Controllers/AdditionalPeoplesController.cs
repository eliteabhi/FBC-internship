using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using test2.Models;

namespace test2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalPeoplesController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private IConfiguration _configuration;
        private IDbContextTransaction _transaction;

        public AdditionalPeoplesController(SurveyDBContext context, IConfiguration iconfig, bool embeddedTransaction = false, IDbContextTransaction? contextTransaction = null)
        {
            _context = context;
            _configuration = iconfig;
            _transaction = embeddedTransaction ? contextTransaction! : context.Database.BeginTransaction();
        }

        // GET: api/AdditionalPeoples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdditionalPeople>>> GetAdditionalPeople()
        {
          if (_context.AdditionalPeople == null)
          {
              return NotFound();
          }
            return await _context.AdditionalPeople.ToListAsync();
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

        // PUT: api/AdditionalPeoples/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdditionalPeople(int id, string apikey, AdditionalPeople additionalPeople)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            additionalPeople.AdditionalId = id;

           if (id != additionalPeople.AdditionalId)
            {
                return BadRequest();
            }

            _context.Entry(additionalPeople).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transaction.RollbackAsync();
                if (!AdditionalPeopleExists(id))
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
       
        // POST: api/AdditionalPeoples
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{apikey}")]
        public async Task<ActionResult<AdditionalPeople>> PostAdditionalPeople(IEnumerable<AdditionalPeople> additionalPeople, string apikey)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("GeneralSurveyKey"))) return Unauthorized("Please supply valid API key");

            try
            {
                foreach (AdditionalPeople ap in additionalPeople)
                {

                    if (_context.AdditionalPeople == null)
                    {
                        return Problem("Entity set 'SurveyDBContext.AdditionalPeople'  is null.");
                    }
                    _context.AdditionalPeople.Add(ap);

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

       private bool AdditionalPeopleExists(int id)
        {
            return (_context.AdditionalPeople?.Any(e => e.AdditionalId == id)).GetValueOrDefault();
        }
    }
}

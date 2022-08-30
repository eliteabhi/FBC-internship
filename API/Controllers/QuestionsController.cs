using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using test2.Models;

namespace test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private IConfiguration _configuration;
        private IDbContextTransaction _transaction;

        public QuestionsController(SurveyDBContext context, IConfiguration iconfig)
        {
            _context = context;
            _configuration = iconfig;
            _transaction = context.Database.BeginTransaction();
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestions()
        {
          if (_context.Questions == null)
          {
              return NotFound();
          }
            return await _context.Questions.ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> GetQuestions(int id)
        {
          if (_context.Questions == null)
          {
              return NotFound();
          }
            var questions = await _context.Questions.FindAsync(id);

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        // GET: api/Questions/array
        [HttpGet("array")]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestionsArray([FromQuery]IEnumerable<int> ids)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var questions = await _context.Questions.Where(r => ids.Contains(r.QuestionId)).ToListAsync();

            if (questions == null)
            {
                return NotFound();
            }

            return Ok(questions);
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestions(int id, string apikey, Questions quests)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            if (id != quests.QuestionId)
            {
                return BadRequest();
            }

            _context.Entry(quests).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transaction.RollbackAsync();
                if (!QuestionsExists(id))
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

        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{apikey}")]
        public async Task<ActionResult<Questions>> PostQuestion(List<Questions> quests, string apikey)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            try
            {
                foreach (Questions ap in quests)
                {

                    if (_context.Questions == null)
                    {
                        return Problem("Entity set 'SurveyDBContext.Question'  is null.");
                    }
                    _context.Questions.Add(ap);

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

        private bool QuestionsExists(int id)
        {
            return (_context.Questions?.Any(e => e.QuestionId == id)).GetValueOrDefault();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using test2.Models;
namespace test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyInfoesController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private IConfiguration _configuration;
        private IDbContextTransaction _transaction;

        public SurveyInfoesController(SurveyDBContext context, IConfiguration iconfig)
        {
            _context = context;
            _configuration = iconfig;
            _transaction = context.Database.BeginTransaction();
        }

        // GET: api/SurveyInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurveyInfo>>> GetSurveyInfo()
        {
          if (_context.SurveyInfo == null)
          {
              return NotFound();
          }

            var survey = _context.SurveyInfo.ToListAsync();
            return await LinkSdid(survey.Result);
        }

        // GET: api/SurveyInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SurveyInfo>>> GetSurveyInfo(int id)
        {
          if (_context.SurveyInfo == null)
          {
              return NotFound();
          }
            var surveyInfo = await _context.SurveyInfo.Where(s => s.Sdid == id).ToListAsync();

            if (surveyInfo == null)
            {
                return NotFound();
            }

            return await LinkSdid(surveyInfo);
        }

        // PUT: api/SurveyInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurveyInfo(int id, string apikey, SurveyInfo surveyInfo)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("AdminKey"))) return Unauthorized("Please supply valid API key");

            surveyInfo.Sdid = id;

            if (id != surveyInfo.Sdid)
            {
                return BadRequest();
            }

            _context.Entry(surveyInfo).State = EntityState.Modified;

            try            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyInfoExists(id))
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

        // POST: api/SurveyInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{apiKey}")]
        public async Task<ActionResult<IEnumerable<SurveyInfo>>> PostSurveyInfo(SurveyInfo surveyInfo, string apikey)
        {
            if (!apikey.Equals(_configuration.GetValue<string>("GeneralSurveyKey"))) return Unauthorized("Please supply valid API key");

            try
            {
                if (_context.SurveyInfo == null)
                {
                    return Problem("Entity set 'SurveyDBContext.SurveyInfo'  is null.");
                }

                _context.SurveyInfo.Add(surveyInfo);
                await _context.SaveChangesAsync();

                await _transaction.CommitAsync();


                // this is very dirty but until I find a better solution this is what I got

                surveyInfo.Gaurdians!.ToList().ForEach(g => { g.Sdid = surveyInfo.Sdid; });
                surveyInfo.AdditionalPeople!.ToList().ForEach(g => g.Sdid = surveyInfo.Sdid);
                surveyInfo.Interviewers!.ToList().ForEach(g => g.Sdid = surveyInfo.Sdid);
                surveyInfo.QAndA!.ToList().ForEach(g => g.Sdid = surveyInfo.Sdid);

                QAndAsController qAndAs = new QAndAsController(_context, _configuration);
                await qAndAs.PostQAndA(surveyInfo.QAndA!, apikey);

                InterviewersController interviewers = new InterviewersController(_context, _configuration);
                await interviewers.PostInterviewers(surveyInfo.Interviewers!, apikey);

                if (surveyInfo.Gaurdians != null)
                {
                    GaurdiansController gaurdians = new GaurdiansController(_context, _configuration);
                    await gaurdians.PostGaurdians(surveyInfo.Gaurdians, apikey);
                }

                if (surveyInfo.AdditionalPeople != null)
                {
                    AdditionalPeoplesController additionalPeople = new AdditionalPeoplesController(_context, _configuration);
                    await additionalPeople.PostAdditionalPeople(surveyInfo.AdditionalPeople, apikey);
                }

                await PutSurveyInfo(surveyInfo.Sdid, apikey,surveyInfo);

                return CreatedAtAction("GetSurveyInfo", new { id = surveyInfo.Sdid }, surveyInfo);
            }

            catch (Exception e)
            {
                await _transaction.RollbackAsync();
                return BadRequest(e);
            }

        }

        private bool SurveyInfoExists(int id)
        {
            return (_context.SurveyInfo?.Any(e => e.Sdid == id)).GetValueOrDefault();
        }

        private async Task<ActionResult<IEnumerable<SurveyInfo>>> LinkSdid(List<SurveyInfo> surveyList)
        {
            surveyList.ForEach(g => g.Gaurdians!.ToList().AddRange(_context.Gaurdians.Where(s => s.Sdid == g.Sdid)));
            surveyList.ForEach(g => g.Interviewers!.ToList().AddRange(_context.Interviewers.Where(s => s.Sdid == g.Sdid)));
            surveyList.ForEach(g => g.AdditionalPeople!.ToList().AddRange(_context.AdditionalPeople.Where(s => s.Sdid == g.Sdid)));
            surveyList.ForEach(g => g.QAndA!.ToList().AddRange(_context.QAndA.Where(s => s.Sdid == g.Sdid)));
            return surveyList;
        }

        private SurveyInfo updateSdid(SurveyInfo survey, int Sdid)
        {
            survey.Gaurdians!.ToList().ForEach(g => { g.Sdid = Sdid; });
            survey.AdditionalPeople!.ToList().ForEach(g => g.Sdid = Sdid);
            survey.Interviewers!.ToList().ForEach(g => g.Sdid = Sdid);
            survey.QAndA!.ToList().ForEach(g => g.Sdid = Sdid);

            return survey;
        }
    }
}

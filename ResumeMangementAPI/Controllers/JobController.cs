using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeManagementAPI.DTO;
using ResumeManagementAPI.DTO.Job;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using ResumeManagementAPI.Repository;

namespace ResumeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepo _jobRepo;
        private readonly ILogger<JobController> _logger;
        private readonly IMapper _mapper;

        public JobController(IJobRepo jobRepo,
            ILogger<JobController> logger, IMapper mapper)
        {
            _jobRepo = jobRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<JobDTO>> GetJobS()
        {
            var jobs = await _jobRepo.GetAll(include: j => j.Include(x => x.Company));
            var result = _mapper.Map<List<JobDTO>>(jobs);
            return result;
        }

        // GET api/<JobController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            var job =  await _jobRepo.GetByIdAsync(x=>x.Id == id,include:q=>q.Include(c=>c.Company));
            var result = _mapper.Map<JobDTO>(job);
            return Ok(result);
        }

        // POST api/<JobController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateJob([FromBody] JobCreateDTO dTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attempt{nameof(CreateJob)}");
                return BadRequest(ModelState);
            }
            var job = _mapper.Map<Job>(dTO);
            await _jobRepo.AddAsync(job);
            return Ok("Successfully Created");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] JobUpdateDto jobDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"invalid Update Attempt in {nameof(UpdateJob)}");
                return BadRequest(ModelState);
            }

            var job = await _jobRepo.GetByIdAsync(x => x.Id == id);
            if (job == null)
            {
                _logger.LogError($"invalid Update Attempt in {nameof(UpdateJob)}");
                return BadRequest("Submitted Data is Invalid");
            }
            _mapper.Map(jobDto, job);
            await _jobRepo.UpdateAsync(job);
            return NoContent();

        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"invalid Delete Attempt in {nameof(DeleteJob)}");
                return BadRequest(ModelState);
            }

            var job = await _jobRepo.GetByIdAsync(x => x.Id == id);
            if (job == null)
            {
                _logger.LogError($"invalid Delete Attempt in {nameof(DeleteJob)}");
                return BadRequest("Submitted Data is Invalid");
            }

            await _jobRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}

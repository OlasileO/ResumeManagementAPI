
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using ResumeManagementAPI.DTO;
using ResumeManagementAPI.DTO.Candidate;
using ResumeManagementAPI.DTO.Job;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using ResumeManagementAPI.Repository;

namespace ResumeManagementAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateRepo _candidateRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CandidateController> _logger;

        public CandidateController(ICandidateRepo candidateRepo,
            IMapper mapper, ILogger<CandidateController> logger)
        {
            _candidateRepo = candidateRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<CandidateDTO>> GetAllCandidate()
        {
            var candidates = await _candidateRepo.GetAll(include: j => j.Include(x => x.Job));
            var result = _mapper.Map<List<CandidateDTO>>(candidates);
            return result;
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            var candidate = await _candidateRepo.GetByIdAsync(x => x.Id == id, include: q =>q.Include(j=> j.Job));
            var result = _mapper.Map<CandidateDTO>(candidate);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attempt{nameof(CreateCandidate)}");
                return BadRequest(ModelState);
            }

            var fileResult = _candidateRepo.SaveFile(createDTO.Resume);

            if (fileResult.Item1 == 0)
            {
                return BadRequest(ModelState);
            }

            var imageName = fileResult.Item2;

            var candidateDTO = _mapper.Map<Candidates>(createDTO);
            candidateDTO.ResumeUrl = imageName;
            await _candidateRepo.AddAsync(candidateDTO);
            return Ok("Successfully Created");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCandidate(int id, [FromForm] CandidateUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"invalid Update Attempt in {nameof(UpdateCandidate)}");
                return BadRequest(ModelState);
            }

            var candidate = await _candidateRepo.GetByIdAsync(x => x.Id == id);
             _candidateRepo.DeleteFile(candidate.ResumeUrl);
            if (candidate == null)
            {
                _logger.LogError($"invalid Update Attempt in {nameof(UpdateCandidate)}");
                return BadRequest("Submitted Data is Invalid");
            }
         

            var fileResult = _candidateRepo.SaveFile(updateDTO.Resume);

            if (fileResult.Item1 == 0)
            {
                return BadRequest(ModelState);
            }

            var imageName = fileResult.Item2;
            _mapper.Map(updateDTO, candidate);
            candidate.ResumeUrl = imageName;

            await _candidateRepo.UpdateAsync(candidate);

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"invalid Delete Attempt in {nameof(DeleteCandidate)}");
                return BadRequest(ModelState);
            }

            var candidate = await _candidateRepo.GetByIdAsync(x => x.Id == id);
             _candidateRepo.DeleteFile(candidate.ResumeUrl);
            if (candidate == null)
            {
                
                _logger.LogError($"invalid Delete Attempt in {nameof(DeleteCandidate)}");
                return BadRequest("Submitted Data is Invalid");
            }

            await _candidateRepo.DeleteAsync(id);
            return NoContent();
        }

    }
}

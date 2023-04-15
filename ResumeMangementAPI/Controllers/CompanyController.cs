using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ResumeManagementAPI.DTO.Company;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using System.Diagnostics.Metrics;

namespace ResumeManagementAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepo _companyRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyRepo companyRepo, IMapper mapper,
            ILogger<CompanyController> logger)
        {
            _companyRepo = companyRepo;
            _mapper = mapper;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<CompanyDto>> GetAllCompany()
        {
            var companies = await _companyRepo.GetAll();
            var result = _mapper.Map<List<CompanyDto>>(companies);
            return result;
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyRepo.GetById(id);
            if (company == null)
            {
                return NotFound();
            }
            var companyDTO = _mapper.Map<CompanyDto>(company);
            return Ok(companyDTO);
        }

        // POST api/<CompanyController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto companyCreate)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attempt{nameof(CreateCompany)}");
                return BadRequest(ModelState);
            }
            var companyDTO = _mapper.Map<Company>(companyCreate);
            await _companyRepo.AddAsync(companyDTO);
            return Ok("Successfully Created");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyUpdateDto companyDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"invalid Update Attempt in {nameof(UpdateCompany)}");
                return BadRequest(ModelState);
            }

            var comapany = await _companyRepo.GetByIdAsync(x => x.Id == id);
            if (comapany == null)
            {
                _logger.LogError($"invalid Update Attempt in {nameof(UpdateCompany)}");
                return BadRequest("Submitted Data is Invalid");
            }
            _mapper.Map(companyDto, comapany);
            await _companyRepo.UpdateAsync(comapany);
            return NoContent();

        }



        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"invalid Delete Attempt in {nameof(DeleteCompany)}");
                return BadRequest(ModelState);
            }

            var comapany = await _companyRepo.GetByIdAsync(x => x.Id == id);
            if (comapany == null)
            {
                _logger.LogError($"invalid Delete Attempt in {nameof(DeleteCompany)}");
                return BadRequest("Submitted Data is Invalid");
            }

            await _companyRepo.DeleteAsync(id);
            return NoContent();

        }

    }
}

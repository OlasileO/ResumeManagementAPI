using AutoMapper;
using ResumeManagementAPI.DTO;
using ResumeManagementAPI.DTO.Candidate;
using ResumeManagementAPI.DTO.Job;
using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Company, CreateCompanyDto>().ReverseMap();
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();

            //Job
            CreateMap<Job, JobDTO>().ReverseMap();
            CreateMap<Job, JobCreateDTO>().ReverseMap();
            CreateMap<Job, JobUpdateDto>().ReverseMap();


            //Candidate
            CreateMap<Candidates, CandidateDTO>().ReverseMap();
            CreateMap<Candidates, CandidateCreateDTO>().ReverseMap();
            CreateMap<Candidates, CandidateUpdateDTO>().ReverseMap();
        }
    }
}

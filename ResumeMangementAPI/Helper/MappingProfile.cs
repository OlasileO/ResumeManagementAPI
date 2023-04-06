using AutoMapper;
using ResumeManagementAPI.DTO;
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
        }
    }
}

using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using ResumeManagementAPI.Models.Data;

namespace ResumeManagementAPI.Repository
{
    public class CompanyRepo:GenericRepo<Company>,ICompanyRepo
    {
        private readonly ResumeContext _context;
        public CompanyRepo(ResumeContext context):base(context)
        {
            _context = context;
        }
    }
}

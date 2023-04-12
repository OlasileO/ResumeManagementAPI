using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using ResumeManagementAPI.Models.Data;

namespace ResumeManagementAPI.Repository
{
    public class JobRepo:GenericRepo<Job>,IJobRepo
    {
        private readonly ResumeContext _context;

        public JobRepo(ResumeContext context):base(context)
        {
            _context = context;
        }
    }
}

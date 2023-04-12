using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.IRepository
{
    public interface ICandidateRepo:IGenericRepo<Candidates>
    {
        Tuple<int, string> SaveFile(IFormFile File);
        bool DeleteFile(string File);
    }
}

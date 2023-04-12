using AutoMapper;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using ResumeManagementAPI.Models.Data;

namespace ResumeManagementAPI.Repository
{
    public class CandidateRepo:GenericRepo<Candidates>,ICandidateRepo
    {
        private readonly ResumeContext _context;
        private readonly IWebHostEnvironment _environment;

        public CandidateRepo(ResumeContext context, 
            IWebHostEnvironment environment) : base(context)
        {
            _context = context;
            _environment = environment;
        }

        public bool DeleteFile(string File)
        {
            try
            {
                var wwwPath = this._environment.WebRootPath;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Document\\", File);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Tuple<int,string> SaveFile(IFormFile File)
        {
            try
            {
                var fiveMegaByte = 5 * 1024 * 1024;
                var wwwPath = this._environment.WebRootPath;
                var path = Path.Combine(Directory.GetCurrentDirectory(),"Document");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var ext = Path.GetExtension(File.FileName);
                var allowedExtension = new string[] { ".pdf", ".docx"};
                if (!allowedExtension.Contains(ext) || File.Length > fiveMegaByte)
                {
                    string msg = string.Format("only {0} extension are allowed", string.Join(",", allowedExtension));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueFileName = Guid.NewGuid().ToString();
                string filename = uniqueFileName + ext;
                var filewithPath = Path.Combine(path, filename);
                var stream = new FileStream(filewithPath, FileMode.Create);
                File.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, filename);
            }
            catch (Exception ex)
            {

                return new Tuple<int, string>(0, "Error has occured");
            }
        }
    }
}

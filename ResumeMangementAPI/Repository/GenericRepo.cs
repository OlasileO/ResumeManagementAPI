using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models.Data;
using System.Linq.Expressions;

namespace ResumeManagementAPI.Repository
{
    public class GenericRepo<T>: IDisposable,IGenericRepo<T> where T : class
    {
        private readonly ResumeContext _context;
       

        public GenericRepo(ResumeContext context)
        {
            _context = context;
            
        }
        private bool disposed = true;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
                   _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void DeleteRange(List<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
        }

        public async Task<T> GetById(int id)
        {
           
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<bool> Exits(int id)
        {
            var entity = await GetById(id);
            return entity != null;
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (orderby != null)
            {
                query = orderby(query);
            }
           return await query.AsNoTracking().ToListAsync();
            
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

       

       
    }
}

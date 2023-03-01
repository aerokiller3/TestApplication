using Microsoft.EntityFrameworkCore;
using TestingApplication.Infrastructure.Base.Interface;
using TestingApplication.Infrastructure.Data;

namespace TestingApplication.Infrastructure.Base.Repository
{
    public class RepositoryService<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly ApplicationDBContext _dbContext = new ApplicationDBContext();

        public RepositoryService()
        {
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> Table => _dbSet.AsNoTracking().AsQueryable();

        public void InsertRange(IEnumerable<T> items)
        {
            _dbSet.AddRange(items);
            _dbContext.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            _dbSet.UpdateRange(items);
            _dbContext.SaveChanges();
        }
        
        public void DeleteAll()
        {
            _dbSet.RemoveRange(_dbSet);
            _dbContext.SaveChanges();
        }
    }
}

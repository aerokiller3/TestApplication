namespace TestingApplication.Infrastructure.Base.Interface
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        void InsertRange(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        void DeleteAll();
    }
}

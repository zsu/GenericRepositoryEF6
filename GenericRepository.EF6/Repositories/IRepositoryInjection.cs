using System.Data.Entity;

namespace GenericRepository
{
    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}
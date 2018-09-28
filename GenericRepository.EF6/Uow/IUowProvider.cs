using System.Data.Entity;

namespace GenericRepository
{
    public interface IUowProvider
    {
        //IUnitOfWork CreateUnitOfWork(DbContext context, bool enableLogging = false);
        IUnitOfWork CreateUnitOfWork<TEntityContext>(bool enableLogging = false) where TEntityContext : DbContext, new();
    }
}

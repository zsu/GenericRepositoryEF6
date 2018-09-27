using System.Data.Entity;

namespace GenericRepository
{
    public interface IUowProvider
    {
        //IUnitOfWork CreateUnitOfWork(DbContext context, bool enableLogging = false);
        IUnitOfWork CreateUnitOfWork(bool enableLogging = false);
    }
}

using System;
using System.Data.Entity;

namespace GenericRepository.Uow
{
    public class UnitOfWork : UnitOfWorkBase<DbContext>, IUnitOfWork
    {
        public UnitOfWork(DbContext context) : base(context)
        { }
    }
    public class UnitOfWork<TDbContext> : UnitOfWorkBase<TDbContext>, IUnitOfWork where TDbContext:DbContext,new()
    {
        public UnitOfWork() : base(new TDbContext())
        { }
    }
}

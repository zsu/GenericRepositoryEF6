using System;
using System.Data.Entity;

namespace GenericRepository
{
    public class UnitOfWork : UnitOfWorkBase<DbContext>, IUnitOfWork
    {
        public UnitOfWork(DbContext context) : base(context)
        { }
    }
}

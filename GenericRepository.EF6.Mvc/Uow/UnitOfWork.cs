using System;
using System.Data.Entity;

namespace GenericRepository.Mvc
{
    public class UnitOfWork : UnitOfWorkBase<DbContext>, IUnitOfWork
    {
        public UnitOfWork(DbContext context) : base(context)
        { }
    }
}

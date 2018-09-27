using System;
using System.Data.Entity;
using GenericRepository.Context;

namespace GenericRepository.Uow
{
    //public class UowProvider : IUowProvider
    //{
    //    public UowProvider()
    //    { }

    //    public IUnitOfWork CreateUnitOfWork(DbContext context,  bool enableLogging = false)
    //    {
    //        var _context = context;
    //        var uow = new UnitOfWork(_context);
    //        return uow;
    //    }
    //}
    public class UowProvider<TDbContext> : IUowProvider where TDbContext:DbContext,new()
    {
        public UowProvider()
        { }

        public IUnitOfWork CreateUnitOfWork(bool enableLogging = false)
        {
            var uow = new UnitOfWork<TDbContext>();
            return uow;
        }
    }
}

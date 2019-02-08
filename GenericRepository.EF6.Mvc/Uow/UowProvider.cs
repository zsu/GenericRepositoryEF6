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
    public class UowProvider:IUowProvider
    {
        public UowProvider()
        { }

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool enableLogging = false) where TEntityContext : DbContext,new()
        {
            var uow = new UnitOfWork(new TEntityContext());
            return uow;
        }
    }
}

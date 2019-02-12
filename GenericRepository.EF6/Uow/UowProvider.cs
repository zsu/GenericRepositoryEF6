using System;
using System.Data.Entity;

namespace GenericRepository
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
        public UowProvider(Action<string> logger)
        {
            _logger = logger;
        }

        private readonly Action<string> _logger;

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) where TEntityContext : DbContext, new()
        {
            var context = new TEntityContext();

            if (!trackChanges)
                context.Configuration.AutoDetectChangesEnabled = trackChanges;
            if (enableLogging && _logger != null)
            {
               context.Database.Log = x => _logger(x);
            }
            var uow = new UnitOfWork(context);
            return uow;
        }
        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool enableLogging = false) where TEntityContext : DbContext,new()
        {
            return CreateUnitOfWork<TEntityContext>(true, enableLogging);
        }
    }
}

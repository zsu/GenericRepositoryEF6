using System;
using System.Data.Entity;
using GenericRepository.Context;
using Microsoft.Extensions.Logging;

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
        public UowProvider(ILogger logger)
        {
            _logger = logger;
        }

        private readonly ILogger _logger;

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) where TEntityContext : DbContext, new()
        {
            var context = new TEntityContext();

            if (!trackChanges)
                context.Configuration.AutoDetectChangesEnabled = trackChanges;
            if (enableLogging)
            {
                if(_logger==null && _logger!=null)
                    context.Database.Log = x => _logger.LogInformation(x);
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

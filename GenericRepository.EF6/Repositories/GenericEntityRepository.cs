using System;
using System.Data.Entity;

namespace GenericRepository
{
    public class GenericEntityRepository<TEntity> : EntityRepositoryBase<DbContext, TEntity> where TEntity : class, new()
    {
		public GenericEntityRepository() : base(null)
		{ }
	}
}
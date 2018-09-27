using GenericRepository.Paging;
using GenericRepository.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericRepository.Repositories
{
    public abstract class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>, IRepository<TEntity> where TContext : DbContext where TEntity : class, new()
	{
        private readonly OrderBy<TEntity> DefaultOrderBy = null;// new OrderBy<TEntity>(qry => qry.OrderBy(e => e.Id));

		protected EntityRepositoryBase(TContext context) : base(context)
		{ }

		public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			var result = QueryDb(null, orderBy, includes);
			return result.ToList();
		}

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			var result = QueryDb(null, orderBy, includes);
			return await result.ToListAsync();
		}

        public virtual void Load(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			if ( orderBy == null ) orderBy = DefaultOrderBy?.Expression;

			var result = QueryDb(null, orderBy, includes).AsNoTracking();
			return result.Skip(startRow).Take(pageLength).ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			if ( orderBy == null ) orderBy = DefaultOrderBy?.Expression;

			var result = QueryDb(null, orderBy, includes).AsNoTracking();
			return await result.Skip(startRow).Take(pageLength).ToListAsync();
		}

		public virtual TEntity Get(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }
            //if(typeof(TEntity).IsSubclassOf(typeof(Entity<>)))
            //    return query.SingleOrDefault(x => x.Id.Equals(id));
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.IsDefined(typeof(KeyAttribute), true));
            if (properties.Count() != 1 || !(properties.First().PropertyType== id.GetType()))
                throw new Exception(string.Format("Invalid key type {0}.", id == null ? null : id.GetType().Name));
            return query.SingleOrDefault(x => (x.GetType().GetProperty(properties.First().Name).GetValue(x, null)).Equals(id));
        }

		public virtual Task<TEntity> GetAsync(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            //if (typeof(TEntity).IsSubclassOf(typeof(Entity<>)))
            //    return query.SingleOrDefaultAsync(x => x.Id.Equals(id));
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.IsDefined(typeof(KeyAttribute), true));
            if (properties.Count() != 1 || !(properties.First().PropertyType == id.GetType()))
                throw new Exception(string.Format("Invalid key type {0}.", id == null ? null : id.GetType().Name));
            return query.SingleOrDefaultAsync(x => (x.GetType().GetProperty(properties.First().Name).GetValue(x, null)).Equals(id));

        }

        public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			var result = QueryDb(filter, orderBy, includes);
			return result.ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			var result = QueryDb(filter, orderBy, includes);
			return await result.ToListAsync();
		}

        public virtual void Load(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			if ( orderBy == null ) orderBy = DefaultOrderBy?.Expression;

			var result = QueryDb(filter, orderBy, includes).AsNoTracking();
			return result.Skip(startRow).Take(pageLength).ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
		{
			if ( orderBy == null ) orderBy = DefaultOrderBy?.Expression;

			var result = QueryDb(filter, orderBy, includes).AsNoTracking();
			return await result.Skip(startRow).Take(pageLength).ToListAsync();
		}

		public virtual void Add(TEntity entity)
		{
			if ( entity == null ) throw new InvalidOperationException("Unable to add a null entity to the repository.");
			Context.Set<TEntity>().Add(entity);
		}

		public virtual TEntity Update(TEntity entity)
		{
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.IsDefined(typeof(KeyAttribute), true));
            if (properties.Count() ==0)
                throw new Exception(string.Format("No Key for entity {0}.",typeof(TEntity).Name));
            List<object> keyValues=new List<object>();
            foreach(var key in properties)
            {
                keyValues.Add(entity.GetType().GetProperty(key.Name).GetValue(entity));
            }
            var existing = Context.Set<TEntity>().Find(keyValues.ToArray());
            if (existing == null) throw new Exception(string.Format("Cannot find entity type {0} with key {1}", typeof(TEntity).Name, string.Join(",", keyValues.ToArray())));
            Context.Entry(existing).CurrentValues.SetValues(entity);
            return existing;
        }

        public virtual void Remove(TEntity entity)
		{
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Set<TEntity>().Remove(entity);
		}

		public virtual void Remove(object id)
		{
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.IsDefined(typeof(KeyAttribute), true));
            if (properties.Count() != 1 || !(properties.First().PropertyType == id.GetType()))
                throw new Exception(string.Format("Invalid key type {0}.", id==null?null:id.GetType().Name));
            var entity = new TEntity();
            entity.GetType().GetProperty(properties.First().Name).SetValue(entity, id);
            this.Remove(entity);
		}

        public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.AnyAsync();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return query.Count();
		}

		public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return query.CountAsync();
		}

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public void SetUnchanged(TEntity entity)
        {
            base.Context.Entry<TEntity>(entity).State = EntityState.Unchanged;
        }
        //Paging

        public DataPage<TEntity> Get(int pageNumber, int pageLength, OrderBy<TEntity> orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
                var startRow = (pageNumber - 1) * pageLength;
                var data = GetPage(startRow, pageLength, includes: includes, orderBy: orderby?.Expression);
                var totalCount = Count();

                return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        public async Task<DataPage<TEntity>> GetAsync(int pageNumber, int pageLength, OrderBy<TEntity> orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
                var startRow = (pageNumber - 1) * pageLength;
                var data = await GetPageAsync(startRow, pageLength, includes: includes, orderBy: orderby?.Expression);
                var totalCount = await CountAsync();

                return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        public DataPage<TEntity> Query(int pageNumber, int pageLength, Filter<TEntity> filter, OrderBy<TEntity> orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
                var startRow = (pageNumber - 1) * pageLength;
                var data = QueryPage(startRow, pageLength, filter.Expression, includes: includes, orderBy: orderby?.Expression);
                var totalCount = Count(filter.Expression);

                return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        public async Task<DataPage<TEntity>> QueryAsync(int pageNumber, int pageLength, Filter<TEntity> filter, OrderBy<TEntity> orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
                var startRow = (pageNumber - 1) * pageLength;
                var data = await QueryPageAsync(startRow, pageLength, filter.Expression, includes: includes, orderBy: orderby?.Expression);
                var totalCount = await CountAsync(filter.Expression);

                return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        private DataPage<TEntity> CreateDataPage(int pageNumber, int pageLength, IEnumerable<TEntity> data, long totalEntityCount)
        {
            var page = new DataPage<TEntity>()
            {
                Data = data,
                TotalEntityCount = totalEntityCount,
                PageLength = pageLength,
                PageNumber = pageNumber
            };

            return page;
        }

    }
}
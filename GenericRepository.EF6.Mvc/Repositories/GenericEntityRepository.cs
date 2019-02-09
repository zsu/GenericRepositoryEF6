using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace GenericRepository.Repositories
{
    public class GenericEntityRepository<TEntity> : EntityRepositoryBase<DbContext, TEntity> where TEntity : class, new()
    {
		public GenericEntityRepository() : base(null)
		{ }
        protected override IEnumerable<PropertyInfo> GetKeyProperties()
        {
            var type = typeof(TEntity);

            var metadataType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true)

            .OfType<MetadataTypeAttribute>().FirstOrDefault();

            var metaData = (metadataType != null)

            ? ModelMetadataProviders.Current.GetMetadataForType(null, metadataType.MetadataClassType)

            : ModelMetadataProviders.Current.GetMetadataForType(null, type);

            var propertMetaData = metaData.Properties

            .Where(e =>

            {

                var attribute = metaData.ModelType.GetProperty(e.PropertyName).GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault() as KeyAttribute;
                return attribute != null;

            }).Select(x => typeof(TEntity).GetProperty(x.PropertyName));
            if (propertMetaData.Count() > 0)
                return propertMetaData;
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.IsDefined(typeof(KeyAttribute), true));
            return properties;
        }
    }
}
using Example.Entities;
using System.Data.Entity;
using SQLite.CodeFirst;

namespace Example
{
    public class AppContext : DbContext
    {
        public AppContext() : base("DefaultConnection")
        { }

        public DbSet<Department> Buildings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<AppContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
            base.OnModelCreating(modelBuilder);
        }
    }
}

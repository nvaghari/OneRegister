using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Entities.EDuit;

namespace OneRegister.Data.Context
{
    public class EDuitContext : DbContext
    {
        public EDuitContext(DbContextOptions<EDuitContext> options) : base(options)
        {
        }

        #region Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EDuitSchool>().HasNoKey();
        }
        #endregion

        public DbSet<EDuitSchool> EDuitSchools { get; set; }
    }
}

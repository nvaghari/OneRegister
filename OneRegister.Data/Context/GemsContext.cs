using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Entities.Gems;

namespace OneRegister.Data.Context
{
    public class GemsContext : DbContext
    {
        public GemsContext(DbContextOptions<GemsContext> options) : base(options)
        {
        }

        public DbSet<CL_Bank> Banks { get; set; }

        public DbSet<CL_Country> Countries { get; set; }

        public DbSet<CL_CountryState> CountryStates { get; set; }

        public DbSet<CL_IdentityType> IdentityTypes { get; set; }

        public DbSet<CL_Industry> Industries { get; set; }

        public DbSet<CL_Occupation> Occupations { get; set; }

        public DbSet<CL_RemitPurpose> RemitPurposes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CL_Country>().HasNoKey();
            modelBuilder.Entity<CL_CountryState>().HasNoKey();
            modelBuilder.Entity<CL_Industry>().HasNoKey();
            modelBuilder.Entity<CL_Occupation>().HasNoKey();
            modelBuilder.Entity<CL_RemitPurpose>().HasNoKey();
            modelBuilder.Entity<CL_Bank>().HasNoKey();
            modelBuilder.Entity<CL_IdentityType>().HasNoKey();
        }
    }
}

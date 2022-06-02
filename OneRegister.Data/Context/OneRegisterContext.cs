using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.AgroRegistration;
using OneRegister.Data.Entities.Application;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Data.Entities.Notification;
using OneRegister.Data.Entities.StudentRegistration;
using OneRegister.Data.Identication;
using OneRegister.Data.SuperEntities;
using System;

namespace OneRegister.Data.Context
{
    public class OneRegisterContext : IdentityDbContext<OUser, ORole, Guid>
    {
        //This Line would be commented if you want to Migrate 
        public OneRegisterContext(DbContextOptions<OneRegisterContext> options) : base(options)
        {
        }


        //This Line would be commented if you want to use Data layer in production
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=OneRegisterV2;Integrated Security=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>()
                        .HasDiscriminator<OrganizationType>("Type")
                        .HasValue<Organization>(OrganizationType.Root)
                        .HasValue<School>(OrganizationType.School)
                        .HasValue<AgroOrganization>(OrganizationType.Agropreneur)
                        .HasValue<Merchant>(OrganizationType.Merchant);
            modelBuilder.Entity<Site>()
                        .HasDiscriminator<SiteType>("Type")
                        .HasValue<Site>(SiteType.Root)
                        .HasValue<ClassRoom>(SiteType.Class)
                        .HasValue<HomeRoom>(SiteType.HomeRoom)
                        .HasValue<MerchantOutlet>(SiteType.Outlet);
            modelBuilder.Entity<Member>()
                        .HasDiscriminator<MemberType>("Type")
                        .HasValue<Member>(MemberType.Root)
                        .HasValue<Student>(MemberType.Student)
                        .HasValue<Agropreneur>(MemberType.Agropreneur)
                        .HasValue<MerchantOwner>(MemberType.MerchantOwner);

            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Permissions)
                .UsingEntity<RolePermission>();

            //rename Identity tables
            modelBuilder.Entity<ORole>(b => b.ToTable("Roles", schema: SchemaNames.Security));
            modelBuilder.Entity<OUser>(b => b.ToTable("Users", schema: SchemaNames.Security));
            modelBuilder.Entity<IdentityUserRole<Guid>>(b => b.ToTable("UserRoles", schema: SchemaNames.Security).HasKey(r => new { r.UserId, r.RoleId }));
            modelBuilder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("UserClaims", schema: SchemaNames.Security));
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("RoleClaims", schema: SchemaNames.Security));
            modelBuilder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("UserLogins", schema: SchemaNames.Security));
            modelBuilder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("UserTokens", schema: SchemaNames.Security));
            
            modelBuilder.Entity<OUser>().Property(b => b.PasswordHash).HasMaxLength(256);
            modelBuilder.Entity<OUser>().Property(b => b.SecurityStamp).HasMaxLength(256);
            modelBuilder.Entity<OUser>().Property(b => b.ConcurrencyStamp).HasMaxLength(256);
            modelBuilder.Entity<OUser>().Property(b => b.PhoneNumber).HasMaxLength(32);
        }

        #region DbSets

        public DbSet<MemberAddress> MemberAddresses { get; set; }
        public DbSet<MemberFile> MemberFiles { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<OrganizationFile> OrganizationFiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Site> Sites { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<HomeRoom> HomeRooms { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<AgroOrganization> AgroOrganizations { get; set; }
        public DbSet<Agropreneur> Agropreneurs { get; set; }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<MerchantInfo> MerchantInfos { get; set; }
        public DbSet<MerchantCommission> MerchantCommissions { get; set; }
        public DbSet<MerchantOutlet> MerchantOutlets { get; set; }
        public DbSet<MerchantOwner> MerchantOwners { get; set; }

        public DbSet<NotificationJob> NotificationJobs { get; set; }
        public DbSet<NotificationTask> NotificationTasks { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<CodeList> CodeLists { get; set;}

        public DbSet<InquiryTask> InquiryTasks { get; set; }


        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion DbSets
    }
}
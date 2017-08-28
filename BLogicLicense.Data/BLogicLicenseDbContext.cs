using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using BLogicLicense.Model.Models;

namespace BLogicLicense.Data
{
    public class BLogicLicenseDbContext : IdentityDbContext<AppUser>
    {
        public BLogicLicenseDbContext() : base("BLogicLicenseConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Page> Pages { set; get; }
     
        public DbSet<SystemConfig> SystemConfigs { set; get; }

        public DbSet<Error> Errors { set; get; }
    
        public DbSet<Function> Functions { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<IdentityUserRole> UserRoles { set; get; }

        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        public DbSet<ProductKey> ProductKeys { set; get; }
        public DbSet<Software> Softwares { set; get; }
        public DbSet<Store> Stores { set; get; }
        public DbSet<Transaction> Transactions { set; get; }
        public DbSet<UnRegisterKey> UnRegisterKeys { set; get; }
        public static BLogicLicenseDbContext Create()
        {
            return new BLogicLicenseDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AppRoles");
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("AppUserLogins");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("AppUserClaims");
        }
    }
}
using URFX.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.DataEntity
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }               
        public ICollection<UserRole> UserRoles { get; set; }
        public Client Client { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
    }
    public class UserRole : IdentityUserRole
    {
    }
    public class UserClaim : IdentityUserClaim
    {
    }
    public class UserLogin : IdentityUserLogin
    {
    }
    public class URFXDbContext : IdentityDbContext<ApplicationUser>
    {
        public URFXDbContext()
            : base("UrfxConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ServiceProvider> ServiceProvider { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }      

        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<UserPlan> UserPlans { get; set; }

        public virtual DbSet<ServiceProviderServiceMapping> ServiceProviderServiceMapping { get; set; }

        public virtual DbSet<Country> Country { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);

            //Defining the keys and relations
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany<UserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUser>().HasOptional(x=>x.Client).WithRequired(x=>x.User);
            modelBuilder.Entity<UserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
            
        }
        public static URFXDbContext Create()
        {
            return new URFXDbContext();
        }
    }
}

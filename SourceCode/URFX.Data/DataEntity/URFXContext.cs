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
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Net.Mail;
using System.Net;
using URFX.Data.DataEntity.DomainModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using URFX.Data.Enums;

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

        public string Role { get; set; }
        public string DeviceType { get; set; }
        
        public string DeviceToken { get; set; }

        public RegistrationType RegistrationType { get; set; }

        public string FacebookId { get; set; }

        public string GoogleId { get; set; }

        public string TwitterId { get; set; }

        public bool IsRegister { get; set; }

        public bool IsLogin { get; set; }
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

        public virtual DbSet<City> City { get; set; }   
        
        public virtual DbSet<District> District { get; set; }

        public virtual DbSet<UserLocation> UserLocation { get; set; }

        public virtual DbSet<TransactionHistory> TransactionHistory { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<ServicePoviderEmployeeMapping> ServicePoviderEmployeeMapping { get; set; }

       // public virtual DbSet<ClientLocation> ClientLocation { get; set; }
        public virtual DbSet<Complaint> Complaint { get; set; }
        public virtual DbSet<ComplaintImageMapping> ComplaintImageMapping { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<PlanPayment> PlanPayment { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<JobServiceMapping> JobServiceMapping { get; set; }
        public virtual DbSet<JobServicePicturesMapping> JobServicePicturesMapping { get; set; }

        public virtual DbSet<JobPayment> JobPayment { get; set; }
        public virtual DbSet<JobRequest> JobRequest { get; set; }

        public virtual DbSet<EmployeeServiceMapping> EmployeeServiceMapping { get; set; }
        public virtual DbSet<EmployeeSchedule> EmployeeSchedule { get; set; }

        public virtual DbSet<ClientRating> ClientRating { get; set; }
        public virtual DbSet<CarType> CarType { get; set; }
        public virtual DbSet<CarEmployeeMapping> CarEmployeeMapping { get; set; }
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
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(38, 18));

        }
        public static URFXDbContext Create()
        {
            return new URFXDbContext();
        }



        public virtual List<CategoryWithPriceModel> CategoryWithPriceModel() 
        {
            List<CategoryWithPriceModel> result = new List<CategoryWithPriceModel>();
               
                using (URFXDbContext db = new URFXDbContext()) 
                {
                    var command = db.Database.Connection.CreateCommand();
                    command.CommandText = "sp_GetCategoriesWithPrice";
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000000000;
                    try
                    {
                        command.Connection.Open();
                        var reader = command.ExecuteReader();
                        ObjectResult<CategoryWithPriceModel> sectionDuration = ((IObjectContextAdapter)db)
                       .ObjectContext
                       .Translate<CategoryWithPriceModel>(reader);
                        result = (from a in sectionDuration select a).ToList();
                     }
                    finally
                    {
                      command.Connection.Close();
                    }
                }



                return result;
        }

      


    }
}

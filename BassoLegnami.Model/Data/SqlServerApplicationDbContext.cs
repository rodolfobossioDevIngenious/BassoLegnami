using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using In.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Threading;
using BassoLegnami.Model.Models.Support;
using BassoLegnami.Model.Models.Users;
using Microsoft.AspNetCore.Http;
using BassoLegnami.Model.Models.Log;

namespace BassoLegnami.Model.Data
{
    public class SqlServerApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private Guid _user;

        public SqlServerApplicationDbContext(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Setting>().HasAlternateKey(r => r.Key);

            builder.Entity<Error>().HasOne(r => r.Log).WithMany(r => r.Errors).HasForeignKey(r => r.LogID).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<File>().HasOne(r => r.FileFolder).WithMany(r => r.Files).HasForeignKey(r => r.FileFolderID).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ExternalSystemValue>().HasOne(r => r.ExternalSystem).WithMany(r => r.ExternalSystemValues).HasForeignKey(r => r.ExternalSystemID).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public Func<DateTime> TimestampProvider { get; set; } = () => DateTime.Now;

        public Guid User
        {
            get
            {
                if (_user == default)
                {
                    string username = _httpContext.HttpContext?.User.Identity.Name ?? ApplicationUser.USER_SYSTEM;
                    string userId = Users.FirstOrDefault(r => r.UserName == username)?.Id;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return _user;
                    }

                    _user = new Guid(userId);
                }
                return _user;
            }
        }

        public void SetUser(Guid user)
        {
            _user = user;
        }

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            TrackChanges();
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private void TrackChanges()
        {
            Guid user = User;
            DateTime timestamp = TimestampProvider();
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is Auditable)
                {
                    Auditable auditable = entry.Entity as Auditable;
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedBy = user.ToString();
                        auditable.CreatedOn = timestamp;
                        auditable.UpdatedBy = user.ToString();
                        auditable.UpdatedOn = timestamp;
                    }
                    else
                    {
                        auditable.CreatedBy = entry.GetDatabaseValues().GetValue<string>(nameof(auditable.CreatedBy));
                        auditable.CreatedOn = entry.GetDatabaseValues().GetValue<DateTime?>(nameof(auditable.CreatedOn));
                        auditable.UpdatedBy = user.ToString();
                        auditable.UpdatedOn = timestamp;
                    }
                }
            }
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Error> Errors { get; set; }

        public DbSet<In.Core.Models.Authorization.RecordFilterRuleType> RecordFilterRuleTypes { get; set; }
        public DbSet<In.Core.Models.Authorization.RecordFilterRule> RecordFilterRules { get; set; }
        public DbSet<In.Core.Models.Authorization.RecordFilterRuleValue> RecordFilterRuleValues { get; set; }
        public DbSet<In.Core.Models.Authorization.Authorization> Authorizations { get; set; }

        public DbSet<FileFolder> FileFolders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Models.EmailMessage> EmailMessages { get; set; }
        public DbSet<ExternalSystem> ExternalSystems { get; set; }
        public DbSet<ExternalSystemValue> ExternalSystemValues { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Festivity> Festivities { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<Models.GeographicSupport.City> Cities { get; set; }
        public DbSet<Models.GeographicSupport.Province> Provinces { get; set; }
        public DbSet<Models.GeographicSupport.Region> Regions { get; set; }
        public DbSet<Models.GeographicSupport.RegionalZone> RegionalZones { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public DbSet<Clienti> Clienti { get; set; }
        public DbSet<AgentiGiacenze> AgentiGiacenze { get; set; }
        public DbSet<Tabelle> Tabelle { get; set; }
    }
}

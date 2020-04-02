using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using si2.dal.Entities;
using si2.dal.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace si2.dal.Context
{

    public class Si2DbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }

        public DbSet<Dataflow> Dataflows { get; set; }

		public DbSet<Simplecohort> Simplecohorts { get; set; }

		public DbSet<Vehicle> Vehicles { get; set; }

        public Si2DbContext(DbContextOptions<Si2DbContext> options) : base(options)
        {
            _httpContextAccessor = this.GetService<IHttpContextAccessor>();

			AuditManager.DefaultConfiguration.Exclude(x => true); // Exclude ALL
			AuditManager.DefaultConfiguration.Include<IAuditable>();
			AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
				(context as Si2DbContext).AuditEntries.AddRange(audit.Entries);
			
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            // seed the database with dummy data
        }

		public override int SaveChanges()
		{
			var audit = new Audit() { CreatedBy = _httpContextAccessor.HttpContext.User.Identity.Name };
			audit.PreSaveChanges(this);
			var rowAffecteds = base.SaveChanges();
			audit.PostSaveChanges();

			if (audit.Configuration.AutoSavePreAction != null)
			{
				audit.Configuration.AutoSavePreAction(this, audit);
				base.SaveChanges();
			}

			return rowAffecteds;
		}


		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			var audit = new Audit() { CreatedBy = _httpContextAccessor.HttpContext.User.Identity.Name };
			audit.PreSaveChanges(this);
			var rowAffecteds = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			audit.PostSaveChanges();

			if (audit.Configuration.AutoSavePreAction != null)
			{
				audit.Configuration.AutoSavePreAction(this, audit);
				await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}

			return rowAffecteds;
		}
	}
}
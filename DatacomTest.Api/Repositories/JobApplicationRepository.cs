using System.Threading.Tasks;

using DatacomTest.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace DatacomTest.Api.Repositories {
	public class JobApplicationRepository : IJobApplicationRepository {
		private readonly DbContextOptions<JobApplicationDbContext> _options;


		public JobApplicationRepository(DbContextOptions<JobApplicationDbContext> options) {
			_options = options;
		}


		public async Task<IEnumerable<JobApplication>> GetAllAsync() {
			using var context = new JobApplicationDbContext(_options);

			return await context.JobApplications.ToListAsync();
		}

		public async Task<JobApplication?> GetByIdAsync(int id) {
			using var context = new JobApplicationDbContext(_options);

			return await context.JobApplications.FindAsync(id);
		}

		public async Task<JobApplication> CreateAsync(JobApplication application) {
			using var context = new JobApplicationDbContext(_options);

			context.JobApplications.Add(application);
			await context.SaveChangesAsync();

			return application;
		}

		public async Task<JobApplication> UpdateAsync(JobApplication application) {
			using var context = new JobApplicationDbContext(_options);

			context.JobApplications.Update(application);
			await context.SaveChangesAsync();

			return application;
		}
	}

	public class JobApplicationDbContext : DbContext {
		public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : base(options) { }


		public DbSet<JobApplication> JobApplications { get; set; } = null!;


		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<JobApplication>()
				.Property(e => e.CompanyName)
					.HasMaxLength(200);

			modelBuilder.Entity<JobApplication>()
				.Property(e => e.Position)
					.HasMaxLength(200);
		}
	}
}

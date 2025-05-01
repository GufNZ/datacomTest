using Microsoft.EntityFrameworkCore;

using DatacomTest.Api.Models;

namespace DatacomTest.Api.Repositories;

public class JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : DbContext(options) {
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

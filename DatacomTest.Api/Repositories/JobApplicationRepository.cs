using Microsoft.EntityFrameworkCore;

using DatacomTest.Api.Models;

namespace DatacomTest.Api.Repositories;

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

	public async Task DeleteAsync(int id) {
		using var context = new JobApplicationDbContext(_options);

		var application = await context.JobApplications.FindAsync(id);
		if (application != null) {
			context.JobApplications.Remove(application);
			await context.SaveChangesAsync();
		}
	}
}

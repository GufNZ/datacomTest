using Microsoft.EntityFrameworkCore;

using DatacomTest.Api.Models;

namespace DatacomTest.Api.Repositories;

public class JobApplicationRepository : IJobApplicationRepository {
	private readonly DbContextOptions<JobApplicationDbContext> _options;


	public JobApplicationRepository(DbContextOptions<JobApplicationDbContext> options) {
		_options = options;
	}


	public async Task<List<JobApplication>> GetAllAsync(
		int page = 1,
		int limit = 10,
		ApplicationStatus? status = null,
		string? company = null,
		string? position = null
	) {
		await using var context = new JobApplicationDbContext(_options);

		var query = context.JobApplications.AsQueryable();

		if (status.HasValue) {
			query = query.Where(a => a.Status == status.Value);
		}

		if (!string.IsNullOrWhiteSpace(company)) {
			query = query.Where(a => a.CompanyName.ToUpper().Contains(company.ToUpper()));
		}

		if (!string.IsNullOrWhiteSpace(position)) {
			query = query.Where(a => a.Position.ToUpper().Contains(position.ToUpper()));
		}

		return await query
			.Skip((page - 1) * limit)
			.Take(limit)
			.ToListAsync();
	}

	public async Task<JobApplication?> GetByIdAsync(int id) {
		await using var context = new JobApplicationDbContext(_options);

		return await context.JobApplications.FindAsync(id);
	}

	public async Task<JobApplication> AddAsync(JobApplication application) {
		await using var context = new JobApplicationDbContext(_options);

		await context.JobApplications.AddAsync(application);
		await context.SaveChangesAsync();

		return application;
	}

	public async Task<JobApplication> UpdateAsync(JobApplication application) {
		await using var context = new JobApplicationDbContext(_options);

		context.JobApplications.Update(application);
		await context.SaveChangesAsync();

		return application;
	}

	public async Task<int> CountAsync() {
		await using var context = new JobApplicationDbContext(_options);

		return await context.JobApplications.CountAsync();
	}

	public async Task DeleteAsync(int id) {
		await using var context = new JobApplicationDbContext(_options);

		var application = await context.JobApplications.FindAsync(id);
		if (application != null) {
			context.JobApplications.Remove(application);
			await context.SaveChangesAsync();
		}
	}
}

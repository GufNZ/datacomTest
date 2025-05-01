using System.Diagnostics;

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
		string? position = null,
		SortKey sortBy = SortKey.DateApplied,
		SortDirection sortDirection = SortDirection.Desc
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

		var ordered = sortBy switch {
			SortKey.CompanyName => sortDirection == SortDirection.Asc
				? query.OrderBy(a => a.CompanyName)
				: query.OrderByDescending(a => a.CompanyName),
			SortKey.Position => sortDirection == SortDirection.Asc
				? query.OrderBy(a => a.Position)
				: query.OrderByDescending(a => a.Position),
			SortKey.DateApplied => sortDirection == SortDirection.Asc
				? query.OrderBy(a => a.DateApplied)
				: query.OrderByDescending(a => a.DateApplied),
			SortKey.Status => sortDirection == SortDirection.Asc
				? query.OrderBy(a => a.Status)
				: query.OrderByDescending(a => a.Status),
			_ => throw new UnreachableException("Can't get here!")
		};

		var withIdSort = sortDirection == SortDirection.Asc
			? ordered.ThenBy(application => application.Id)
			: ordered.ThenByDescending(application => application.Id);

		return await withIdSort
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

	public async Task DeleteAsync(int id) {
		await using var context = new JobApplicationDbContext(_options);

		var application = await context.JobApplications.FindAsync(id);
		if (application != null) {
			context.JobApplications.Remove(application);
			await context.SaveChangesAsync();
		}
	}
}

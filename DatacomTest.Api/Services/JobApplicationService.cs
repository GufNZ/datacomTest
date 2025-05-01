using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

namespace DatacomTest.Api.Services;

public class JobApplicationService : IJobApplicationService {
	private readonly IJobApplicationRepository _repository;


	public JobApplicationService(IJobApplicationRepository repository) {
		_repository = repository;
	}


	public async Task<PaginatedResponse<JobApplication>> GetApplicationsAsync(
		int page = 1,
		int limit = 10,
		ApplicationStatus? status = null,
		string? company = null,
		string? position = null,
		SortKey sortBy = SortKey.DateApplied,
		SortDirection sortDirection = SortDirection.Asc
	) {
		var applications = await _repository.GetAllAsync(page, limit, status, company, position, sortBy, sortDirection);
		var totalPages = (int)Math.Ceiling(applications.Count / (double)limit);

		return new PaginatedResponse<JobApplication> {
			Data = applications,
			Total = applications.Count,
			Page = page,
			Pages = totalPages,
			Limit = limit
		};
	}

	public async Task<JobApplication?> GetApplicationAsync(int id) {
		return await _repository.GetByIdAsync(id);
	}

	public async Task<JobApplication> CreateApplicationAsync(JobApplication application) {
		return await _repository.AddAsync(application);
	}

	public async Task<JobApplication> UpdateApplicationAsync(int id, JobApplication application) {
		var existingApplication = await _repository.GetByIdAsync(id);
		if (existingApplication == null) {
			throw new KeyNotFoundException("Application not found!");
		}


		application.Id = id;
		return await _repository.UpdateAsync(application);
	}

	public async Task DeleteApplicationAsync(int id) {
		var application = await _repository.GetByIdAsync(id);
		if (application == null) {
			throw new KeyNotFoundException("Application not found!");
		}


		await _repository.DeleteAsync(id);
	}
}

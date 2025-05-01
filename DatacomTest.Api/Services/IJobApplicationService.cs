using DatacomTest.Api.Models;

namespace DatacomTest.Api.Services;

public interface IJobApplicationService {
	Task<PaginatedResponse<JobApplication>> GetApplicationsAsync(
		int page = 1,
		int limit = 10,
		ApplicationStatus? status = null,
		string? company = null,
		string? position = null
	);

	Task<JobApplication?> GetApplicationAsync(int id);

	Task<JobApplication> CreateApplicationAsync(JobApplication application);

	Task<JobApplication> UpdateApplicationAsync(int id, JobApplication application);

	Task DeleteApplicationAsync(int id);
}

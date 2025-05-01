using DatacomTest.Api.Models;

namespace DatacomTest.Api.Repositories;

public interface IJobApplicationRepository {
	Task<List<JobApplication>> GetAllAsync(
		int page = 1,
		int limit = 10,
		ApplicationStatus? status = null,
		string? company = null,
		string? position = null
	);
	Task<int> CountAsync();
	Task<JobApplication?> GetByIdAsync(int id);
	Task<JobApplication> AddAsync(JobApplication application);
	Task<JobApplication> UpdateAsync(JobApplication application);
	Task DeleteAsync(int id);
}

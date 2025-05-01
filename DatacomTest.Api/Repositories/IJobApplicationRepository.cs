using System.Threading.Tasks;

using DatacomTest.Api.Models;

namespace DatacomTest.Api.Repositories {
	public interface IJobApplicationRepository {
		Task<IEnumerable<JobApplication>> GetAllAsync();
		Task<JobApplication?> GetByIdAsync(int id);
		Task<JobApplication> CreateAsync(JobApplication application);
		Task<JobApplication> UpdateAsync(JobApplication application);
	}
}

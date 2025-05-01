using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

namespace DatacomTest.Api.Services;

public class DataSeedService : IDataSeedService {
	private readonly IJobApplicationRepository _repository;


	public DataSeedService(IJobApplicationRepository repository) {
		_repository = repository;
	}


	public async Task SeedDataAsync(int count = 20) {
		// Check if there are any existing applications:
		var existingApps = await _repository.GetAllAsync();
		if (existingApps.Any()) {
			return;		// Don't seed if there's already data.
		}


		// List of companies and positions for test data:
		var companies = new[] {
			"Google",
			"Microsoft",
			"Amazon",
			"Facebook",
			"Apple",
			"Netflix",
			"Tesla",
			"Uber",
			"Airbnb",
			"Spotify"
		};

		var positions = new[] {
			"Software Engineer",
			"Product Manager",
			"Data Scientist",
			"DevOps Engineer",
			"UI/UX Designer"
		};

		// Create test applications:
		for (int i = 0; i < count; i++) {
			var app = new JobApplication {
				CompanyName = companies[i % companies.Length],
				Position = positions[i % positions.Length],
				Status = (ApplicationStatus)(i % 4),		// Cycle through statuses.
				DateApplied = DateOnly.FromDateTime(DateTime.Now.AddDays(-i))
			};
			await _repository.AddAsync(app);
		}
	}
}

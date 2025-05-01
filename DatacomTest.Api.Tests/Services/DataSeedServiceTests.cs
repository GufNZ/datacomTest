using Moq;

using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;
using DatacomTest.Api.Services;

namespace DatacomTest.Api.Tests.Services;

public class DataSeedServiceTests {
	private readonly Mock<IJobApplicationRepository> _mockRepository;
	private readonly IDataSeedService _service;


	public DataSeedServiceTests() {
		_mockRepository = new Mock<IJobApplicationRepository>();
		_service = new DataSeedService(_mockRepository.Object);
	}


	[Fact]
	public async Task SeedDataAsync_DoesNotSeed_WhenDataExists() {
		// Arrange:
		var existingApps = new List<JobApplication> {
			new JobApplication {
				Id = 1,
				CompanyName = "Test Company",
				Position = "Test Position",
				Status = ApplicationStatus.Applied,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			}
		};

		_mockRepository
			.Setup(repo => repo.GetAllAsync(
				/* page */ 1,
				/* limit */ 10,
				/* status */ null,
				/* company */ null,
				/* position */ null,
				/* sortBy */ SortKey.DateApplied,
				/* sortDirection */ SortDirection.Desc
			))
			.ReturnsAsync(existingApps);

		// Act:
		await _service.SeedDataAsync();

		// Assert:
		_mockRepository.Verify(repo => repo.AddAsync(It.IsAny<JobApplication>()), Times.Never);
	}

	[Fact]
	public async Task SeedDataAsync_SeedsWith20Applications_WhenNoDataExists() {
		// Arrange:
		var emptyList = new List<JobApplication>();

		_mockRepository
			.Setup(repo => repo.GetAllAsync(
				/* page */ 1,
				/* limit */ 10,
				/* status */ null,
				/* company */ null,
				/* position */ null,
				/* sortBy */ SortKey.DateApplied,
				/* sortDirection */ SortDirection.Desc
			))
			.ReturnsAsync(emptyList);

		// Act:
		await _service.SeedDataAsync();

		// Assert:
		_mockRepository.Verify(repo => repo.AddAsync(It.IsAny<JobApplication>()), Times.Exactly(20));
	}
}

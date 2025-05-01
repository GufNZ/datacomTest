using Moq;

using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;
using DatacomTest.Api.Services;

namespace DatacomTest.Api.Tests.Services;

public class JobApplicationServiceTests {
	private readonly Mock<IJobApplicationRepository> _mockRepository;
	private readonly JobApplicationService _service;


	public JobApplicationServiceTests() {
		_mockRepository = new Mock<IJobApplicationRepository>();
		_service = new JobApplicationService(_mockRepository.Object);
	}


	[Fact]
	public async Task GetApplicationsAsync_ReturnsPaginatedResponse() {
		// Arrange:
		var applications = new List<JobApplication> {
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
			.ReturnsAsync(applications);

		// Act:
		var result = await _service.GetApplicationsAsync();

		// Assert:
		Assert.NotNull(result);
		Assert.Single(result.Data);
		Assert.Equal(applications.Count, result.Total);
		Assert.Equal(1, result.Pages);
		Assert.Equal(1, result.Page);
		Assert.Equal(10, result.Limit);
	}

	[Fact]
	public async Task GetApplicationAsync_ReturnsApplication_WhenFound() {
		// Arrange:
		var application = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockRepository
			.Setup(repo => repo.GetByIdAsync(1))
			.ReturnsAsync(application);

		// Act:
		var result = await _service.GetApplicationAsync(1);

		// Assert:
		Assert.NotNull(result);
		Assert.Equal(1, result.Id);
	}

	[Fact]
	public async Task GetApplicationAsync_ReturnsNull_WhenNotFound() {
		// Arrange:
		_mockRepository
			.Setup(repo => repo.GetByIdAsync(1))
			.ReturnsAsync((JobApplication)null!);

		// Act:
		var result = await _service.GetApplicationAsync(1);

		// Assert:
		Assert.Null(result);
	}

	[Fact]
	public async Task CreateApplicationAsync_CreatesNewApplication() {
		// Arrange:
		var application = new JobApplication {
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		var expectedId = 1;
		application.Id = expectedId;

		_mockRepository
			.Setup(repo => repo.AddAsync(It.IsAny<JobApplication>()))
			.ReturnsAsync(application);

		// Act:
		var result = await _service.CreateApplicationAsync(application);

		// Assert:
		_mockRepository.Verify(repo => repo.AddAsync(It.IsAny<JobApplication>()), Times.Once);
		Assert.NotNull(result);
		Assert.Equal(expectedId, result.Id);
	}

	[Fact]
	public async Task UpdateApplicationAsync_UpdatesExistingApplication() {
		// Arrange:
		var application = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockRepository
			.Setup(repo => repo.GetByIdAsync(1))
			.ReturnsAsync(application);

		_mockRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<JobApplication>()))
			.ReturnsAsync(application);

		// Act:
		var result = await _service.UpdateApplicationAsync(1, application);

		// Assert:
		_mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<JobApplication>()), Times.Once);
		Assert.NotNull(result);
		Assert.Equal(1, result.Id);
	}

	[Fact]
	public async Task DeleteApplicationAsync_DeletesExistingApplication() {
		// Arrange:
		var application = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockRepository
			.Setup(repo => repo.GetByIdAsync(1))
			.ReturnsAsync(application);

		// Act:
		await _service.DeleteApplicationAsync(1);

		// Assert:
		_mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
	}

	[Fact]
	public async Task DeleteApplicationAsync_ThrowsKeyNotFoundException_WhenNotFound() {
		// Arrange:
		_mockRepository
			.Setup(repo => repo.GetByIdAsync(1))
			.ReturnsAsync((JobApplication?)null);

		// Act & Assert:
		await Assert.ThrowsAsync<KeyNotFoundException>(
			async () => await _service.DeleteApplicationAsync(1)
		);
	}
}

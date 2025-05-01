using Microsoft.EntityFrameworkCore;

using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

namespace DatacomTest.Api.Tests.Repositories;

[Collection("Database collection")]
public class JobApplicationRepositoryTests : IClassFixture<JobApplicationRepositoryTestDatabaseFixture> {
	private readonly DbContextOptions<JobApplicationDbContext> _options;


	public JobApplicationRepositoryTests(JobApplicationRepositoryTestDatabaseFixture fixture) {
		_options = fixture.Options;
	}


	[Fact]
	public async Task GetAllAsync_ReturnsAllApplications() {
		// Arrange:
		await using var context = new JobApplicationDbContext(_options);
		var repository = new JobApplicationRepository(_options);

		// Clean up any existing data:
		context.JobApplications.RemoveRange(context.JobApplications);
		await context.SaveChangesAsync();

		var testApplication = new JobApplication {
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		await repository.AddAsync(testApplication);

		// Act:
		var result = await repository.GetAllAsync();

		// Assert:
		Assert.Single(result);
		var application = result.First();
		Assert.Equal(testApplication.CompanyName, application.CompanyName);
		Assert.Equal(testApplication.Position, application.Position);
		Assert.Equal(testApplication.Status, application.Status);
		Assert.Equal(testApplication.DateApplied, application.DateApplied);
	}

	[Fact]
	public async Task GetAllAsync_ReturnsFilteredApplications() {
		// Arrange:
		await using var context = new JobApplicationDbContext(_options);
		var repository = new JobApplicationRepository(_options);

		// Clean up any existing data:
		context.JobApplications.RemoveRange(context.JobApplications);
		await context.SaveChangesAsync();

		// Add test applications:
		var testApplications = new List<JobApplication> {
			new JobApplication {
				CompanyName = "Test Company 1",
				Position = "Test Position 1",
				Status = ApplicationStatus.Applied,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			},
			new JobApplication {
				CompanyName = "Test Company 2",
				Position = "Test Position 2",
				Status = ApplicationStatus.Interview,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			}
		};

		foreach (var app in testApplications) {
			await repository.AddAsync(app);
		}

		// Act:
		var result = await repository.GetAllAsync(1, 10, ApplicationStatus.Applied);

		// Assert:
		Assert.Single(result);
		var application = result[0];
		Assert.Equal(testApplications[0].CompanyName, application.CompanyName);
		Assert.Equal(testApplications[0].Position, application.Position);
		Assert.Equal(testApplications[0].Status, application.Status);
		Assert.Equal(testApplications[0].DateApplied, application.DateApplied);
	}

	[Fact]
	public async Task GetByIdAsync_ReturnsCorrectApplication() {
		// Arrange:
		using var context = new JobApplicationDbContext(_options);
		var repository = new JobApplicationRepository(_options);

		var testApplication = new JobApplication {
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		await repository.AddAsync(testApplication);

		// Act:
		var result = await repository.GetByIdAsync(testApplication.Id);

		// Assert:
		Assert.NotNull(result);
		Assert.Equal(testApplication.CompanyName, result.CompanyName);
		Assert.Equal(testApplication.Position, result.Position);
		Assert.Equal(testApplication.Status, result.Status);
		Assert.Equal(testApplication.DateApplied, result.DateApplied);
	}

	[Fact]
	public async Task AddAsync_AddsNewApplication() {
		// Arrange:
		var repository = new JobApplicationRepository(_options);

		var newApplication = new JobApplication {
			CompanyName = "New Company",
			Position = "New Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		// Act:
		var result = await repository.AddAsync(newApplication);

		// Assert:
		Assert.NotNull(result);
		Assert.True(result.Id > 0);
		Assert.Equal(newApplication.CompanyName, result.CompanyName);
		Assert.Equal(newApplication.Position, result.Position);
		Assert.Equal(newApplication.Status, result.Status);
		Assert.Equal(newApplication.DateApplied, result.DateApplied);
	}

	[Fact]
	public async Task UpdateAsync_UpdatesExistingApplication() {
		// Arrange:
		using var context = new JobApplicationDbContext(_options);
		var repository = new JobApplicationRepository(_options);

		var testApplication = new JobApplication {
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		await repository.AddAsync(testApplication);

		testApplication.CompanyName = "Updated Company";
		testApplication.Position = "Updated Position";
		testApplication.Status = ApplicationStatus.Interview;

		// Act:
		var result = await repository.UpdateAsync(testApplication);

		// Assert:
		Assert.NotNull(result);
		Assert.Equal(testApplication.CompanyName, result.CompanyName);
		Assert.Equal(testApplication.Position, result.Position);
		Assert.Equal(testApplication.Status, result.Status);
		Assert.Equal(testApplication.DateApplied, result.DateApplied);
	}

	[Fact]
	public async Task DeleteAsync_RemovesExistingApplication() {
		// Arrange:
		using var context = new JobApplicationDbContext(_options);
		var repository = new JobApplicationRepository(_options);

		var testApplication = new JobApplication {
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		await repository.AddAsync(testApplication);

		// Act:
		await repository.DeleteAsync(testApplication.Id);

		// Assert:
		var result = await repository.GetByIdAsync(testApplication.Id);
		Assert.Null(result);
	}
}

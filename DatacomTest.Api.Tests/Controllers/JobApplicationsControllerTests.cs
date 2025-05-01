using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

using DatacomTest.Api.Controllers;
using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

namespace DatacomTest.Api.Tests.Controllers;

[Collection("Database collection")]
public class JobApplicationsControllerTests {
	private readonly Mock<IJobApplicationRepository> _mockRepo;
	private readonly JobApplicationsController _controller;


	public JobApplicationsControllerTests() {
		_mockRepo = new Mock<IJobApplicationRepository>();
		_controller = new JobApplicationsController(_mockRepo.Object);
	}


	[Fact]
	public async Task GetApplications_ReturnsAllApplications() {
		// Arrange:
		var testApplications = new List<JobApplication> {
			new JobApplication {
				Id = 1,
				CompanyName = "Test Company",
				Position = "Test Position",
				Status = ApplicationStatus.Applied,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			}
		};

		_mockRepo.Setup(repo => repo.GetAllAsync())
			.ReturnsAsync(testApplications);

		// Act:
		var result = await _controller.GetApplications();

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result.Result);
		var applications = Assert.IsType<List<JobApplication>>(okResult.Value);
		Assert.Single(applications);
		Assert.Equal(testApplications[0].CompanyName, applications[0].CompanyName);
		Assert.Equal(testApplications[0].Position, applications[0].Position);
		Assert.Equal(testApplications[0].Status, applications[0].Status);
		Assert.Equal(testApplications[0].DateApplied, applications[0].DateApplied);
	}

	[Fact]
	public async Task GetApplications_FiltersByStatus() {
		// Arrange:
		var testApplications = new List<JobApplication> {
			new JobApplication {
				Id = 1,
				CompanyName = "Test Company",
				Position = "Test Position",
				Status = ApplicationStatus.Applied,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			},
			new JobApplication {
				Id = 2,
				CompanyName = "Another Company",
				Position = "Another Position",
				Status = ApplicationStatus.Interview,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			}
		};

		_mockRepo.Setup(repo => repo.GetAllAsync())
			.ReturnsAsync(testApplications);

		// Act:
		var result = await _controller.GetApplications(status: ApplicationStatus.Applied);

		// Assert:
		var okResult = Assert.IsType<OkObjectResult>(result.Result);
		var applications = Assert.IsType<List<JobApplication>>(okResult.Value);
		Assert.Single(applications);
		Assert.Equal(ApplicationStatus.Applied, applications[0].Status);
	}

	[Fact]
	public async Task GetApplications_FiltersByCompanyName() {
		// Arrange:
		var testApplications = new List<JobApplication> {
			new JobApplication {
				Id = 1,
				CompanyName = "Test Company",
				Position = "Test Position",
				Status = ApplicationStatus.Applied,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			},
			new JobApplication {
				Id = 2,
				CompanyName = "Another Company",
				Position = "Another Position",
				Status = ApplicationStatus.Interview,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			}
		};

		_mockRepo.Setup(repo => repo.GetAllAsync())
			.ReturnsAsync(testApplications);

		// Act:
		var result = await _controller.GetApplications(company: "test");

		// Assert:
		var okResult = Assert.IsType<OkObjectResult>(result.Result);
		var applications = Assert.IsType<List<JobApplication>>(okResult.Value);
		Assert.Single(applications);
		Assert.Equal("Test Company", applications[0].CompanyName);
	}

	[Fact]
	public async Task GetApplication_ReturnsNotFoundForNonExistentApplication() {
		// Arrange:
		var id = 1;
		_mockRepo.Setup(repo => repo.GetByIdAsync(id))
			.ReturnsAsync((JobApplication?)null);

		// Act:
		var result = await _controller.GetApplication(id);

		// Assert:
		var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
	}

	[Fact]
	public async Task GetApplication_ReturnsCorrectApplication() {
		// Arrange:
		var testApplication = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockRepo.Setup(repo => repo.GetByIdAsync(testApplication.Id))
			.ReturnsAsync(testApplication);

		// Act:
		var result = await _controller.GetApplication(testApplication.Id);

		// Assert:
		var okResult = Assert.IsType<OkObjectResult>(result.Result);
		var application = Assert.IsType<JobApplication>(okResult.Value);
		Assert.Equal(testApplication.CompanyName, application.CompanyName);
		Assert.Equal(testApplication.Position, application.Position);
		Assert.Equal(testApplication.Status, application.Status);
		Assert.Equal(testApplication.DateApplied, application.DateApplied);
	}

	[Fact]
	public async Task CreateApplication_CreatesNewApplication() {
		// Arrange:
		var newApplication = new JobApplication {
			CompanyName = "New Company",
			Position = "New Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockRepo.Setup(repo => repo.CreateAsync(newApplication))
			.ReturnsAsync(newApplication);

		// Act:
		var result = await _controller.CreateApplication(newApplication);

		// Assert:
		var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
		var returnedApplication = Assert.IsType<JobApplication>(createdResult.Value);
		Assert.Equal(newApplication.CompanyName, returnedApplication.CompanyName);
		Assert.Equal(newApplication.Position, returnedApplication.Position);
		Assert.Equal(newApplication.Status, returnedApplication.Status);
		Assert.Equal(newApplication.DateApplied, returnedApplication.DateApplied);
	}

	[Fact]
	public async Task UpdateApplication_UpdatesExistingApplication() {
		// Arrange:
		var testApplication = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		var updatedApplication = new JobApplication {
			Id = testApplication.Id,
			CompanyName = "Updated Company",
			Position = "Updated Position",
			Status = ApplicationStatus.Interview,
			DateApplied = testApplication.DateApplied
		};

		// Mock both GetByIdAsync and UpdateAsync
		_mockRepo.Setup(repo => repo.GetByIdAsync(testApplication.Id))
			.ReturnsAsync(testApplication);

		_mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<JobApplication>()))
			.ReturnsAsync(updatedApplication);

		// Act:
		var result = await _controller.UpdateApplication(testApplication.Id, updatedApplication);

		// Assert:
		var okResult = Assert.IsType<OkObjectResult>(result.Result);
		var returnedApplication = Assert.IsType<JobApplication>(okResult.Value);
		Assert.Equal(updatedApplication.CompanyName, returnedApplication.CompanyName);
		Assert.Equal(updatedApplication.Position, returnedApplication.Position);
		Assert.Equal(updatedApplication.Status, returnedApplication.Status);
		Assert.Equal(updatedApplication.DateApplied, returnedApplication.DateApplied);
	}

	[Fact]
	public async Task DeleteApplication_DeletesExistingApplication() {
		// Arrange:
		var id = 1;
		_mockRepo.Setup(repo => repo.GetByIdAsync(id))
			.ReturnsAsync(
				new JobApplication {
				Id = id,
				CompanyName = "Test Company",
				Position = "Test Position",
				Status = ApplicationStatus.Applied,
				DateApplied = DateOnly.FromDateTime(DateTime.Now)
			}
		);

		// Act:
		var result = await _controller.DeleteApplication(id);

		// Assert:
		var noContentResult = Assert.IsType<NoContentResult>(result);
	}
}

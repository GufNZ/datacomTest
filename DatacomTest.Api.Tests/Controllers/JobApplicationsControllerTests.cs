using Microsoft.AspNetCore.Mvc;

using Moq;

using DatacomTest.Api.Controllers;
using DatacomTest.Api.Models;
using DatacomTest.Api.Services;
using DatacomTest.Api.Tests.Extensions;

namespace DatacomTest.Api.Tests.Controllers;

public class JobApplicationsControllerTests {
	private readonly Mock<IJobApplicationService> _mockService;
	private readonly JobApplicationsController _controller;


	public JobApplicationsControllerTests() {
		_mockService = new Mock<IJobApplicationService>();
		_controller = new JobApplicationsController(_mockService.Object);
	}


	[Fact]
	public async Task GetApplications_ReturnsPaginatedResponse() {
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

		var paginatedResponse = new PaginatedResponse<JobApplication> {
			Data = applications,
			Total = 1,
			Page = 1,
			Pages = 1,
			Limit = 10
		};

		_mockService
			.Setup(
				service => service.GetApplicationsAsync(
					/* page */ It.IsAny<int>(),
					/* limit */ It.IsAny<int>(),
					/* status */ It.IsAny<ApplicationStatus?>(),
					/* company */ It.IsAny<string>(),
					/* position */ It.IsAny<string>()
				)
			)
			.ReturnsAsync(paginatedResponse);

		// Act:
		var result = await _controller.GetApplications(page: 1, limit: 10);

		// Assert:
		var actionResult = Assert.IsType<ActionResult<PaginatedResponse<JobApplication>>>(result);
		var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
		var response = Assert.IsType<PaginatedResponse<JobApplication>>(okResult.Value);
		Assert.Single(response.Data);
		Assert.Equal(1, response.Total);
		Assert.Equal(1, response.Pages);
		Assert.Equal(1, response.Page);
		Assert.Equal(10, response.Limit);
	}

	[Fact]
	public async Task GetApplication_ReturnsApplication_WhenFound() {
		// Arrange:
		var application = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockService
			.Setup(service => service.GetApplicationAsync(1))
			.ReturnsAsync(application);

		// Act:
		var result = await _controller.GetApplication(1);

		// Assert:
		var actionResult = Assert.IsType<ActionResult<JobApplication>>(result);
		var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
		var returnedApp = Assert.IsType<JobApplication>(okResult.Value);
		Assert.Equal(1, returnedApp.Id);
	}

	[Fact]
	public async Task GetApplication_ReturnsNotFound_WhenNotFound() {
		// Arrange:
		_mockService
			.Setup(service => service.GetApplicationAsync(1))
			.ReturnsAsync((JobApplication?)null);

		// Act:
		var result = await _controller.GetApplication(1);

		// Assert:
		var actionResult = Assert.IsType<ActionResult<JobApplication>>(result);
		Assert.IsType<NotFoundResult>(actionResult.Result);
	}

	[Fact]
	public async Task CreateApplication_CreatesNewApplication() {
		// Arrange:
		var application = new JobApplication {
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		const int EXPECTED_ID = 1;
		application.Id = EXPECTED_ID;

		_mockService
			.Setup(service => service.CreateApplicationAsync(It.IsAny<JobApplication>()))
			.ReturnsAsync(application);

		// Act:
		var result = await _controller.CreateApplication(application);

		// Assert:
		_mockService.Verify(service => service.CreateApplicationAsync(It.IsAny<JobApplication>()), Times.Once);
		var actionResult = Assert.IsType<ActionResult<JobApplication>>(result);
		var createdAtAction = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
		var createdApp = Assert.IsType<JobApplication>(createdAtAction.Value);
		Assert.Equal(EXPECTED_ID, createdApp.Id);
	}

	[Fact]
	public async Task UpdateApplication_UpdatesExistingApplication() {
		// Arrange:
		var application = new JobApplication {
			Id = 1,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		_mockService
			.Setup(service => service.UpdateApplicationAsync(1, application))
			.ReturnsAsync(application);

		// Act:
		var result = await _controller.UpdateApplication(1, application);

		// Assert:
		_mockService.Verify(service => service.UpdateApplicationAsync(1, application), Times.Once);
		var actionResult = Assert.IsType<ActionResult<JobApplication>>(result);
		var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
		var returnedApp = Assert.IsType<JobApplication>(okResult.Value);
		Assert.Equal(1, returnedApp.Id);
	}

	[Fact]
	public async Task UpdateApplication_ReturnsBadRequest_WhenIdsDontMatch() {
		// Arrange:
		var application = new JobApplication {
			Id = 2,
			CompanyName = "Test Company",
			Position = "Test Position",
			Status = ApplicationStatus.Applied,
			DateApplied = DateOnly.FromDateTime(DateTime.Now)
		};

		// Act & Assert:
		var result = await _controller.UpdateApplication(1, application);
		var badRequestResult = Assert.IsType<ActionResult<JobApplication>>(result);
		var badRequest = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
		Assert.Equal("Application ID in URL does not match the ID in the request body!", badRequest.Value);
	}

	[Fact]
	public async Task DeleteApplication_DeletesExistingApplication() {
		// Arrange:
		const int ID = 1;
		_mockService
			.Setup(service => service.DeleteApplicationAsync(ID))
			.ReturnsAsync();

		// Act:
		var result = await _controller.DeleteApplication(ID);

		// Assert:
		Assert.IsType<NoContentResult>(result);
	}
}

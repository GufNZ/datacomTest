using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

using DatacomTest.Api.Models;
using DatacomTest.Api.Services;

namespace DatacomTest.Api.Controllers;

[ApiController]
[Route("api/applications")]
[Produces(MediaTypeNames.Application.Json)]
public class JobApplicationsController : ControllerBase {
	private readonly IJobApplicationService _service;


	public JobApplicationsController(IJobApplicationService service) {
		_service = service;
	}


	/// <summary>Get job applications with optional filtering and pagination.</summary>
	/// <param name="status">Filter by application status (optional)</param>
	/// <param name="company">Filter by company name (optional)</param>
	/// <param name="position">Filter by position (optional)</param>
	/// <param name="page">Page number (optional, default: 1)</param>
	/// <param name="limit">Number of items per page (optional, default: 10)</param>
	/// <returns>Paginated list of filtered job applications.</returns>
	[HttpGet]
	public async Task<ActionResult<PaginatedResponse<JobApplication>>> GetApplications(
		[FromQuery] ApplicationStatus? status = null,
		[FromQuery] string? company = null,
		[FromQuery] string? position = null,
		[FromQuery] int page = 1,
		[FromQuery] int limit = 10
	) {
		var result = await _service.GetApplicationsAsync(page, limit, status, company, position);
		return Ok(result);
	}

	/// <summary>Get a specific job application by ID.</summary>
	/// <param name="id">Application ID.</param>
	/// <returns>Job application details.</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<JobApplication>> GetApplication(int id) {
		var application = await _service.GetApplicationAsync(id);
		if (application == null) {
			return NotFound();
		}

		return Ok(application);
	}

	/// <summary>Create a new job application.</summary>
	/// <param name="application">Job application data.</param>
	/// <returns>Created job application.</returns>
	[HttpPost]
	public async Task<ActionResult<JobApplication>> CreateApplication(JobApplication application) {
		var createdApplication = await _service.CreateApplicationAsync(application);
		return CreatedAtAction(nameof(GetApplication), new { id = createdApplication.Id }, createdApplication);
	}

	/// <summary>Update an existing job application.</summary>
	/// <param name="id">Application ID.</param>
	/// <param name="application">Updated job application data.</param>
	/// <returns>Updated job application.</returns>
	[HttpPut("{id}")]
	public async Task<ActionResult<JobApplication>> UpdateApplication(int id, JobApplication application) {
		if (application.Id != id) {
			return BadRequest("Application ID in URL does not match the ID in the request body!");
		}

		try {
			var updatedApplication = await _service.UpdateApplicationAsync(id, application);
			return Ok(updatedApplication);
		} catch (KeyNotFoundException ex) {
			return NotFound(ex.Message);
		}
	}

	/// <summary>Delete an existing job application.</summary>
	/// <param name="id">Application ID.</param>
	/// <returns>No content if successful, NotFound if application doesn't exist.</returns>
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteApplication(int id) {
		try {
			await _service.DeleteApplicationAsync(id);
			return NoContent();
		} catch (KeyNotFoundException) {
			return NotFound();
		}
	}
}

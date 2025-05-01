using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

namespace DatacomTest.Api.Controllers;

[ApiController]
[Route("api/applications")]
[Produces(MediaTypeNames.Application.Json)]
public class JobApplicationsController : ControllerBase {
	private readonly IJobApplicationRepository _repository;


	public JobApplicationsController(IJobApplicationRepository repository) {
		_repository = repository;
	}


	/// <summary>Get job applications with optional filtering.</summary>
	/// <param name="status">Filter by application status (optional)</param>
	/// <param name="company">Filter by company name (optional)</param>
	/// <param name="position">Filter by position (optional)</param>
	/// <returns>List of filtered job applications.</returns>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<JobApplication>>> GetApplications(
		[FromQuery] ApplicationStatus? status = null,
		[FromQuery] string? company = null,
		[FromQuery] string? position = null
	) {
		var applications = await _repository.GetAllAsync();

		if (status.HasValue) {
			applications = applications.Where(a => a.Status == status.Value);
		}

		if (!string.IsNullOrWhiteSpace(company)) {
			applications = applications.Where(a => a.CompanyName.Contains(company, StringComparison.OrdinalIgnoreCase));
		}

		if (!string.IsNullOrWhiteSpace(position)) {
			applications = applications.Where(a => a.Position.Contains(position, StringComparison.OrdinalIgnoreCase));
		}

		var filteredApplications = applications.ToList();
		return Ok(filteredApplications);
	}

	/// <summary>Get a specific job application by ID.</summary>
	/// <param name="id">Application ID.</param>
	/// <returns>Single job application.</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<JobApplication>> GetApplication(int id) {
		var application = await _repository.GetByIdAsync(id);
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
		if (application == null) {
			return BadRequest();
		}


		var createdApplication = await _repository.CreateAsync(application);

		return CreatedAtAction(nameof(GetApplication), new { id = createdApplication.Id }, createdApplication);
	}


	/// <summary>Update an existing job application.</summary>
	/// <param name="id">Application ID.</param>
	/// <param name="application">Updated job application data.</param>
	/// <returns>Updated job application.</returns>
	[HttpPut("{id}")]
	public async Task<ActionResult<JobApplication>> UpdateApplication(int id, JobApplication application) {
		if (application == null || application.Id != id) {
			return BadRequest();
		}


		var existingApplication = await _repository.GetByIdAsync(id);
		if (existingApplication == null) {
			return NotFound();
		}


		application.Id = id;
		var updatedApplication = await _repository.UpdateAsync(application);

		return Ok(updatedApplication);
	}

	/// <summary>Delete an existing job application.</summary>
	/// <param name="id">Application ID.</param>
	/// <returns>No content if successful, NotFound if application doesn't exist.</returns>
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteApplication(int id) {
		var application = await _repository.GetByIdAsync(id);
		if (application == null) {
			return NotFound();
		}


		await _repository.DeleteAsync(id);

		return NoContent();
	}
}

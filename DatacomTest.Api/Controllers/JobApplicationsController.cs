using System.Net.Mime;

using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace DatacomTest.Api.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	[Produces(MediaTypeNames.Application.Json)]
	public class JobApplicationsController : ControllerBase {
		private readonly IJobApplicationRepository _repository;


		public JobApplicationsController(IJobApplicationRepository repository) {
			_repository = repository;
		}


		/// <summary>Get all job applications.</summary>
		/// <returns>List of jobapplications applications.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<JobApplication>>> GetApplications() {
			var applications = await _repository.GetAllAsync();
			return Ok(applications);
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
	}
}

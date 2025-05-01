using System;

using Microsoft.EntityFrameworkCore;

using DatacomTest.Api.Models;
using DatacomTest.Api.Repositories;

namespace DatacomTest.Api.Tests.Repositories;

public class JobApplicationRepositoryTestDatabaseFixture {
	public JobApplicationRepositoryTestDatabaseFixture() {
		Options = new DbContextOptionsBuilder<JobApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;
	}


	public DbContextOptions<JobApplicationDbContext> Options { get; }
}

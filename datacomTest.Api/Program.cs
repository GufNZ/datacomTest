using Microsoft.EntityFrameworkCore;

using DatacomTest.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure database:
builder.Services.AddDbContext<JobApplicationDbContext>(
	options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
		?? "Data Source=jobapplications.db")
);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	// Ensure database schema is created in development mode:
	using (var scope = app.Services.CreateScope()) {
		var services = scope.ServiceProvider;
		try {
			var context = services.GetRequiredService<JobApplicationDbContext>();
			await context.Database.EnsureDeletedAsync();
			await context.Database.EnsureCreatedAsync();
		} catch (Exception ex) {
			var logger = services.GetRequiredService<ILogger<Program>>();
			logger.LogError(ex, "An error occurred while creating the database.");
		}
	}

	// Configure the HTTP request pipeline:
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

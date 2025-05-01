using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;

using DatacomTest.Api.Repositories;
using DatacomTest.Api.Controllers;

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

// Configure the HTTP request pipeline:
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

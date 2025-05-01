namespace DatacomTest.Api.Models;

public class JobApplication {
	public int Id { get; set; }
	public string CompanyName { get; set; } = "";
	public string Position { get; set; } = "";
	public ApplicationStatus Status { get; set; }
	public DateOnly DateApplied { get; set; }
}

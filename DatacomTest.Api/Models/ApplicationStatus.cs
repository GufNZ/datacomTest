namespace DatacomTest.Api.Models {
	using System.Text.Json.Serialization;

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ApplicationStatus {
		Applied,
		Interview,
		Offer,
		Rejected
	}
}

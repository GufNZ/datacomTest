using System.Text.Json.Serialization;

namespace DatacomTest.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationStatus {
	Applied,
	Interview,
	Offer,
	Rejected
}

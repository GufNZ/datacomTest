namespace DatacomTest.Api.Services;

public interface IDataSeedService {
	Task SeedDataAsync(int count = 20);
}

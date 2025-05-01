using Moq.Language.Flow;

namespace DatacomTest.Api.Tests.Extensions;

public static class MockExtensions {
	public static IReturnsResult<TMock> ReturnsAsync<TMock>(
		this ISetup<TMock, Task> setup
	) where TMock : class {
		return setup.Returns(Task.CompletedTask);
	}
}

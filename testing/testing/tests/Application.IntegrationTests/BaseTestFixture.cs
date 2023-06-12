using NUnit.Framework;

namespace testing.Application.IntegrationTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture {
    [SetUp]
    public async Task TestSetUp() {
        await ResetState();
    }
}
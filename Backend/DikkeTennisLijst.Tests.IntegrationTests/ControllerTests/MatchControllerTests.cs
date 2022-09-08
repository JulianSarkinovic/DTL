using NUnit.Framework;
using static DikkeTennisLijst.Tests.IntegrationTests.Setup;

namespace DikkeTennisLijst.Tests.IntegrationTests.ControllerTests
{
    [TestFixture]
    public class MatchControllerTests
    {
        [Test]
        public async Task GetAsync_ReturnsMatch()
        {
            var response = await TestHttpClient.GetAsync("Match/GetAsync");
            Assert.That(response, Is.Not.Null);
        }
    }
}
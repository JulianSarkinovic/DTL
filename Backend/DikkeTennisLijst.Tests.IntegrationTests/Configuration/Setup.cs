using DikkeTennisLijst.Tests.IntegrationTests.Configuration;
using NUnit.Framework;

namespace DikkeTennisLijst.Tests.IntegrationTests
{
    [SetUpFixture]
    public static class Setup
    {
        public static IntegrationTestFactory TestFactory { get; private set; }
        public static HttpClient TestHttpClient { get; private set; }

        [OneTimeSetUp]
        public static void RunBeforeAnyTests()
        {
            TestFactory = new IntegrationTestFactory();
            TestHttpClient = TestFactory.CreateDefaultClient();
        }

        [OneTimeTearDown]
        public static void RunAfterAnyTests()
        {
            TestFactory?.Dispose();
            TestHttpClient?.Dispose();
        }
    }
}
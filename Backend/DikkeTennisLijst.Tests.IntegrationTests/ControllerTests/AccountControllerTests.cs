using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Infrastructure.Serialization;
using DikkeTennisLijst.Tests.IntegrationTests.Configuration;
using DikkeTennisLijst.WebAPI.Models.Request;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using static DikkeTennisLijst.Tests.IntegrationTests.Setup;

namespace DikkeTennisLijst.Tests.IntegrationTests.ControllerTests
{
    [TestFixture]
    [Ignore("Not finished, not working.")]
    public class AccountControllerTests
    {
        private UserManager<Player> UserManager { get; set; }
        private PlayerRegisterRequestModel Player { get; set; }

        [SetUp]
        public void SetUp()
        {
            UserManager = TestFactory.Services.GetRequiredScopedService<UserManager<Player>>();

            Player = new PlayerRegisterRequestModel
            {
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "testmail@integrationtests.com",
                Password = "P@ssw0rd",
                ConfirmationPassword = "P@ssw0rd"
            };
        }

        [TearDown]
        public async Task TearDown()
        {
            var player = await UserManager.FindByEmailAsync(Player.Email);
            if (player != null)
            {
                await UserManager.DeleteAsync(player);
            }
        }

        [Test]
        public async Task GetAsync_ReturnsMatch()
        {
            //var body = new StringContent(JsonConvert.SerializeObject(Player));

            var body = new ByteArrayContent(ObjectSerializer.SerializeToUtf8Bytes(Player));

            var response = await TestHttpClient.PostAsync("api/Account/Register", body);

            response.EnsureSuccessStatusCode();
            Assert.That(response, Is.Not.Null);
        }
    }
}
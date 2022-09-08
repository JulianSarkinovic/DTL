using DikkeTennisLijst.Infrastructure.Repositories;
using DikkeTennisLijst.Tests.Common.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace DikkeTennisLijst.Tests.IntegrationTests.Configuration
{
    public class IntegrationTestFactory : WebApplicationFactory<WebAPI.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("IntegrationTestDB"));
                services.AddScoped<DbContext, TestDbContext>();

                services.AddScoped<BlobObjectRepository<TestObject>>();
                services.AddScoped<BlobObjectRepository<TestEntity>>();
                services.AddScoped<SQLEntityRepository<TestEntity>>();
            });
        }
    }
}
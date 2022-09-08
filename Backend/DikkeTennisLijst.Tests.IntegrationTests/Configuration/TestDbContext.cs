using DikkeTennisLijst.Tests.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace DikkeTennisLijst.Tests.IntegrationTests.Configuration
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
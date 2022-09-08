using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Shared.Exceptions;
using DikkeTennisLijst.Infrastructure.Repositories;
using DikkeTennisLijst.Tests.Common.Entities;
using DikkeTennisLijst.Tests.Common.Specifications;
using DikkeTennisLijst.Tests.IntegrationTests.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using static DikkeTennisLijst.Tests.IntegrationTests.Setup;

namespace DikkeTennisLijst.Tests.IntegrationTests.RepositoryTests
{
    [TestFixture]
    public class BlobObjectRepositoryTests : IObjectRepositoryTests<BlobObjectRepository<TestObject>, TestObject>
    {
        public override async Task SetupAsync()
        {
            await base.SetupAsync();
            await Repository.DeleteRangeAsync();
        }
    }

    [TestFixture]
    public class BlobEntityRepositoryTests : IEntityRepositoryTests<BlobObjectRepository<TestEntity>, TestEntity>
    {
        public override async Task SetupAsync()
        {
            await base.SetupAsync();
            await Repository.DeleteRangeAsync();
        }
    }

    [TestFixture]
    public class SqlRepositoryTests : IEntityRepositoryTests<SQLEntityRepository<TestEntity>, TestEntity>
    {
        public override async Task SetupAsync()
        {
            await base.SetupAsync();

            var context = TestFactory.Services.GetRequiredScopedService<TestDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Repository = new SQLEntityRepository<TestEntity>(context);
        }
    }

    public abstract class IEntityRepositoryTests<TRepository, TTestObject> : IObjectRepositoryTests<TRepository, TTestObject>
        where TRepository : IObjectRepository<TTestObject>
        where TTestObject : ITestObject, IHasId, new()
    {
        public override async Task SetupAsync()
        {
            await base.SetupAsync();

            Alberto.Id = 0;
            Bernard.Id = 0;
            Cassius.Id = 0;
            Donaldo.Id = 0;
            Eduardo.Id = 0;
        }

        [Test]
        public override async Task AddAsync_WhenDatabaseHasObject_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.AddAsync(EntitiesABC[0]);
            await method.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public override async Task AddRangeAsync_WhenDatabaseHasObjects_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.AddRangeAsync(EntitiesABC);
            await method.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public override async Task AddRangeAsync_WhenDatabaseHasSomeOfTheObjects_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.AddRangeAsync(new[] { Alberto, Donaldo });
            await method.Should().ThrowAsync<ArgumentException>();
        }
    }

    public abstract class IObjectRepositoryTests<TRepository, TTestObject>
        where TRepository : IObjectRepository<TTestObject>
        where TTestObject : ITestObject, IHasId, new()
    {
        protected const string PeopleGroup = "People";
        protected const int IdThatDoesNotExist = -1;

        protected IObjectRepository<TTestObject> Repository { get; set; }
        protected List<TTestObject> EntitiesABC { get; set; }
        protected TTestObject Alberto { get; set; }
        protected TTestObject Bernard { get; set; }
        protected TTestObject Cassius { get; set; }
        protected TTestObject Donaldo { get; set; }
        protected TTestObject Eduardo { get; set; }

        [SetUp]
        public virtual async Task SetupAsync()
        {
            Repository = TestFactory.Services.GetRequiredScopedService<TRepository>();

            Alberto = new() { Id = 1, SomeNumber = 1, Name = nameof(Alberto), Group = PeopleGroup };
            Bernard = new() { Id = 2, SomeNumber = 2, Name = nameof(Bernard), Group = PeopleGroup };
            Cassius = new() { Id = 3, SomeNumber = 3, Name = nameof(Cassius), Group = PeopleGroup };
            Donaldo = new() { Id = 4, SomeNumber = 4, Name = nameof(Donaldo), Group = PeopleGroup };
            Eduardo = new() { Id = 5, SomeNumber = 5, Name = nameof(Eduardo), Group = PeopleGroup };

            EntitiesABC = new List<TTestObject> { Alberto, Bernard, Cassius };
        }

        [Test]
        public async Task AddAsync__WhenDatabaseIsEmpty_ObjectIsAdded()
        {
            await Repository.AddAsync(Donaldo);

            var entity = await Repository.GetAsync(Donaldo.Id);
            entity.Should().BeEquivalentTo(Donaldo);
        }

        [Test]
        public async Task AddAsync__WhenDatabaseDoesNotHaveObject_ObjectIsAddedToExisting()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            await Repository.AddAsync(Donaldo);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().HaveCount(4);
            entities.Find(x => x.Id == Donaldo.Id).Should().BeEquivalentTo(Donaldo);
        }

        [Test]
        public virtual async Task AddAsync_WhenDatabaseHasObject_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.AddAsync(EntitiesABC[0]);
            await method.Should().ThrowAsync<DuplicateElementException>();
        }

        [Test]
        public async Task AddRangeAsync_WhenDatabaseIsEmpty_ObjectsAreAdded()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEquivalentTo(EntitiesABC);
        }

        [Test]
        public async Task AddRangeAsync_WhenDatabaseDoesNotHaveObjects_ObjectsAreAdded()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            await Repository.AddRangeAsync(new[] { Donaldo, Eduardo });

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().HaveCount(5);
            entities.Find(x => x.Id == Donaldo.Id).Should().BeEquivalentTo(Donaldo);
        }

        [Test]
        public virtual async Task AddRangeAsync_WhenDatabaseHasObjects_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.AddRangeAsync(EntitiesABC);
            await method.Should().ThrowAsync<DuplicateElementException>();
        }

        [Test]
        public virtual async Task AddRangeAsync_WhenDatabaseHasSomeOfTheObjects_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.AddRangeAsync(new[] { Alberto, Donaldo });
            await method.Should().ThrowAsync<DuplicateElementException>();
        }

        [Test]
        public async Task GetAsync_ById_WhenDatabaseDoesNotHaveObject_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.GetAsync(IdThatDoesNotExist);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_ById_WhenDatabaseHasObject_Gets()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entity = await Repository.GetAsync(Alberto.Id);

            entity.Should().BeEquivalentTo(Alberto);
        }

        [Test]
        public async Task GetAsync_BySpec_WhenDatabaseDoesNotHaveObject_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(IdThatDoesNotExist);

            Func<Task> method = async () => await Repository.GetAsync(spec);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_BySpec_WhenDatabaseHasMultipleMatchingObjects_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id > 1);

            Func<Task> method = async () => await Repository.GetAsync(spec);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_BySpec_WhenDatabaseHasSingleObject_Gets()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(Alberto.Id);

            var entity = await Repository.GetAsync(spec);

            entity.Should().BeEquivalentTo(Alberto);
        }

        [Test]
        public async Task GetAsync_ByIdAndSpec_WhenDatabaseHasNoObjectWithIdSpecDoesNotMatch_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(IdThatDoesNotExist);

            Func<Task> method = async () => await Repository.GetAsync(IdThatDoesNotExist, spec);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_ByIdAndSpec_WhenDatabaseHasIdSpecDoesNotMatch_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(IdThatDoesNotExist);

            Func<Task> method = async () => await Repository.GetAsync(Alberto.Id, spec);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_ByIdAndSpec_WhenDatabaseHasIdSpecMatches_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(Alberto.Id);

            Func<Task> method = async () => await Repository.GetAsync(IdThatDoesNotExist, spec);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_ByIdAndSpec_WhenDatabaseHasIdHasSpecNotSameEntity_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(Alberto.Id);

            Func<Task> method = async () => await Repository.GetAsync(Bernard.Id, spec);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task GetAsync_ByIdAndSpec_WhenDatabaseHasIdHasSpecSameEntity_Gets()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var spec = new SpecificationTestEntity<TTestObject>(x => x.Group.Equals(PeopleGroup));

            var entity = await Repository.GetAsync(Alberto.Id, spec);

            entity.Should().BeEquivalentTo(Alberto);
        }

        [Test]
        public async Task GetRangeAsync_WhenDatabaseIsEmpty_EmptyEnumerable()
        {
            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEmpty();
        }

        [Test]
        public async Task GetRangeAsync_WhenDatabaseHasDataAndNoSpecification_AllData()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEquivalentTo(EntitiesABC);
        }

        [Test]
        public async Task GetRangeAsync_WhenDatabaseHasDataAndSpecMatches_MatchedData()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            var expected = new List<TTestObject> { Alberto, Bernard };

            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id < 3);
            var entities = await Repository.GetRangeAsync(spec).ToListAsync();

            entities.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetRangeAsync_WhenDatabaseHasDataButSpecDoesNotMatch_Empty()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id == IdThatDoesNotExist);
            var entities = await Repository.GetRangeAsync(spec).ToListAsync();

            entities.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateAsync_WhenDataBaseHasData_UpdatesData()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            const string newName = "Albertina";
            Alberto.Name = newName;

            await Repository.UpdateAsync(Alberto);

            var entity = await Repository.GetAsync(Alberto.Id);
            entity.Name.Should().BeEquivalentTo(newName);
        }

        [Test]
        public async Task UpdateAsync_WhenDataBaseDoesNotHaveData_Throws()
        {
            Alberto.Name = "Albertino";

            Func<Task> method = async () => await Repository.UpdateAsync(Alberto);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task UpdateRangeAsync_WhenDataBaseHasData_UpdatesData()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            const string newGroup = "Animals";

            foreach (var item in EntitiesABC)
            {
                item.Group = newGroup;
            }

            await Repository.UpdateRangeAsync(EntitiesABC);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            var areUpdated = entities.All(x => x.Group.Contains(newGroup, StringComparison.OrdinalIgnoreCase));

            areUpdated.Should().BeTrue();
        }

        [Test]
        public async Task UpdateRangeAsync_WhenDataBaseDoesNotHaveData_Throws()
        {
            const string newGroup = "Animals";

            foreach (var item in EntitiesABC)
            {
                item.Group = newGroup;
            }

            Func<Task> method = async () => await Repository.UpdateRangeAsync(EntitiesABC);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task UpdateOrAddAsync_WhenDataBaseHasData_UpdatesData()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            const string newName = "Albertina";
            Alberto.Name = newName;

            await Repository.AddOrUpdateAsync(Alberto);

            var entity = await Repository.GetAsync(Alberto.Id);
            entity.Name.Should().BeEquivalentTo(newName);
        }

        [Test]
        public async Task UpdateOrAddAsync_WhenDataBaseDoesNotHaveData_AddsData()
        {
            const string newName = "Albertina";
            Alberto.Name = newName;

            await Repository.AddOrUpdateAsync(Alberto);

            var entity = await Repository.GetAsync(Alberto.Id);
            entity.Name.Should().BeEquivalentTo(newName);
        }

        [Test]
        public async Task UpdateOrAddRangeAsync_WhenDataBaseHasData_UpdatesData()
        {
            await Repository.AddRangeAsync(EntitiesABC);
            const string newGroup = "Animals";

            foreach (var item in EntitiesABC)
            {
                item.Group = newGroup;
            }

            await Repository.AddOrUpdateRangeAsync(EntitiesABC);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            var areUpdated = entities.All(x => x.Group.Contains(newGroup, StringComparison.OrdinalIgnoreCase));

            areUpdated.Should().BeTrue();
        }

        [Test]
        public async Task UpdateOrAddRangeAsync_WhenDataBaseDoesNotHaveData_AddsData()
        {
            const string newGroup = "Animals";

            foreach (var item in EntitiesABC)
            {
                item.Group = newGroup;
            }

            await Repository.AddOrUpdateRangeAsync(EntitiesABC);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            var areUpdated = entities.All(x => x.Group.Contains(newGroup, StringComparison.OrdinalIgnoreCase));

            areUpdated.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_ByObject_WhenNotExists_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entity = new TTestObject() { Id = IdThatDoesNotExist };
            Func<Task> method = async () => await Repository.DeleteAsync(entity);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task DeleteAsync_ByObject_WhenExists_Deletes()
        {
            var expected = new List<TTestObject> { Bernard, Cassius };
            await Repository.AddRangeAsync(EntitiesABC);

            var entity = await Repository.GetAsync(Alberto.Id);
            await Repository.DeleteAsync(entity);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task DeleteAsync_ById_WhenNotExists_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            Func<Task> method = async () => await Repository.DeleteAsync(IdThatDoesNotExist);
            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task DeleteAsync_ById_WhenExists_Deletes()
        {
            var expected = new List<TTestObject> { Bernard, Cassius };
            await Repository.AddRangeAsync(EntitiesABC);

            await Repository.DeleteAsync(Alberto.Id);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task DeleteRangeAsync_BySpec_NoSpec__AllDeleted()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            await Repository.DeleteRangeAsync();
            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEmpty();
        }

        [Test]
        public async Task DeleteRangeAsync_BySpec_SpecMatchesSome_SomeAreDeleted()
        {
            var expected = new List<TTestObject> { Cassius };
            await Repository.AddRangeAsync(EntitiesABC);

            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id < 3);
            await Repository.DeleteRangeAsync(spec);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task DeleteRangeAsync_BySpec_SpecMatchesNone_NoneAreDeleted()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id > 10);
            await Repository.DeleteRangeAsync(spec);

            var entities = await Repository.GetRangeAsync().ToListAsync();
            entities.Should().BeEquivalentTo(EntitiesABC);
        }

        [Test]
        public async Task DeleteRangeAsync_ByObjects_ObjectsMatchSomeButSomeDoNotExist_Throws()
        {
            var expected = new List<TTestObject> { Cassius };
            await Repository.AddRangeAsync(EntitiesABC);

            var entitiesToDelete = new[] { Cassius, new() { Id = IdThatDoesNotExist } };
            Func<Task> method = async () => await Repository.DeleteRangeAsync(entitiesToDelete);

            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task DeleteRangeAsync_ByObjects_ObjectsMatchNone_Throws()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entitiesToDelete = new List<TTestObject>() { new() { Id = IdThatDoesNotExist } };
            Func<Task> method = async () => await Repository.DeleteRangeAsync(entitiesToDelete);

            await method.Should().ThrowAsync<NonExistentElementException>();
        }

        [Test]
        public async Task DeleteRangeAsync_ByObjects_AllObjectsExist_ThoseObjectsDeleted()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            await Repository.DeleteRangeAsync(new List<TTestObject>() { Alberto, Bernard });

            var entities = await Repository.GetRangeAsync().ToListAsync();

            entities.Count.Should().Be(1);
            entities[0].Id.Should().Be(Cassius.Id);
        }

        [Test]
        public async Task ExistsAsync_WhenNotExists_ReturnsFalse()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var found = await Repository.ExistsAsync(IdThatDoesNotExist);

            found.Should().BeFalse();
        }

        [Test]
        public async Task ExistsAsync_WhenExists_ReturnsTrue()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var found = await Repository.ExistsAsync(Alberto.Id);

            found.Should().BeTrue();
        }

        [Test]
        public async Task ExistsRangeAsync_BySpec_WhenNotExists_ReturnsFalse()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id == IdThatDoesNotExist);
            var found = await Repository.ExistsRangeAsync(spec);

            found.Should().BeFalse();
        }

        [Test]
        public async Task ExistsRangeAsync_BySpec_WhenExists_ReturnsTrue()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var spec = new SpecificationTestEntity<TTestObject>(x => x.Id == Alberto.Id);
            var found = await Repository.ExistsRangeAsync(spec);

            found.Should().BeTrue();
        }

        [Test]
        public async Task ExistsRangeAsync_ByObjects_WhenNotExists_ReturnsFalse()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entitiesToCheck = new List<TTestObject>() { new() { Id = IdThatDoesNotExist } };
            var found = await Repository.ExistsRangeAsync(entitiesToCheck);

            found.Should().BeFalse();
        }

        [Test]
        public async Task ExistsRangeAsync_ByObjects_WhenSomeExist_ReturnsFalse()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var entitiesToCheck = new List<TTestObject>() { Cassius, new() { Id = IdThatDoesNotExist } };
            var found = await Repository.ExistsRangeAsync(entitiesToCheck);

            found.Should().BeFalse();
        }

        [Test]
        public async Task ExistsRangeAsync_ByObjects_WhenAllExist_ReturnsTrue()
        {
            await Repository.AddRangeAsync(EntitiesABC);

            var found = await Repository.ExistsRangeAsync(new List<TTestObject>() { Cassius, Alberto });

            found.Should().BeTrue();
        }
    }
}
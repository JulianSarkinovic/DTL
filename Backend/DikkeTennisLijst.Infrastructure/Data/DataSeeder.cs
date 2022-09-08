using DikkeTennisLijst.Core.Calculations;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Constants;
using DikkeTennisLijst.Core.Shared.Enums;
using DikkeTennisLijst.Core.Shared.Helpers;
using DikkeTennisLijst.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DikkeTennisLijst.Infrastructure.Data
{
    public static class HostDataSeedExtension
    {
        public static async Task<IHost> SeedDataAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationContext>();
            var logger = services.GetRequiredService<ILogger<DataSeeder>>();
            var userManager = services.GetRequiredService<UserManager<Player>>();
            var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = env?.Equals("development", StringComparison.OrdinalIgnoreCase) == true;

            var dataSeeder = new DataSeeder(context, userManager, logger, isDevelopment, appSettings);
            await dataSeeder.SeedAll();

            return host;
        }
    }

    public class DataSeeder
    {
        private ApplicationContext Context { get; init; }
        private UserManager<Player> UserManager { get; init; }
        private ILogger<DataSeeder> Logger { get; init; }
        private bool IsDevelopment { get; init; }
        private AppSettings AppSettings { get; init; }

        private Player UserB { get; set; }
        private Player UserC { get; set; }
        private Player UserD { get; set; }
        private Surface SurfaceClay { get; set; }
        private Surface SurfaceHard { get; set; }
        private Surface SurfaceGraz { get; set; }
        private Club ClubAms { get; set; }
        private Club ClubJoy { get; set; }

        public DataSeeder(
            ApplicationContext applicationContext,
            UserManager<Player> userManager,
            ILogger<DataSeeder> logger,
            bool isDevelopment,
            AppSettings appSettings)
        {
            Context = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            IsDevelopment = isDevelopment;
            AppSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// Seeds the database.
        /// The database is not thread safe, await before other database actions are performed.
        /// </summary>
        public async Task SeedAll()
        {
            try
            {
                Logger.LogInformation("Migrating SQL database...");
                Context.Database.Migrate();
                Logger.LogInformation("Migrating SQL database finished.");

                Logger.LogInformation("Seeding SQL database...");
                await SeedSQLAsync(reseed: false);
                Logger.LogInformation("Seeding SQL database finished.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task SeedSQLAsync(bool reseed)
        {
            if (IsDevelopment)
            {
                await Seed0_AdminAsync(reseed);
                await Seed1_PlayersAsync();
                await Seed2_AddressesAsync(reseed);
                await Seed3_FriendshipsAsync(reseed);
                await Seed5_ClubsAsync(reseed);
                await Seed6_SurfacesAsync(reseed);
                await Seed7_SurfaceClubJoinsAsync(reseed);
                await Seed4_FollowingAsync(reseed);
                await Seed8_PlayerClubsAsync(reseed);
                await Seed9_MatchesAsync(reseed);
                await Seed10_EloRankedHistoryAsync();
                await Seed11_EloCasualHistoryAsync();
            }
        }

        private async Task Seed0_AdminAsync(bool reset)
        {
            if (reset) await DeleteUsers();

            if (await UserManager.FindByEmailAsync(AppSettings.AdminEmail) == null)
            {
                var player = new Player("Julian", "Sarkinovic", AppSettings.AdminEmail)
                {
                    DateOfBirth = DateTimeOffset.Now.Date.AddDays(-8000).Date,
                    Gender = Gender.Male,
                    Status = Status.Enabled,
                    ConfirmationStatus = ConfirmationStatus.Confirmed,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(player, AppSettings.AdminPassword);

                if (result.Succeeded)
                {
                    string[] roles = { Role.Admin, Role.Manager, Role.Developer, Role.Player };
                    await UserManager.AddToRolesAsync(player, roles);
                }

                await SaveAndDetachAsync();
            }
        }

        private async Task Seed1_PlayersAsync()
        {
            if (await UserManager.FindByEmailAsync(DataSeederConstants.EmailBernard) == null)
            {
                var player = new Player("Bernard", "Bernardus", DataSeederConstants.EmailBernard)
                {
                    DateOfBirth = DateTimeOffset.Now.Date.AddDays(-7000).Date,
                    Gender = Gender.Male,
                    Status = Status.Enabled,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(player, "P@ssw0rd1!");

                if (result.Succeeded)
                {
                    string[] roles = { Role.Manager, Role.Player };
                    var rolesResult = await UserManager.AddToRolesAsync(player, roles);
                    if (rolesResult.Succeeded)
                    {
                        Logger.LogInformation("Seeding Bernard was succesfull");
                    }
                }

                await SaveAndDetachAsync();
            }

            if (await UserManager.FindByEmailAsync(DataSeederConstants.EmailCasius) == null)
            {
                var player = new Player("Casius", "Clay", DataSeederConstants.EmailCasius)
                {
                    DateOfBirth = DateTimeOffset.Now.Date.AddDays(-10000).Date,
                    Gender = Gender.Male,
                    Status = Status.Enabled,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(player, "P@ssw0rd1!");

                if (result.Succeeded)
                {
                    string[] roles = { Role.Manager, Role.Player };
                    var rolesResult = await UserManager.AddToRolesAsync(player, roles);
                    if (rolesResult.Succeeded)
                    {
                        Logger.LogInformation("Seeding Casius was succesfull");
                    }
                }

                await SaveAndDetachAsync();
            }

            if (await UserManager.FindByEmailAsync(DataSeederConstants.EmailDirk) == null)
            {
                var player = new Player("Donald", "Duck", DataSeederConstants.EmailDirk)
                {
                    DateOfBirth = DateTimeOffset.Now.Date.AddDays(-8000).Date,
                    Gender = Gender.Male,
                    Status = Status.Enabled,
                    EmailConfirmed = true,
                };

                var result = await UserManager.CreateAsync(player, "P@ssw0rd1!");

                if (result.Succeeded)
                {
                    string[] roles = { Role.Manager, Role.Player };
                    var rolesResult = await UserManager.AddToRolesAsync(player, roles);
                    if (rolesResult.Succeeded)
                    {
                        Logger.LogInformation("Seeding Donald was succesfull");
                    }
                }

                await SaveAndDetachAsync();
            }

            await SetPlayersLocalAsync();
        }

        /// <summary>
        /// Deletes users, taking into account the cascades of self-referencing entities.
        /// </summary>
        private async Task DeleteUsers()
        {
            await DeleteAddressesAsync();
            await DeleteFriendshipsAsync();
            await DeleteFollowingsAsync();
            await DeleteMatchesAsync();

            var roles = await Context.UserRoles.ToListAsync();
            Context.UserRoles.RemoveRange(roles);

            var players = await UserManager.Users.ToListAsync();

            foreach (var player in players)
            {
                await UserManager.DeleteAsync(player);
            }

            await SaveAndDetachAsync();
        }

        private async Task SetPlayersLocalAsync()
        {
            UserB = await UserManager.FindByEmailAsync(DataSeederConstants.EmailBernard);
            UserC = await UserManager.FindByEmailAsync(DataSeederConstants.EmailCasius);
            UserD = await UserManager.FindByEmailAsync(DataSeederConstants.EmailDirk);

            await SaveAndDetachAsync();
        }

        private async Task Seed2_AddressesAsync(bool reset)
        {
            if (reset) await DeleteAddressesAsync();
            if (await Context.Addresses.AnyAsync()) return;

            var adresses = new List<Address>
            {
                new Address(
                    "Dirk", "Donders", null, "Netherlands", "Noord-Holland",
                    "Amsterdam", "Amazingstreet 10-1", "1000AA", UserD.Id),
                new Address(
                    "Dirk", "Donders", null, "Netherlands", "Noord-Holland",
                    "Amsterdam", "Boringstreet 10-1", "1000BB", UserD.Id)
            };

            await Context.Addresses.AddRangeAsync(adresses);
            await SaveAndDetachAsync();
        }

        private async Task DeleteAddressesAsync()
        {
            var addresses = await Context.Addresses.ToListAsync();
            Context.Addresses.RemoveRange(addresses);
            await SaveAndDetachAsync();
        }

        private async Task Seed3_FriendshipsAsync(bool reset)
        {
            if (reset) await DeleteFriendshipsAsync();
            if (await Context.Friendships.AnyAsync()) return;

            var friendships = new List<Friendship>
            {
                new Friendship(UserB.Id, UserC.Id),
                new Friendship(UserB.Id, UserD.Id),
                new Friendship(UserC.Id, UserB.Id),
            };

            Context.Friendships.AddRange(friendships);
            await SaveAndDetachAsync();
        }

        private async Task DeleteFriendshipsAsync()
        {
            var friendships = await Context.Friendships.ToListAsync();
            Context.Friendships.RemoveRange(friendships);
            await SaveAndDetachAsync();
        }

        private async Task Seed4_FollowingAsync(bool reset)
        {
            if (reset) await DeleteFollowingsAsync();
            if (await Context.Followings.AnyAsync()) return;

            var followings = new List<Following>
            {
                new Following(UserB.Id, UserC.Id),
                new Following(UserB.Id, UserD.Id),
                new Following(UserC.Id, UserB.Id),
            };

            Context.Followings.AddRange(followings);
            await SaveAndDetachAsync();
        }

        private async Task DeleteFollowingsAsync()
        {
            var followings = await Context.Followings.ToListAsync();
            Context.Followings.RemoveRange(followings);
            await SaveAndDetachAsync();
        }

        private async Task Seed5_ClubsAsync(bool reset)
        {
            if (reset) await DeleteClubsAsync();
            if (!await Context.Clubs.AnyAsync())
            {
                var clubs = new List<Club>
                {
                    new Club("Amstelpark"),
                    new Club("Joy Jaagpad"),
                };
                Context.Clubs.AddRange(clubs);

                await SaveAndDetachAsync();
            }
            await SetClubsLocalAsync();
        }

        private async Task DeleteClubsAsync()
        {
            var clubs = await Context.Clubs.ToListAsync();
            Context.Clubs.RemoveRange(clubs);
            await SaveAndDetachAsync();
        }

        private async Task SetClubsLocalAsync()
        {
            ClubAms = await Context.Clubs.FirstOrDefaultAsync(c => c.Name.Contains("park"));
            ClubJoy = await Context.Clubs.FirstOrDefaultAsync(c => c.Name.Contains("gpad"));
        }

        private async Task Seed6_SurfacesAsync(bool reset)
        {
            if (reset) await DeleteSurfacesAsync();
            if (!await Context.Surfaces.AnyAsync())
            {
                var surfaces = new List<Surface>
                {
                    new Surface(SurfaceConstants.Clay),
                    new Surface(SurfaceConstants.HardCourt),
                    new Surface(SurfaceConstants.ArtificialGrass),
                    new Surface(SurfaceConstants.Carpet),
                    new Surface(SurfaceConstants.Grass),
                };
                Context.Surfaces.AddRange(surfaces);
                await SaveAndDetachAsync();
            }
            await SetSurfacesLocalAsync();
        }

        private async Task DeleteSurfacesAsync()
        {
            var surfaces = await Context.Surfaces.ToListAsync();
            Context.Surfaces.RemoveRange(surfaces);
            await SaveAndDetachAsync();
        }

        private async Task SetSurfacesLocalAsync()
        {
            SurfaceClay = await Context.Surfaces.FirstOrDefaultAsync(x => x.Name.Contains(SurfaceConstants.Clay));
            SurfaceHard = await Context.Surfaces.FirstOrDefaultAsync(x => x.Name.Contains(SurfaceConstants.HardCourt));
            SurfaceGraz = await Context.Surfaces.FirstOrDefaultAsync(x => x.Name.Contains(SurfaceConstants.Grass));
        }

        private async Task Seed7_SurfaceClubJoinsAsync(bool reset)
        {
            if (reset) await DeleteSurfaceClubJoinsAsync();
            if (await Context.SurfaceClubJoins.AnyAsync()) return;

            var surfaceClubJoins = new List<SurfaceClubJoin>
            {
                new SurfaceClubJoin(ClubAms.Id, SurfaceClay.Id),
                new SurfaceClubJoin(ClubAms.Id, SurfaceHard.Id),
                new SurfaceClubJoin(ClubJoy.Id, SurfaceClay.Id)
            };

            Context.SurfaceClubJoins.AddRange(surfaceClubJoins);
            await SaveAndDetachAsync();
        }

        private async Task DeleteSurfaceClubJoinsAsync()
        {
            var surfaceClubJoins = await Context.SurfaceClubJoins.ToListAsync();
            Context.SurfaceClubJoins.RemoveRange(surfaceClubJoins);
            await SaveAndDetachAsync();
        }

        private async Task Seed8_PlayerClubsAsync(bool reset)
        {
            if (reset) await DeletePlayerClubsAsync();
            if (await Context.PlayerClubs.AnyAsync()) return;

            var playerClubs = new List<PlayerClub>
            {
                new PlayerClub(UserB.Id, ClubAms.Id),
                new PlayerClub(UserC.Id, ClubJoy.Id),
                new PlayerClub(UserD.Id, ClubAms.Id),
                new PlayerClub(UserD.Id, ClubJoy.Id)
            };

            Context.PlayerClubs.AddRange(playerClubs);
            await SaveAndDetachAsync();
        }

        private async Task DeletePlayerClubsAsync()
        {
            var playerClubs = await Context.PlayerClubs.ToListAsync();
            Context.PlayerClubs.RemoveRange(playerClubs);
            await SaveAndDetachAsync();
        }

        private async Task Seed9_MatchesAsync(bool reset)
        {
            if (reset) await DeleteMatchesAsync();
            if (await Context.Matches.AnyAsync()) return;

            var scoreBC1 = new List<Set>()
            {
                new Set(6, 4),
                new Set(6, 4)
            };

            var scoreBC2 = new List<Set>()
            {
                new Set(7, 5),
                new Set(7, 6, 7, 5)
            };

            var scoreBD1 = new List<Set>()
            {
                new Set(4, 6),
                new Set(7, 6, 7, 2),
                new Set(6, 3)
            };

            var scoreCD1 = new List<Set>()
            {
                new Set(6, 2),
                new Set(6, 3)
            };

            var scoreCD2 = new List<Set>()
            {
                new Set(4, 6),
                new Set(6, 3),
                new Set(6, 0)
            };

            var matches = new List<Match>()
            {
                new Match(UserB.Id, UserC.Id, SurfaceHard, ClubAms, scoreBC1,
                new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(87)), MatchWinner.PlayerOne, MatchFormat.BestOf3),
                new Match(UserB.Id, UserC.Id, SurfaceClay, ClubAms, scoreBC2,
                new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(88)), MatchWinner.PlayerOne, MatchFormat.BestOf3),
                new Match(UserB.Id, UserD.Id, SurfaceClay, ClubJoy, scoreBD1,
                new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(89)), MatchWinner.PlayerOne, MatchFormat.BestOf3),
                new Match(UserC.Id, UserD.Id, SurfaceGraz, ClubJoy, scoreCD1,
                new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(90)), MatchWinner.PlayerOne, MatchFormat.BestOf3),
                new Match(UserC.Id, UserD.Id, SurfaceClay, ClubJoy, scoreCD2,
                new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(91)), MatchWinner.PlayerOne, MatchFormat.BestOf3),
            };

            Context.Matches.AddRange(matches);
            await Context.SaveChangesAsync();
            await SaveAndDetachAsync();
        }

        private async Task Seed10_EloRankedHistoryAsync()
        {
            var eloRankedOne = await Context.EloRanked.FirstOrDefaultAsync(x => x.PlayerId == UserB.Id);

            if (eloRankedOne.History.Count > 0) return;

            var eloRankedTwo = await Context.EloRanked.FirstOrDefaultAsync(x => x.PlayerId == UserC.Id);
            var match = await Context.Matches.FirstOrDefaultAsync(x => x.PlayerOneId == UserB.Id && x.PlayerTwoId == UserC.Id);
            EloCalculator.UpdateElosForMatch(eloRankedOne, eloRankedTwo, match);
            await SaveAndDetachAsync();
        }

        private async Task Seed11_EloCasualHistoryAsync()
        {
            var eloCasualOne = await Context.EloCasual.FirstOrDefaultAsync(x => x.PlayerId == UserB.Id);

            if (eloCasualOne.History.Count > 0) return;

            var eloCasualTwo = await Context.EloCasual.FirstOrDefaultAsync(x => x.PlayerId == UserC.Id);
            var match = await Context.Matches.FirstOrDefaultAsync(x => x.PlayerOneId == UserB.Id && x.PlayerTwoId == UserC.Id);
            EloCalculator.UpdateElosForMatch(eloCasualOne, eloCasualTwo, match);
            await SaveAndDetachAsync();
        }

        private async Task DeleteMatchesAsync()
        {
            var matches = await Context.Matches.ToListAsync();
            Context.Matches.RemoveRange(matches);
            await SaveAndDetachAsync();
        }

        private async Task SaveAndDetachAsync()
        {
            await Context.SaveChangesAsync();
            //Context.DetachAllEntities();
        }
    }
}
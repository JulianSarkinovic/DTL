using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DikkeTennisLijst.Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<Player>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        // Player-related
        public DbSet<Player> Players { get; set; }

        public DbSet<EloRanked> EloRanked { get; set; }
        public DbSet<EloCasual> EloCasual { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // Many-to-many-tables
        public DbSet<Match> Matches { get; set; }

        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<PlayerClub> PlayerClubs { get; set; }
        public DbSet<SurfaceClubJoin> SurfaceClubJoins { get; set; }

        // Other
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Surface> Surfaces { get; set; }
        public DbSet<Address> Addresses { get; set; }

        [Core.Shared.Attributes.Todo("Contains todo's")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            // Todo: check if these are populated, as likely, we need further key explication.
            modelBuilder.Entity<EloRanked>().OwnsMany(p => p.History);
            modelBuilder.Entity<EloCasual>().OwnsMany(p => p.History);

            // Mapping of Match
            // As it is a self referencing relationship, cascade delete cannot be used.
            // Join entities should be deleted manually before the principal entity is deleted.
            modelBuilder.Entity<Match>().OwnsMany(p => p.Sets);
            modelBuilder.Entity<Match>().OwnsOne(p => p.Duration);

            modelBuilder
                .Entity<Match>()
                .HasOne(x => x.PlayerOne)
                .WithMany()
                .HasForeignKey(x => x.PlayerOneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Match>()
                .HasOne(x => x.PlayerOnePartner)
                .WithMany()
                .HasForeignKey(x => x.PlayerOnePartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Match>()
                .HasOne(x => x.PlayerTwoPartner)
                .WithMany()
                .HasForeignKey(x => x.PlayerTwoPartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Match>()
                .HasOne(x => x.PlayerTwo)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.PlayerTwoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mapping of Friendships
            // As it is a self referencing relationship, cascade delete cannot be used.
            // Join entities should be deleted manually before the principal entity is deleted.
            modelBuilder
                .Entity<Friendship>()
                .HasOne(x => x.Friended)
                .WithMany()
                .HasForeignKey(x => x.FriendedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Friendship>()
                .HasOne(x => x.Friender)
                .WithMany(x => x.Friendships)
                .HasForeignKey(x => x.FrienderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mapping of Followings
            // As it is a self referencing relationship, cascade delete cannot be used.
            // Join entities should be deleted manually before the principal entity is deleted.
            modelBuilder
                .Entity<Following>()
                .HasOne(x => x.Followed)
                .WithMany()
                .HasForeignKey(x => x.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Following>()
                .HasOne(x => x.Follower)
                .WithMany(x => x.Followings)
                .HasForeignKey(x => x.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            UpdateTimeStamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimeStamps()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.Entity is ITimestamps && e.State == EntityState.Modified))
            {
                entry.Property(nameof(ITimestamps.UpdatedAt)).CurrentValue = DateTimeOffset.Now;
            }
        }
    }
}
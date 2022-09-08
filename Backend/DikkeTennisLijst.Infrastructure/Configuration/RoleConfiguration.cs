using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DikkeTennisLijst.Infrastructure.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = Role.Player,
                    NormalizedName = Role.Player.ToUpper()
                },
                new IdentityRole
                {
                    Name = Role.Manager,
                    NormalizedName = Role.Manager.ToUpper()
                },
                new IdentityRole
                {
                    Name = Role.Admin,
                    NormalizedName = Role.Admin.ToUpper()
                },
                new IdentityRole
                {
                    Name = Role.Developer,
                    NormalizedName = Role.Developer.ToUpper()
                });
        }
    }
}
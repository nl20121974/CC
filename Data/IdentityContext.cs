using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CC.Identity
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(entity => entity.ToTable(name: "IdentityUser"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "IdentityRole"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("IdentityUserRole"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("IdentityUserClaim"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("IdentityUserLogin"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("IdentityRoleClaim"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("IdentityUserToken"));
        }
    }
}
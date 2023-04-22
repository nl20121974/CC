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
            builder.Entity<IdentityUser>(entity => entity.ToTable(name: "User"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Role"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRole"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaim"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogin"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaim"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserToken"));
        }
    }
}
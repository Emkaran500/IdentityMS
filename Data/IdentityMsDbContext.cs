using IdentityMS.Configurations;
using IdentityMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMS.Data;


public class IdentityMsDbContext : IdentityDbContext<User>
{
    public IdentityMsDbContext(DbContextOptions<IdentityMsDbContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
    }
}
using CleanArch.Application.Common.Interfaces;
using CleanArch.Shared.Authorization;
using Light.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Identity;

public class AppIdentityDbContext(
    ICurrentUser currentUser,
    IDateTime clock,
    DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext(options)
{
    protected override string CurrentUserId => currentUser.UserId ?? base.CurrentUserId;

    protected override bool SoftDelete => false;

    protected override DateTimeOffset Time => clock.Now;

    public virtual DbSet<Models.Notification> Notifications => Set<Models.Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Models.Notification>().ToTable(name: "Notifications", "System");
    }
}
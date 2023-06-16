using Chatty.Server.Data.Models;
using Chatty.Server.Data.Models.Base;
using Chatty.Server.Infrastructure.Services;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatty.Server.Data;

public class AppDbContext : IdentityDbContext<User>
{
    private readonly ICurrentUserService currentUser;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUserService currentUser)
        : base(options)
        => this.currentUser = currentUser;

    public DbSet<Chat> Chats { get; init; }

    public DbSet<Message> Messages { get; init; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ApplyAuditInformation();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken)
    {
        this.ApplyAuditInformation();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Chat>()
            .HasQueryFilter(c => !c.IsDeleted)
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Chat>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Chats);

        builder.Entity<Message>()
            .HasQueryFilter(m => !m.IsDeleted)
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
    }


    private void ApplyAuditInformation()
        => this.ChangeTracker
            .Entries()
            .ToList()
            .ForEach(entry =>
            {
                var username = this.currentUser.GetUsername();

                if (entry.Entity is IDeletableEntity deletableEntity)
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        deletableEntity.DeletedOn = DateTime.UtcNow;
                        deletableEntity.DeletedBy = username;
                        deletableEntity.IsDeleted = true;

                        entry.State = EntityState.Modified;

                        return;
                    }
                }

                if (entry.Entity is IEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = DateTime.UtcNow;
                        entity.CreatedBy = username;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.ModifiedOn = DateTime.UtcNow;
                        entity.ModifiedBy = username;
                    }
                }
            });
}

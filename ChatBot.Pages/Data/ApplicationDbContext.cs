using ChatBot.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Pages.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ChatUser> ChatUsers { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder
          .Entity<Message>()
          .HasOne(message => message.Sender)
          .WithMany(user => user.Messages)
          .HasForeignKey(user => user.UserId);

      builder
          .Entity<ChatUser>()
          .HasIndex(e => e.DisplayName)
          .IsUnique();
    }
  }
}
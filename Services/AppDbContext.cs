using Microsoft.EntityFrameworkCore;
using SocialChatTool.Models;

namespace SocialChatTool.Services;

/// <summary>
/// Entity Framework DbContext for MySQL database
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SocialPage> SocialPages { get; set; }
    public DbSet<SocialConversation> SocialConversations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing table names in database
        modelBuilder.Entity<SocialPage>().ToTable("social_page");
        modelBuilder.Entity<SocialConversation>().ToTable("social_conversation");

        // Configure primary keys
        modelBuilder.Entity<SocialPage>().HasKey(p => p.Id);
        modelBuilder.Entity<SocialConversation>().HasKey(c => c.Id);
    }
}

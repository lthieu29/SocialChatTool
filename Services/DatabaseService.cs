using Microsoft.EntityFrameworkCore;
using SocialChatTool.Models;

namespace SocialChatTool.Services;

/// <summary>
/// Service for database operations - supports dynamic connection string
/// </summary>
public class DatabaseService
{
    private string? _connectionString;

    public DatabaseService()
    {
    }

    /// <summary>
    /// Set connection string for company database
    /// </summary>
    public void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Check if connection string is set
    /// </summary>
    public bool IsConnected => !string.IsNullOrEmpty(_connectionString);

    private AppDbContext CreateContext()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("Connection string not set. Please select a company first.");
        
        return new AppDbContext(_connectionString);
    }

    /// <summary>
    /// Get all distinct pages from social_page table
    /// Group by SocialType, PageID, Name
    /// </summary>
    public async Task<List<SocialPage>> GetPagesAsync()
    {
        await using var context = CreateContext();
        return await context.SocialPages
            .GroupBy(p => new { p.SocialType, p.PageID, p.Name })
            .Select(g => g.First())
            .OrderBy(p => p.SocialType)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get all conversations for a specific page and social type
    /// </summary>
    public async Task<List<SocialConversation>> GetConversationsAsync(string pageId, int socialType)
    {
        await using var context = CreateContext();
        return await context.SocialConversations
            .Where(c => c.PageID == pageId && c.SocialType == socialType)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using SocialChatTool.Models;

namespace SocialChatTool.Services;

/// <summary>
/// Service for database operations
/// </summary>
public class DatabaseService
{
    private readonly AppDbContext _context;

    public DatabaseService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all distinct pages from social_page table
    /// Group by SocialType, PageID, Name
    /// </summary>
    public async Task<List<SocialPage>> GetPagesAsync()
    {
        return await _context.SocialPages
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
        return await _context.SocialConversations
            .Where(c => c.PageID == pageId && c.SocialType == socialType)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}

namespace SocialChatTool.Models;

/// <summary>
/// Represents a conversation in a social media platform
/// </summary>
public class SocialConversation
{
    public int Id { get; set; }
    
    public string ConversationID { get; set; } = string.Empty;
    
    public string PageID { get; set; } = string.Empty;
    
    public int SocialType { get; set; }
    
    public string Name { get; set; } = string.Empty;
}

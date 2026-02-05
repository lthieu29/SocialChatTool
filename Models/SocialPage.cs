namespace SocialChatTool.Models;

/// <summary>
/// Represents a social media page connection
/// </summary>
public class SocialPage
{
    public int Id { get; set; }
    
    public string PageID { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 1=ZaloOA, 2=Facebook, 3=ZaloPersonal, 4=ShopeeChat, 5=TiktokBusiness
    /// </summary>
    public int SocialType { get; set; }

    public string DisplayText => $"{Name} ({GetSocialTypeName()})";

    private string GetSocialTypeName()
    {
        return SocialType switch
        {
            1 => "Zalo OA",
            2 => "Facebook",
            3 => "Zalo Personal",
            4 => "Shopee Chat",
            5 => "Tiktok Business",
            _ => "Unknown"
        };
    }
}

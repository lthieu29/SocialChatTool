namespace SocialChatTool.Services;

/// <summary>
/// Service for sending messages to different social platforms
/// </summary>
public class MessageService
{
    /// <summary>
    /// Send message to social platform - Empty implementation for user to fill
    /// </summary>
    /// <param name="socialType">1=ZaloOA, 2=Facebook, 3=ZaloPersonal, 4=ShopeeChat, 5=TiktokBusiness</param>
    /// <param name="pageId">Page identifier</param>
    /// <param name="conversationId">Conversation identifier</param>
    /// <param name="message">Message text to send</param>
    public async Task SendMessage(int socialType, string pageId, string conversationId, string message)
    {
        switch (socialType)
        {
            case 1: // Zalo OA
                await SendZaloOAMessage(pageId, conversationId, message);
                break;
                
            case 2: // Facebook
                await SendFacebookMessage(pageId, conversationId, message);
                break;
                
            case 3: // Zalo Personal
                await SendZaloPersonalMessage(pageId, conversationId, message);
                break;
                
            case 4: // Shopee Chat
                await SendShopeeChatMessage(pageId, conversationId, message);
                break;
                
            case 5: // Tiktok Business
                await SendTiktokBusinessMessage(pageId, conversationId, message);
                break;
                
            default:
                throw new NotSupportedException($"SocialType {socialType} không được hỗ trợ");
        }
    }
}

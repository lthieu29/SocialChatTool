using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SocialChatTool.Models;
using SocialChatTool.Services;

namespace SocialChatTool.ViewModels;

/// <summary>
/// ViewModel for MainWindow - Handles UI logic and data binding
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly MessageService _messageService;

    public MainViewModel(DatabaseService databaseService, MessageService messageService)
    {
        _databaseService = databaseService;
        _messageService = messageService;
        
        // Initialize command
        LoadPagesCommand = new AsyncRelayCommand(LoadPagesAsync);
        SendMessageCommand = new AsyncRelayCommand(SendMessageAsync, CanSendMessage);
    }

    // Collections
    [ObservableProperty]
    private ObservableCollection<SocialPage> _pages = new();

    [ObservableProperty]
    private ObservableCollection<SocialConversation> _conversations = new();

    // Selected items
    [ObservableProperty]
    private SocialPage? _selectedPage;

    [ObservableProperty]
    private SocialConversation? _selectedConversation;

    // Message input
    [ObservableProperty]
    private string _messageText = string.Empty;

    // Commands
    public IAsyncRelayCommand LoadPagesCommand { get; }
    public IAsyncRelayCommand SendMessageCommand { get; }

    /// <summary>
    /// Load all pages from database
    /// </summary>
    private async Task LoadPagesAsync()
    {
        try
        {
            var pages = await _databaseService.GetPagesAsync();
            Pages.Clear();
            foreach (var page in pages)
            {
                Pages.Add(page);
            }
        }
        catch (Exception ex)
        {
            // TODO: Show error message to user
            System.Diagnostics.Debug.WriteLine($"Error loading pages: {ex.Message}");
        }
    }

    /// <summary>
    /// When page selection changes, load conversations for that page
    /// </summary>
    partial void OnSelectedPageChanged(SocialPage? value)
    {
        if (value != null)
        {
            _ = LoadConversationsAsync(value);
        }
        else
        {
            Conversations.Clear();
        }
    }

    /// <summary>
    /// Load conversations for selected page
    /// </summary>
    private async Task LoadConversationsAsync(SocialPage page)
    {
        try
        {
            var conversations = await _databaseService.GetConversationsAsync(page.PageID, page.SocialType);
            Conversations.Clear();
            foreach (var conversation in conversations)
            {
                Conversations.Add(conversation);
            }
        }
        catch (Exception ex)
        {
            // TODO: Show error message to user
            System.Diagnostics.Debug.WriteLine($"Error loading conversations: {ex.Message}");
        }
    }

    /// <summary>
    /// Send message to selected conversation
    /// </summary>
    private async Task SendMessageAsync()
    {
        if (SelectedPage == null || SelectedConversation == null || string.IsNullOrWhiteSpace(MessageText))
            return;

        try
        {
            await _messageService.SendMessage(
                SelectedPage.SocialType,
                SelectedPage.PageID,
                SelectedConversation.ConversationID,
                MessageText
            );

            // Clear message after sending
            MessageText = string.Empty;
            
            // TODO: Show success notification
        }
        catch (Exception ex)
        {
            // TODO: Show error message to user
            System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    /// <summary>
    /// Check if message can be sent
    /// </summary>
    private bool CanSendMessage()
    {
        return SelectedPage != null 
            && SelectedConversation != null 
            && !string.IsNullOrWhiteSpace(MessageText);
    }

    /// <summary>
    /// Update send command state when properties change
    /// </summary>
    partial void OnSelectedConversationChanged(SocialConversation? value)
    {
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    partial void OnMessageTextChanged(string value)
    {
        SendMessageCommand.NotifyCanExecuteChanged();
    }
}

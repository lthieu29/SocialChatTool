using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
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
    private readonly CompanyService _companyService;
    private readonly DatabaseService _databaseService;
    private readonly MessageService _messageService;

    public MainViewModel(CompanyService companyService, DatabaseService databaseService, MessageService messageService)
    {
        _companyService = companyService;
        _databaseService = databaseService;
        _messageService = messageService;
        
        // Initialize commands
        LoadCompaniesCommand = new AsyncRelayCommand(LoadCompaniesAsync);
        SelectCompanyCommand = new AsyncRelayCommand(SelectCompanyAsync, () => SelectedCompany != null);
        LoadPagesCommand = new AsyncRelayCommand(LoadPagesAsync);
        SendMessageCommand = new AsyncRelayCommand(SendMessageAsync, CanSendMessage);

        // Setup filtered view for companies
        _companiesView = CollectionViewSource.GetDefaultView(_allCompanies);
        _companiesView.Filter = FilterCompanies;
    }

    #region Company Selection

    private readonly ObservableCollection<Company> _allCompanies = new();
    private readonly ICollectionView _companiesView;

    public ICollectionView CompaniesView => _companiesView;

    [ObservableProperty]
    private string _companySearchText = string.Empty;

    [ObservableProperty]
    private Company? _selectedCompany;

    [ObservableProperty]
    private bool _isCompanySelected;

    [ObservableProperty]
    private string _selectedCompanyInfo = string.Empty;

    public IAsyncRelayCommand LoadCompaniesCommand { get; }
    public IAsyncRelayCommand SelectCompanyCommand { get; }

    partial void OnCompanySearchTextChanged(string value)
    {
        _companiesView.Refresh();
    }

    partial void OnSelectedCompanyChanged(Company? value)
    {
        SelectCompanyCommand.NotifyCanExecuteChanged();
    }

    private bool FilterCompanies(object obj)
    {
        if (obj is not Company company) return false;
        if (string.IsNullOrWhiteSpace(CompanySearchText)) return true;

        var search = CompanySearchText.ToLower();
        return company.CompanyCode.ToLower().Contains(search) ||
               company.CompanyName.ToLower().Contains(search);
    }

    private async Task LoadCompaniesAsync()
    {
        try
        {
            var companies = await _companyService.GetCompaniesAsync();
            _allCompanies.Clear();
            foreach (var company in companies)
            {
                _allCompanies.Add(company);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading companies: {ex.Message}");
        }
    }

    private async Task SelectCompanyAsync()
    {
        if (SelectedCompany == null) return;

        try
        {
            var connectionString = SelectedCompany.BuildConnectionString();
            _databaseService.SetConnectionString(connectionString);
            
            IsCompanySelected = true;
            SelectedCompanyInfo = $"Đã kết nối: {SelectedCompany.CompanyName}";

            // Auto load pages after company selection
            await LoadPagesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error connecting to company: {ex.Message}");
            IsCompanySelected = false;
            SelectedCompanyInfo = "Lỗi kết nối!";
        }
    }

    #endregion

    #region Pages & Conversations

    [ObservableProperty]
    private ObservableCollection<SocialPage> _pages = new();

    [ObservableProperty]
    private ObservableCollection<SocialConversation> _conversations = new();

    [ObservableProperty]
    private SocialPage? _selectedPage;

    [ObservableProperty]
    private SocialConversation? _selectedConversation;

    [ObservableProperty]
    private string _messageText = string.Empty;

    public IAsyncRelayCommand LoadPagesCommand { get; }
    public IAsyncRelayCommand SendMessageCommand { get; }

    private async Task LoadPagesAsync()
    {
        if (!_databaseService.IsConnected) return;

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
            System.Diagnostics.Debug.WriteLine($"Error loading pages: {ex.Message}");
        }
    }

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
            System.Diagnostics.Debug.WriteLine($"Error loading conversations: {ex.Message}");
        }
    }

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

            MessageText = string.Empty;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    private bool CanSendMessage()
    {
        return SelectedPage != null 
            && SelectedConversation != null 
            && !string.IsNullOrWhiteSpace(MessageText);
    }

    partial void OnSelectedConversationChanged(SocialConversation? value)
    {
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    partial void OnMessageTextChanged(string value)
    {
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    #endregion
}

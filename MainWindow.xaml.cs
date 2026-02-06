using System.Windows;
using SocialChatTool.ViewModels;

namespace SocialChatTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        // Load companies when window is loaded
        Loaded += async (s, e) => await _viewModel.LoadCompaniesCommand.ExecuteAsync(null);
    }
}
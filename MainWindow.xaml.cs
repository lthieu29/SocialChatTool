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
        
        // Load pages when window is loaded
        Loaded += async (s, e) => await _viewModel.LoadPagesCommand.ExecuteAsync(null);
    }
}
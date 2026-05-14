using Microsoft.UI.Xaml;
using PracticeWork10M4.ViewModels;

namespace PracticeWork10M4;

public partial class App : Application
{
    public static MainViewModel ViewModel { get; } = new MainViewModel();

    private Window? m_window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }
}
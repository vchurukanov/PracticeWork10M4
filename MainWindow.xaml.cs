using Microsoft.UI.Xaml;
using PracticeWork10M4.ViewModels;

namespace PracticeWork10M4;

public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; } = App.ViewModel;

    public MainWindow()
    {
        this.InitializeComponent();
    }

    private async void ReloadButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveObjectAsync();
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ClearForm();
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.DeleteSelectedObjectAsync();
    }
}
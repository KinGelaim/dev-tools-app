using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolsApp.Models;
using System.Collections.ObjectModel;

namespace DevToolsApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isPaneOpen;

    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private NavigationItem? _selectedTool;

    public ObservableCollection<NavigationItem> Tools { get; } = [];

    public MainViewModel()
    {
        Tools.Add(new NavigationItem("Base64 Converter", new Base64ViewModel()));
        Tools.Add(new NavigationItem("UUID / GUID Generator", new GuidGeneratorViewModel()));
        Tools.Add(new NavigationItem("JSON Formatter / Validator", new JsonFormatterViewModel()));

        SelectedTool = Tools[0];
    }

    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    partial void OnSelectedToolChanged(NavigationItem? value)
    {
        if (value is not null)
        {
            CurrentPage = value.ViewModel;
            IsPaneOpen = false;
        }
    }
}
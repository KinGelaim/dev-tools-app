using CommunityToolkit.Mvvm.ComponentModel;
using DevToolsApp.Models;
using System.Collections.ObjectModel;

namespace DevToolsApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    [ObservableProperty]
    private NavigationItem? _selectedItem;

    public ObservableCollection<NavigationItem> Tools { get; } = [];

    public MainViewModel()
    {
        var base64Tool = new NavigationItem("Base64 Converter", new Base64ViewModel());
        Tools.Add(base64Tool);

        SelectedItem = base64Tool;
        _currentPage = base64Tool.ViewModel;
    }

    partial void OnSelectedItemChanged(NavigationItem? value)
    {
        if (value != null)
        {
            CurrentPage = value.ViewModel;
        }
    }
}
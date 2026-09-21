using DevToolsApp.ViewModels;

namespace DevToolsApp.Models;

public record NavigationItem(
    string Title,
    ViewModelBase ViewModel);
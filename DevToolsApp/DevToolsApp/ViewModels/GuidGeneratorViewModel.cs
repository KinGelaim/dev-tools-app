using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DevToolsApp.ViewModels;

public partial class GuidGeneratorViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _output = string.Empty;

    [ObservableProperty]
    private int _count = 1;

    [ObservableProperty]
    private bool _useHyphens = true;

    [ObservableProperty]
    private bool _isUppercase = false;

    [ObservableProperty]
    private int _bracketFormatIndex = 0;

    public GuidGeneratorViewModel()
    {
        Generate();
    }

    [RelayCommand]
    private void Generate()
    {
        var safeCount = Math.Clamp(Count, 1, 1000);
        var sb = new StringBuilder();

        for (var i = 0; i < safeCount; i++)
        {
            var guid = Guid.NewGuid();
            sb.AppendLine(FormatGuid(guid));
        }

        Output = sb.ToString().TrimEnd();
    }

    [RelayCommand]
    private async Task CopyAsync()
    {
        if (string.IsNullOrWhiteSpace(Output))
        {
            return;
        }

        var clipboard = GetClipboard();
        if (clipboard is not null)
        {
            await clipboard.SetTextAsync(Output);
        }
    }

    private static Avalonia.Input.Platform.IClipboard? GetClipboard()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow?.Clipboard;
        }
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(singleView.MainView);
            return topLevel?.Clipboard;
        }
        return null;
    }

    private string FormatGuid(Guid guid)
    {
        var formatSpecifier = UseHyphens ? "D" : "N";
        var result = guid.ToString(formatSpecifier);

        result = IsUppercase
            ? result.ToUpperInvariant()
            : result.ToLowerInvariant();

        return BracketFormatIndex switch
        {
            1 => $"{{{result}}}",
            2 => $"({result})",
            _ => result
        };
    }

    partial void OnUseHyphensChanged(bool value) => Generate();
    partial void OnIsUppercaseChanged(bool value) => Generate();
    partial void OnBracketFormatIndexChanged(int value) => Generate();
}
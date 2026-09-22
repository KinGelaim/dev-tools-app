using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevToolsApp.ViewModels;

public partial class JsonFormatterViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private string _outputText = string.Empty;

    [ObservableProperty]
    private string _copyButtonText = "Копировать";

    [RelayCommand]
    private void FormatJson()
    {
        if (string.IsNullOrEmpty(InputText))
        {
            OutputText = string.Empty;
            return;
        }

        try
        {
            var parsed = JsonDocument.Parse(InputText);
            OutputText = JsonSerializer.Serialize(parsed, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        catch (Exception ex)
        {
            OutputText = $"Ошибка: {ex.Message}";
        }
    }

    [RelayCommand]
    private void MinifyJson()
    {
        if (string.IsNullOrEmpty(InputText))
        {
            OutputText = string.Empty;
            return;
        }

        try
        {
            var parsed = JsonDocument.Parse(InputText);
            OutputText = JsonSerializer.Serialize(parsed, new JsonSerializerOptions 
            { 
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        catch (Exception ex)
        {
            OutputText = $"Ошибка: {ex.Message}";
        }
    }

    [RelayCommand]
    private void Clear()
    {
        InputText = string.Empty;
        OutputText = string.Empty;
    }

    [RelayCommand]
    private async Task PasteInputAsync()
    {
        var clipboard = GetClipboard();
        if (clipboard != null)
        {
            var text = await clipboard.TryGetTextAsync();
            if (!string.IsNullOrEmpty(text))
            {
                InputText = text;
            }
        }
    }

    [RelayCommand]
    private async Task CopyOutputAsync()
    {
        if (string.IsNullOrEmpty(OutputText) || OutputText.StartsWith("Ошибка"))
        {
            return;
        }

        var clipboard = GetClipboard();
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(OutputText);

            CopyButtonText = "Скопировано!";
            await Task.Delay(1500);
            CopyButtonText = "Копировать";
        }
    }

    private IClipboard? GetClipboard()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow?.Clipboard;
        }

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(singleView.MainView);
            return topLevel?.Clipboard;
        }

        return null;
    }
}

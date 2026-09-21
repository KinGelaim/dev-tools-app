using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DevToolsApp.ViewModels;

public partial class Base64ViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private string _outputText = string.Empty;

    [ObservableProperty]
    private string _copyButtonText = "Копировать";

    partial void OnInputTextChanged(string value)
    {
        EncodeToBase64();
    }

    [RelayCommand]
    private void EncodeToBase64()
    {
        if (string.IsNullOrEmpty(InputText))
        {
            OutputText = string.Empty;
            return;
        }

        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(InputText);
            OutputText = Convert.ToBase64String(bytes);
        }
        catch (Exception ex)
        {
            OutputText = $"Ошибка кодирования: {ex.Message}";
        }
    }

    [RelayCommand]
    private void DecodeFromBase64()
    {
        if (string.IsNullOrEmpty(InputText))
        {
            OutputText = string.Empty;
            return;
        }

        try
        {
            byte[] bytes = Convert.FromBase64String(InputText);
            OutputText = Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            OutputText = "Ошибка: Некорректная Base64 строка!";
        }
    }

    [RelayCommand]
    private void Clear()
    {
        InputText = string.Empty;
        OutputText = string.Empty;
    }

    [RelayCommand]
    private void Swap()
    {
        if (string.IsNullOrEmpty(OutputText) || OutputText.StartsWith("Ошибка"))
        {
            return;
        }    

        InputText = OutputText;
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
        return null;
    }
}
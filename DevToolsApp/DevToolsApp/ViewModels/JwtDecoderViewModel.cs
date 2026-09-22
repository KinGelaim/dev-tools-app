using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevToolsApp.ViewModels;

public partial class JwtDecoderViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _jwtInput = string.Empty;

    [ObservableProperty]
    private string _decodedPayload = string.Empty;

    [ObservableProperty]
    private string _copyButtonText = "Копировать";

    [RelayCommand]
    private void DecodePayload()
    {
        if (string.IsNullOrEmpty(JwtInput))
        {
            DecodedPayload = string.Empty;
            return;
        }

        try
        {
            var parts = JwtInput.Split('.');
            
            if (parts.Length != 3)
            {
                DecodedPayload = "Ошибка: Неверный формат JWT. Ожидаются 3 части, разделённые точкой.";
                return;
            }

            var payload = parts[1];

            var padding = (4 - payload.Length % 4) % 4;
            for (int i = 0; i < padding; i++)
            {
                payload += "=";
            }

            payload = payload.Replace('-', '+').Replace('_', '/');

            var bytes = Convert.FromBase64String(payload);
            var decoded = Encoding.UTF8.GetString(bytes);

            try
            {
                var parsed = JsonDocument.Parse(decoded);
                DecodedPayload = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                DecodedPayload = decoded;
            }
        }
        catch
        {
            DecodedPayload = "Ошибка: Не удалось декодировать payload. Проверьте корректность токена.";
        }
    }

    [RelayCommand]
    private async Task CopyOutputAsync()
    {
        if (string.IsNullOrEmpty(DecodedPayload) || DecodedPayload.StartsWith("Ошибка"))
        {
            return;
        }

        var clipboard = GetClipboard();
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(DecodedPayload);

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

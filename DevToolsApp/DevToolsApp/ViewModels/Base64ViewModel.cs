using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text;

namespace DevToolsApp.ViewModels;

public partial class Base64ViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private string _outputText = string.Empty;

    partial void OnInputTextChanged(string value)
    {
        EncodeToBase64();
    }

    public void EncodeToBase64()
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
            OutputText = $"Ошибка: {ex.Message}";
        }
    }

    public void DecodeFromBase64()
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
            OutputText = "Некорректная Base64 строка!";
        }
    }
}
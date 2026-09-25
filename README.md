# DevTools App

Кроссплатформенное приложение-сборник утилит для разработчиков, написанное на **Avalonia UI**. Предоставляет набор инструментов для повседневных задач разработки в едином интерфейсе без необходимости выхода в интернет.

## Возможности

Приложение включает следующие инструменты:

| Инструмент | Описание |
|------------|----------|
| **Base64 конвертер** | Кодирование текста в Base64 и декодирование обратно. Автоматическая обработка при вводе. |
| **GUID генератор** | Генерация от 1 до 1000 уникальных идентификаторов с настройкой формата (регистр, дефисы, скобки). |
| **JSON форматтер** | Красивое форматирование и сжатие (minify) JSON-документов с проверкой синтаксиса. |
| **JWT декодер** | Декодирование payload части JWT токена с автоматическим форматированием JSON. |

## Поддерживаемые платформы

- **Desktop** — Windows
- **Android** — ARM64, x86, x86_64
- **iOS** — шаблон настраивается
- **Browser** — шаблон настраивается

## Архитектура проекта

```
┌───────────────────────────────────────────────────────┐
│                    DevToolsApp                        │
│                    (Основное приложение)              │
│                                                       │
│  ┌──────────────┐  ┌─────────────┐  ┌──────────────┐  │
│  │    Models    │  │ ViewModels  │  │    Views     │  │
│  ├──────────────┤  ├─────────────┤  ├──────────────┤  │
│  │NavigationItem│  │ViewModelBase│  │ MainWindow   │  │
│  └──────────────┘  ├─────────────┤  ├──────────────┤  │
│                    │Base64VM     │  │ MainView     │  │
│                    ├─────────────┤  ├──────────────┤  │
│                    │GuidGenVM    │  │Base64View    │  │
│                    ├─────────────┤  ├──────────────┤  │
│                    │JsonFormatVM │  │GuidGenerator │  │
│                    ├─────────────┤  ├──────────────┤  │
│                    │  JwtDecoder │  │ JsonFormatter│  │
│                    └─────────────┘  │ JwtDecoder   │  │
│                                     └──────────────┘  │
└───────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        │                     │                     │
        ▼                     ▼                     ▼
┌───────────────┐   ┌───────────────┐   ┌───────────────┐
│  DevTools     │   │  DevTools     │   │  DevTools     │
│  App.Desktop  │   │  App.Android  │   │  App.iOS      │
└───────────────┘   └───────────────┘   └───────────────┘
```

### Структура директорий

| Директория | Назначение |
|------------|------------|
| `DevToolsApp/` | Основной код приложения (MVVM, общие ресурсы) |
| `DevToolsApp.Desktop/` | Сборка для desktop-платформ |
| `DevToolsApp.Android/` | Сборка для Android |
| `DevToolsApp.iOS/` | Шаблон сборки для iOS |
| `DevToolsApp.Browser/` | Шаблон сборки для браузера |

### Архитектурный паттерн

Применяется паттерн **MVVM (Model-View-ViewModel)** с использованием:

- **CommunityToolkit.MVMV** — ObservableProperty, RelayCommand
- **ViewLocator** — автоматическое связывание View и ViewModel
- **Dependency Injection** — через конструктор в `App.axaml.cs`

## Компоненты приложения

### Models
- `NavigationItem` — модель элемента навигации

### ViewModels
- `ViewModelBase` — базовый класс для всех ViewModel
- `MainViewModel` — главный экран с навигацией
- `Base64ViewModel` — логика Base64 кодирования/декодирования
- `GuidGeneratorViewModel` — генерация GUID с параметрами
- `JsonFormatterViewModel` — форматирование и минификация JSON
- `JwtDecoderViewModel` — декодирование JWT токенов

### Views
- `MainWindow` — главное окно для Desktop платформ
- `MainView` — главный экран для мобильных платформ
- `Base64View.axaml` — интерфейс Base64 конвертера
- `GuidGeneratorView.axaml` — интерфейс генератора GUID
- `JsonFormatterView.axaml` — интерфейс форматтера JSON
- `JwtDecoderView.axaml` — интерфейс декодера JWT

## Сборка проекта

### Сборка под Desktop

```bash
dotnet publish -c Release .\DevToolsApp.Desktop\DevToolsApp.Desktop.csproj
```

### Сборка под Android

Сборка под все архитектуры:
```bash
dotnet publish -c Release .\DevToolsApp.Android\DevToolsApp.Android.csproj
```

Сборка под конкретную архитектуру:
```bash
dotnet publish -c Release -r android-arm64 .\DevToolsApp.Android\DevToolsApp.Android.csproj
```

Сборка под конкретную архитектуру с подписью релизного APK:
```bash
dotnet publish -c Release -r android-arm64 ^
  -p:AndroidSigningKeyStore="C:\SecretKeys\release.keystore" ^
  -p:AndroidSigningStorePass="RealSuperPassword" ^
  -p:AndroidSigningKeyAlias="mycompany" ^
  -p:AndroidSigningKeyPass="RealSuperPassword" ^
  .\DevToolsApp.Android\DevToolsApp.Android.csproj
```

## Технологический стек

| Компонент | Технология |
|-----------|------------|
| Фреймворк | Avalonia UI 11.x |
| Язык | C# 12 |
| Платформа | .NET 8 / .NET 10 |
| MVVM Toolkit | CommunityToolkit.Mvvm |
| Сериализация | System.Text.Json |

## Требования к развёртыванию

- **Desktop**: .NET Runtime (в зависимости от целевой версии)
- **Android**: Android 5.0 (API 21) и выше

## Лицензия

Проект разработан для внутреннего использования и поставляется по лицензии MIT "как есть" (AS IS), без каких-либо гарантий и обязательств по поддержке. Issues и Pull Requests рассматриваются исключительно по настроению и возможности автора.

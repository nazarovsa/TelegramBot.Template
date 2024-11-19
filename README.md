# Telegram Bot Template

A .NET template for quickly bootstrapping Telegram bots using the [Insight.TelegramBot](https://www.nuget.org/packages/Insight.TelegramBot/) framework.

## Features

- Production-ready project structure
- Multi-language support (en-US, ru-RU)
- Docker support
- Structured logging with Serilog
- Message handling infrastructure
- State machine support for complex flows

## Prerequisites

- .NET 8.0 SDK
- Docker (optional)

## Installation

Install the template from NuGet:

```bash
dotnet new install TelegramBot.Template
```

## Usage 

Create a new bot project:

```bash
dotnet new telegram-bot-template -n YourBotName
```

Configure your bot token:

1. Open `src/YourBotName.Host/appsettings.protected.json`
2. Set your Telegram bot token:
```json
{
  "TelegramBotOptions": {
    "Token": "YOUR_BOT_TOKEN"
  }
}
```

## Project Structure

- `YourBotName.AppServices/` - Business logic and message handlers
  - `Telegram/` - Bot-specific code
    - `Handlers/` - Message and callback handlers
- `YourBotName.Host/` - Application host and configuration
  - `Resources/` - Localization files
  - `Infrastructure/` - Application bootstrapping

## Running the Bot

### Local Development

```bash
cd src/YourBotName.Host
dotnet run
```

### Docker

```bash
docker-compose -f docker-compose.local.yml up -d
```

## Adding New Commands

1. Create a new handler class in `AppServices/Telegram/Handlers/Messages`
2. Implement `IMatchingUpdateHandler<T>` interface
3. Add localization resources in `Host/Resources/Handlers`
4. Register in `ServiceCollectionExtensions.cs` if needed

## State Management

The template includes basic state management for multi-step flows:

1. Define states in `ExampleState.cs`
2. Use `CallbackData<T>` for state transitions
3. Implement handlers for each state

## Localization

Add translations in:
- `Resources/Buttons/` - Button texts
- `Resources/Handlers/` - Handler-specific messages

## Logging

Configured with Serilog:
- Console output for development
- File logging for production
- Structured log format
- Configurable log levels per namespace

## License

MIT
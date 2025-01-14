# Telegram Bot Template

A production-ready .NET template for building scalable Telegram bots using clean architecture principles and modern development practices.

## Features

- **Clean Architecture**: Organized in domain-centric layers (Domain, Application, Infrastructure, Host)
- **State Management**: Built-in support for user state tracking and complex conversation flows
- **Multi-language Support**: Integrated localization system with en-US and ru-RU out of the box
- **Database Integration**: Configured Entity Framework Core with MariaDB support
- **Docker Support**: Ready-to-use Docker and docker-compose configurations
- **Logging**: Structured logging with Serilog, including file and console outputs
- **Dependency Injection**: Well-organized DI setup with Microsoft.Extensions.DependencyInjection
- **Configuration Management**: Environment-based settings with protection of sensitive data
- **Testing Ready**: Project structure supports unit and integration testing
- **Error Handling**: Global exception handling and logging setup

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose (optional)
- MariaDB (if running locally)

### Installation

1. Install the template from NuGet:

```bash
dotnet new install TelegramBot.Template
```

2. Create a new bot project:

```bash
dotnet new telegram-bot-template \
    --projectName "YourBotName" \
    --projectNameInKebab "your-bot-name" \
    --dbName "your_database_name" \
    --telegramBotToken "YOUR_BOT_TOKEN" # Optional
```

Parameters:
- `--projectName`: Required. The name of your project (e.g., "MyAwesomeBot")
- `--projectNameInKebab`: Required. Kebab-case version of your project name for Docker services (e.g., "my-awesome-bot")
- `--dbName`: Required. Database name for MariaDB
- `--telegramBotToken`: Optional. Your Telegram bot token. If not provided, you'll need to set it in appsettings.protected.json

### Configuration

1. Set up your Telegram bot token:
   - Open `src/YourBotName.Host/appsettings.protected.json`
   - Replace `TelegramBotToken` with your actual bot token for local development:

```json
{
  "TelegramBotOptions": {
    "Token": "YOUR_BOT_TOKEN"
  }
}
```

2. Configure database connection (if needed):
   - Update connection strings in `appsettings.json` files for your environment

### Running the Bot

#### Local Development

```bash
cd src/YourBotName.Host
dotnet run
```

#### Docker

```bash
docker-compose -f docker-compose.local.yml up -d
```

## Project Structure

```
├── src/
│   ├── YourBotName.Domain/           # Domain entities and interfaces
│   ├── YourBotName.AppServices/      # Application logic and handlers
│   │   ├── UseCases/                 # Bot command implementations
│   │   └── Handlers/                 # Message and callback handlers
│   ├── YourBotName.Persistence/      # Database context and repositories
│   └── YourBotName.Host/             # Application host and configuration
│       └── Resources/                # Localization files
```

## Adding New Features

### Creating a New Command

1. Define the command state in `BotState.cs`:

```csharp
public enum BotState
{
    // Existing states...
    YourNewCommand
}
```

2. Create a new handler in `AppServices/UseCases`:

```csharp
public class YourNewCommandHandler : ContextHandlerBase, IMatchingUpdateHandler<YourNewCommandMatcher>
{
    public async Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        await SetUserContext(update.Message.From.Id, cancellationToken);
        // Your command logic here
    }
}
```

3. Add localization resources in `Host/Resources/Handlers`

### Adding New User States

1. Update `UserState.cs`:

```csharp
public enum UserState
{
    None = 1,
    YourNewState
}
```

2. Implement state transition logic in your handlers using `UpdateState()`.

## Localization

Add translations in:
- `Resources/Buttons/` - Button texts
- `Resources/Handlers/` - Handler messages

Example localization JSON:

```json
{
  "CommandName": {
    "ButtonText": "Click me",
    "ResponseMessage": "Hello, {UserName}!"
  }
}
```

## Database Migrations

Create a new migration:

```bash
cd src/YourBotName.Persistence
./AddMigration.sh "YourMigrationName"
```

Apply migrations:
- Local: Migrations apply automatically on startup
- Docker: Migrations run in a separate container before the main application

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## Built With

- [Insight.TelegramBot](https://www.nuget.org/packages/Insight.TelegramBot/) - Telegram Bot framework
- [Entity Framework Core](https://docs.microsoft.com/ef/core/) - Data access and ORM
- [Serilog](https://serilog.net/) - Structured logging
- [MariaDB](https://mariadb.org/) - Database

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Telegram Bot API
- .NET Community
- Contributors and users of this template

## Support

If you find a bug or have a feature request, please create an issue in the GitHub repository.
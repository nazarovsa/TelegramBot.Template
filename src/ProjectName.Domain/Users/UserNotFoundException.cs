namespace ProjectName.Domain.Users;

public class UserNotFoundException : Exception
{
    public long TelegramId { get; }

    public UserNotFoundException(long telegramId)
        : base($"User with Telegram ID {telegramId} not found")
    {
        TelegramId = telegramId;
    }
}
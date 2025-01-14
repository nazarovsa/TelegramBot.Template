using Insight.TelegramBot;
using Insight.TelegramBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ProjectName.AppServices.Extensions;

public static class BotExtensions
{
    public static async Task EditOrSendTextMessage(this ITelegramBotClient bot,
        int messageId,
        TextMessage message,
        ILogger? logger = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await bot.EditMessageText(messageId, message, cancellationToken);
            return;
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "Не удалось отредактировать текстовое сообщение");
        }

        await bot.SendMessage(message, CancellationToken.None);
    }
}
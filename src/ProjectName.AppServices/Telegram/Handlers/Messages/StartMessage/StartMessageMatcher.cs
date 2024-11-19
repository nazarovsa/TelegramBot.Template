using Insight.TelegramBot.Handling.Matchers.TextMatchers;

namespace ProjectName.AppServices.Telegram.Handlers.Messages.StartMessage;

public sealed class StartMessageMatcher : TextStartWithUpdateMatcher
{
    public StartMessageMatcher()
    {
        Template = "/start";
    }
}
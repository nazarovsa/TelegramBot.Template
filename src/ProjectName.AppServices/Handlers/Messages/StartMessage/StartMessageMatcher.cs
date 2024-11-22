using Insight.TelegramBot.Handling.Matchers.TextMatchers;

namespace ProjectName.AppServices.Handlers.Messages.StartMessage;

public sealed class StartMessageMatcher : TextStartWithUpdateMatcher
{
    public StartMessageMatcher()
    {
        Template = "/start";
    }
}
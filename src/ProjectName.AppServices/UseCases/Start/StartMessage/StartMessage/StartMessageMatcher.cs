using Insight.TelegramBot.Handling.Matchers.TextMatchers;

namespace ProjectName.AppServices.UseCases.Start.StartMessage.StartMessage;

public  class StartMessageMatcher : TextStartWithUpdateMatcher
{
    public StartMessageMatcher() => Template = "/start";
}
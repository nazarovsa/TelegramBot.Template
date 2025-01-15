using Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

namespace ProjectName.AppServices.UseCases.SetUserTimezone;

public class SetTimezoneMatcher : StateCallbackQueryMatcher<BotState>
{
    public SetTimezoneMatcher() => ExpectingState = BotState.SetTimezone;
}
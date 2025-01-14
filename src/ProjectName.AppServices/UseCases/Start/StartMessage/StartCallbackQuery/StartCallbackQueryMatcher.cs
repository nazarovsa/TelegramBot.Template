using Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

namespace ProjectName.AppServices.UseCases.Start.StartMessage.StartCallbackQuery;

public  class StartCallbackQueryMatcher : StateCallbackQueryMatcher<BotState>
{
    public StartCallbackQueryMatcher() => ExpectingState = BotState.Start;
}
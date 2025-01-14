using Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

namespace ProjectName.AppServices.UseCases.UserSettings.ToggleLanguage;

public class ToggleLanguageMatcher : StateCallbackQueryMatcher<BotState>
{
    public ToggleLanguageMatcher() => ExpectingState = BotState.ToggleLanguage;
}
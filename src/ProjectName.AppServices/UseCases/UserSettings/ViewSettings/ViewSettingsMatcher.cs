using Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

namespace ProjectName.AppServices.UseCases.UserSettings.ViewSettings;

public class ViewSettingsMatcher : StateCallbackQueryMatcher<BotState>
{
    public ViewSettingsMatcher() => ExpectingState = BotState.ViewSettings;
}
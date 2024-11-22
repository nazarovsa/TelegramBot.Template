using Insight.TelegramBot;

namespace ProjectName.AppServices;

public class BotData : CallbackData<BotState>
{
    public BotData(BotState nextState, params string[] args) : base(nextState, args)
    {
    }

    public static BotData Start()
    {
        return new BotData(BotState.Start);
    }
}
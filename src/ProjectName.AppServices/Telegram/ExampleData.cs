using Insight.TelegramBot;

namespace ProjectName.AppServices.Telegram;

public class ExampleData : CallbackData<ExampleState>
{
    public ExampleData(ExampleState nextState, params string[] args) : base(nextState, args)
    {
    }

    public static ExampleData Start()
    {
        return new ExampleData(ExampleState.Start);
    }
}
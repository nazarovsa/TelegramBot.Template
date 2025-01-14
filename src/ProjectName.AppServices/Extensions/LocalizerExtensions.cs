using Insight.Localizer;

namespace ProjectName.AppServices.Extensions;

public static class LocalizerExtensions
{
    public static string GetBackButtonText(this ILocalizer localizer)
    {
        return localizer.Get("Buttons", "Back");
    }
}
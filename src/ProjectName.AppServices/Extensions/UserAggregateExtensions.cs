using ProjectName.Domain.Users;

namespace ProjectName.AppServices.Extensions;

public static class UserAggregateExtensions
{
    private const string CustomKey = nameof(CustomKey);

    public static bool TrySetCustomKey(this UserAggregate user, long customKey)
    {
        return user.TrySetStateDataKey(CustomKey, customKey.ToString());
    }

    public static bool TryGetCustomKey(this UserAggregate user, out long? customKey)
    {
        customKey = null;
        if (user.TryGetStateDataValue(CustomKey, out var customKeyStr) &&
            long.TryParse(customKeyStr, out var parsedCustomKey))
        {
            customKey = parsedCustomKey;
            return true;
        }

        return false;
    }
}
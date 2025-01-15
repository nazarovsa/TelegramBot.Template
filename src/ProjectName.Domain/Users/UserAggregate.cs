namespace ProjectName.Domain.Users;

public class UserAggregate
{
    private readonly Dictionary<string, string> _stateData = new(StringComparer.OrdinalIgnoreCase);
    public long Id { get; private set; }
    public string FirstName { get; private set; }
    public string? Username { get; private set; }
    public UserState State { get; private set; }
    public string Culture { get; private set; } = "en";
    public double Timezone { get; private set; }
    public DateTimeOffset LastActivityAt { get; private set; }
    public IReadOnlyDictionary<string, string> StateData => _stateData;
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }


    public static UserAggregate Initialize(long id, string firstName, DateTimeOffset utcNow, string culture,
        string? username = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(culture);

        return new UserAggregate
        {
            Id = id,
            Username = username,
            FirstName = firstName,
            LastActivityAt = utcNow,
            State = UserState.None,
            Culture = culture,
            UpdatedAt = utcNow,
            CreatedAt = utcNow,
        };
    }

    public void Update(string firstName, DateTimeOffset utcNow, string? username = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName);

        FirstName = firstName;
        Username = username;
        UpdatedAt = utcNow;
    }

    public void SetTimezone(double timezone, DateTimeOffset utcNow)
    {
        if (timezone is < -12 or > 14)
            throw new ArgumentException("Timezone should be between -12 and 14");

        Timezone = timezone;
        UpdatedAt = utcNow;
    }

    public void UpdateState(UserState state, DateTimeOffset utcNow)
    {
        if (state == UserState.None)
        {
            _stateData.Clear();
        }

        State = state;
        UpdatedAt = utcNow;
    }

    public void UpdateLastActivityAt(DateTimeOffset utcNow)
    {
        LastActivityAt = utcNow;
        UpdatedAt = utcNow;
    }


    public void UpdateCulture(string culture, DateTimeOffset utcNow)
    {
        ArgumentException.ThrowIfNullOrEmpty(culture);

        Culture = culture;
        UpdatedAt = utcNow;
    }

    public bool TryRemoveStateDataKey(string key)
    {
        return _stateData.Remove(key);
    }

    public bool TrySetStateDataKey(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        _stateData[key] = value;
        return true;
    }

    public bool TryGetStateDataValue(string key, out string value)
    {
        return _stateData.TryGetValue(key, out value);
    }
}
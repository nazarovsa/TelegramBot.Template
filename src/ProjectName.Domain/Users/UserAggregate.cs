namespace ProjectName.Domain.Users
{
    public sealed class UserAggregate
    {
        public long Id { get; private set; }
        public string FirstName { get; private set; }
        public string? Username { get; private set; }

        public DateTimeOffset LastActivityAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }


        public static UserAggregate Initialize(long id, string firstName, DateTimeOffset utcNow,
            string? username = null)
        {
            return new UserAggregate
            {
                Id = id,
                Username = username,
                FirstName = firstName,
                LastActivityAt = utcNow,
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

        public void UpdateLastActivityAt(DateTimeOffset utcNow)
        {
            LastActivityAt = utcNow;
            UpdatedAt = utcNow;
        }
    }
}
namespace KoosMatliApp.Services;

public sealed class AdminSession
{
    private Func<CancellationToken, Task<string?>>? _tokenProvider;
    private int _version;

    internal int Version => Volatile.Read(ref _version); ////(Microsoft Learn, 2026a).

    public string Uid { get; private set; } = "";
    public string Email { get; private set; } = "";

    public bool IsConnected => _tokenProvider is not null;

    public event EventHandler? AuthorizationRequired;

    public void Connect(
        string uid,
        string email,
        Func<CancellationToken, Task<string?>> tokenProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uid);
        ArgumentNullException.ThrowIfNull(tokenProvider);

        Interlocked.Increment(ref _version); //(Microsoft Learn, 2026a).

        Uid = uid;
        Email = email ?? "";
        _tokenProvider = tokenProvider;
    }

    public async Task<string?> GetIdTokenAsync(
        CancellationToken cancellationToken = default)
    {
        var version = Version;
        var provider = _tokenProvider;

        if (provider is null)
            return null;

        var token = await provider(cancellationToken);

        if (version != Version || string.IsNullOrWhiteSpace(token))
            return null;

        return token;
    }

    public void Clear()
    {
        Interlocked.Increment(ref _version);

        _tokenProvider = null;
        Uid = "";
        Email = "";
    }

    internal void NotifyAuthorizationRequired(int expectedVersion)
    {
        if (expectedVersion != Version)
            return;

        Clear();
        AuthorizationRequired?.Invoke(this, EventArgs.Empty); //(Microsoft Learn, 2026b).
    }
}

/*Reference List
 
Microsoft Corporation, "Thread Safety and Concurrency in .NET," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/standard/threading/thread-safety

Microsoft Corporation, "Events and Event Handlers (C# Programming Guide)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/

Microsoft Corporation, "Interlocked Class (System.Threading)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked
 */
namespace KCSCommunity.Abstractions.Interfaces.Services;

public interface IResourceLockService
{
    /// <summary>
    /// Attempts to acquire a lock on a specified resource.
    /// </summary>
    /// <param name="resourceKey">A unique key identifying the resource to be locked.</param>
    /// <param name="timeout">The maximum time to wait for the lock.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>
    /// A disposable object that releases the lock when disposed. Returns null if the lock could not be acquired within the timeout.
    /// </returns>
    Task<IDisposable?> AcquireLockAsync(string resourceKey, TimeSpan timeout, CancellationToken cancellationToken = default);
}
using KCSCommunity.Abstractions.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace KCSCommunity.Infrastructure.Services;

/// <summary>
/// 锁，后期考虑Redis-based
/// </summary>
public class InMemoryLockService : IResourceLockService
{
    private readonly IMemoryCache _cache;

    public InMemoryLockService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<IDisposable?> AcquireLockAsync(string resourceKey, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var semaphore = _cache.GetOrCreate(resourceKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            return new SemaphoreSlim(1, 1);
        });

        if (await semaphore.WaitAsync(timeout, cancellationToken))
        {
            return new SemaphoreReleaser(semaphore);
        }

        return null;
    }

    private sealed class SemaphoreReleaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _isDisposed;

        public SemaphoreReleaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _semaphore.Release();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
namespace ActivityPaint.Client.Components.Shared.Utilities;

public interface IListener
{
    IDisposable Listen(Func<CancellationToken, Task> func);
}

public class AsyncTrigger(int timeout = 30000) : IListener
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly List<AsyncListener> _listeners = [];
    private readonly int _timeout = timeout;

    public async Task Notify(CancellationToken cancellationToken = default)
    {
        if (OperatingSystem.IsBrowser())
        {
            // As WASM Blazor is currently limited to a single thread,
            // loop the listeners and let the UI update after each of them.
            foreach (var listener in _listeners)
            {
                if (await _semaphore.WaitAsync(_timeout, cancellationToken))
                {
                    await listener.Trigger(cancellationToken);
                    await Task.Delay(1, cancellationToken);

                    _semaphore.Release();
                }
            }
            return;
        }

        var tasks = _listeners.Select(x => x.Trigger(cancellationToken));
        await Task.WhenAll(tasks);
    }

    public IDisposable Listen(Func<CancellationToken, Task> func)
    {
        var listener = new AsyncListener(this, func);

        _listeners.Add(listener);

        return listener;
    }

    private void Remove(AsyncListener listener)
    {
        _listeners.Remove(listener);
    }

    private sealed class AsyncListener(AsyncTrigger trigger, Func<CancellationToken, Task> func) : IDisposable
    {
        private readonly AsyncTrigger _trigger = trigger;
        private readonly Func<CancellationToken, Task> _func = func;

        public Task Trigger(CancellationToken cancellationToken)
            => _func(cancellationToken);

        public void Dispose()
        {
            _trigger.Remove(this);
        }
    }
}

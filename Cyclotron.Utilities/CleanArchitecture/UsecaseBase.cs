namespace Cyclotron.Utilities.CleanArchitecture;

public abstract class UsecaseBase<TRequest, TResponse>(TRequest request, ICallback<TResponse> callback) where TRequest : IUsecaseRequest where TResponse : IUsecaseResponse
{
    protected readonly TRequest Request = request;

    protected readonly ICallback<TResponse> Callback = callback;

    protected virtual bool TryServeFromCache() => false;

    protected virtual Task ActionAsync()
    {
        return Task.CompletedTask;
    }

    public void Execute()
    {
        try
        {
            if (Request.IsLocalRequest() && TryServeFromCache())
            {
                return;
            }
        }
        catch (Exception)
        {
            // Swallow exception from cache serving and proceed to ActionAsync
        }

        _ = Task.Run(async () =>
        {
            try
            {
                if (Request.CancellationToken.IsCancellationRequested)
                {
                    Callback?.OnCanceled(default!);
                    return;
                }
                await ActionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Callback?.OnError(Request, ex);
            }
        }).ConfigureAwait(false);
    }
}
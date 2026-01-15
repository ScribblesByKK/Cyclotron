namespace Cyclotron.Utilities.CleanArchitecture;

/// <summary>
/// Base class for implementing use cases following the Clean Architecture pattern.
/// </summary>
/// <typeparam name="TRequest">The type of request that implements <see cref="IUsecaseRequest"/>.</typeparam>
/// <typeparam name="TResponse">The type of response that implements <see cref="IUsecaseResponse"/>.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="UsecaseBase{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="request">The request containing input parameters for the use case.</param>
/// <param name="callback">The callback handler to receive operation results.</param>
public abstract class UsecaseBase<TRequest, TResponse>(TRequest request, ICallback<TResponse> callback) where TRequest : IUsecaseRequest where TResponse : IUsecaseResponse
{
    /// <summary>
    /// Gets the request containing input parameters for this use case.
    /// </summary>
    protected readonly TRequest Request = request;

    /// <summary>
    /// Gets the callback handler to receive operation results.
    /// </summary>
    protected readonly ICallback<TResponse> Callback = callback;

    /// <summary>
    /// Attempts to serve the response from cache for local requests.
    /// </summary>
    /// <returns><c>true</c> if the response was successfully served from cache; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Override this method to implement caching logic for local requests.
    /// If this method returns <c>true</c>, the <see cref="ActionAsync"/> method will not be called.
    /// Default implementation returns <c>false</c>.
    /// </remarks>
    protected virtual bool TryServeFromCache() => false;

    /// <summary>
    /// Performs the main use case operation asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Override this method to implement the core business logic of the use case.
    /// This method is called after cache check (if applicable) and cancellation check.
    /// </remarks>
    protected abstract Task ActionAsync();

    /// <summary>
    /// Executes the use case operation.
    /// </summary>
    /// <remarks>
    /// This method orchestrates the use case execution:
    /// <list type="number">
    /// <item><description>Attempts to serve from cache if the request is a local request</description></item>
    /// <item><description>Runs the main operation asynchronously via <see cref="ActionAsync"/></description></item>
    /// <item><description>Checks for cancellation before executing the main operation</description></item>
    /// <item><description>Handles exceptions and invokes appropriate callback methods</description></item>
    /// </list>
    /// The method executes asynchronously without blocking the calling thread.
    /// </remarks>
    public void Execute()
    {
        try
        {
            // Attempt to serve from cache for local requests
            if (Request.IsLocalRequest() && TryServeFromCache())
            {
                return;
            }
        }
        catch (Exception)
        {
            // Swallow exception from cache serving and proceed to ActionAsync
            // This ensures that cache failures don't prevent the main operation
        }

        // Execute the main operation asynchronously
        _ = Task.Run(async () =>
        {
            try
            {
                // Check for cancellation before starting the operation
                if (Request.CancellationToken.IsCancellationRequested)
                {
                    Callback?.OnCanceled(default!);
                    return;
                }
                await ActionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and invoke the error callback
                Callback?.OnError(Request, ex);
            }
        }).ConfigureAwait(false);
    }
}
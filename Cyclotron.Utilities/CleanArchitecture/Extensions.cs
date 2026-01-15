namespace Cyclotron.Utilities.CleanArchitecture;

/// <summary>
/// Provides extension methods for <see cref="IUsecaseRequest"/> to simplify request type checking.
/// </summary>
public static class RequestExtensions
{
    /// <summary>
    /// Determines whether the request is exclusively a local request.
    /// </summary>
    /// <param name="request">The use case request to check.</param>
    /// <returns><c>true</c> if the request is of type <see cref="RequestType.Local"/>; otherwise, <c>false</c>.</returns>
    public static bool IsLocalRequest(this IUsecaseRequest request) => request.RequestType == RequestType.Local;

    /// <summary>
    /// Determines whether the request is exclusively a network request.
    /// </summary>
    /// <param name="request">The use case request to check.</param>
    /// <returns><c>true</c> if the request is of type <see cref="RequestType.Network"/>; otherwise, <c>false</c>.</returns>
    public static bool IsNetworkRequest(this IUsecaseRequest request) => request.RequestType == RequestType.Network;

    /// <summary>
    /// Determines whether the request includes a local component.
    /// </summary>
    /// <param name="request">The use case request to check.</param>
    /// <returns><c>true</c> if the request type includes local access (Local, LocalAndNetwork, or Sync); otherwise, <c>false</c>.</returns>
    public static bool HasLocalRequest(this IUsecaseRequest request) => request.IsLocalRequest() || request.RequestType == RequestType.LocalAndNetwork || request.RequestType == RequestType.Sync;

    /// <summary>
    /// Determines whether the request includes a network component.
    /// </summary>
    /// <param name="request">The use case request to check.</param>
    /// <returns><c>true</c> if the request type includes network access (Network, LocalAndNetwork, or Sync); otherwise, <c>false</c>.</returns>
    public static bool HasNetworkRequest(this IUsecaseRequest request) => request.IsNetworkRequest() || request.RequestType == RequestType.LocalAndNetwork || request.RequestType == RequestType.Sync;
}

/// <summary>
/// Provides extension methods for <see cref="ICallback{R}"/> to simplify error handling.
/// </summary>
public static class CallbackExtensions
{
    /// <summary>
    /// Handles exceptions and invokes the appropriate callback method based on the exception type.
    /// </summary>
    /// <typeparam name="R">The type of response that implements <see cref="IUsecaseResponse"/>.</typeparam>
    /// <param name="callback">The callback to invoke.</param>
    /// <param name="request">The use case request associated with the exception.</param>
    /// <param name="ex">The exception that occurred.</param>
    /// <remarks>
    /// This method distinguishes between:
    /// <list type="bullet">
    /// <item><description>Cancellation due to explicit cancellation token request - invokes OnCanceled</description></item>
    /// <item><description>Timeout due to OperationCanceledException without explicit cancellation - invokes OnError with TimedOut type</description></item>
    /// <item><description>All other exceptions - invokes OnError with Unknown error type</description></item>
    /// </list>
    /// </remarks>
    public static void OnError<R>(this ICallback<R> callback, IUsecaseRequest request, Exception ex) where R : IUsecaseResponse
    {
        if (callback == null) { return; }

        switch (ex)
        {
            case OperationCanceledException when request.CancellationToken.IsCancellationRequested:
                // Explicit cancellation via cancellation token
                callback.OnCanceled(default!);
                break;
            case OperationCanceledException:
                // Timeout - OperationCanceledException without explicit cancellation
                var errorResponse = new ErrorResponse(request, ErrorType.TimedOut, ex);
                callback.OnError(errorResponse);
                break;
            default:
                // All other exceptions
                callback.OnError(new ErrorResponse(request, ErrorType.Unknown, ex));
                break;
        }
    }
}
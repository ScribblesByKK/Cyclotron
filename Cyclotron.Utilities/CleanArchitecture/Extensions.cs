namespace Cyclotron.Utilities.CleanArchitecture;

public static class RequestExtensions
{
    public static bool IsLocalRequest(this IUsecaseRequest request) => request.RequestType == RequestType.Local;

    public static bool IsNetworkRequest(this IUsecaseRequest request) => request.RequestType == RequestType.Network;

    public static bool HasLocalRequest(this IUsecaseRequest request) => request.IsLocalRequest() || request.RequestType == RequestType.LocalAndNetwork || request.RequestType == RequestType.Sync;

    public static bool HasNetworkRequest(this IUsecaseRequest request) => request.IsNetworkRequest() || request.RequestType == RequestType.LocalAndNetwork || request.RequestType == RequestType.Sync;
}

public static class CallbackExtensions
{
    public static void OnError<R>(this ICallback<R> callback, IUsecaseRequest request, Exception ex) where R : IUsecaseResponse
    {
        if (callback == null) { return; }

        switch (ex)
        {
            case OperationCanceledException when request.CancellationToken.IsCancellationRequested:
                callback.OnCanceled(default!);
                break;
            case OperationCanceledException:
                var errorResponse = new ErrorResponse(request, ErrorType.TimedOut, ex);
                callback.OnError(errorResponse);
                break;
            default:
                callback.OnError(new ErrorResponse(request, ErrorType.Unknown, ex));
                break;
        }
    }
}
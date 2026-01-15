namespace Cyclotron.Utilities.CleanArchitecture;

/// <summary>
/// Represents a callback handler for use case operations.
/// </summary>
/// <typeparam name="R">The type of response that implements <see cref="IUsecaseResponse"/>.</typeparam>
public interface ICallback<R> where R : IUsecaseResponse
{
    /// <summary>
    /// Called when the use case operation completes successfully.
    /// </summary>
    /// <param name="response">The successful response from the use case operation.</param>
    void OnSuccess(R response);

    /// <summary>
    /// Called when the use case operation fails with a known failure condition.
    /// </summary>
    /// <param name="response">The failed response containing failure details.</param>
    void OnFailed(R response);

    /// <summary>
    /// Called when an error occurs during the use case operation.
    /// </summary>
    /// <param name="error">The error response containing error details and exception information.</param>
    void OnError(ErrorResponse error);

    /// <summary>
    /// Called to report progress updates during a long-running use case operation.
    /// </summary>
    /// <param name="response">The progress response containing intermediate results or status.</param>
    void OnProgress(R response);

    /// <summary>
    /// Called when the use case operation is canceled.
    /// </summary>
    /// <param name="response">The response indicating cancellation state.</param>
    void OnCanceled(R response);
}
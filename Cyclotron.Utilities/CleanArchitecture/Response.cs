namespace Cyclotron.Utilities.CleanArchitecture;

/// <summary>
/// Specifies the type of response for a use case operation.
/// </summary>
public enum ResponseType
{
    /// <summary>
    /// Indicates a response from local storage or database.
    /// </summary>
    Local,
    /// <summary>
    /// Indicates a response fetched from the server.
    /// </summary>
    Network,
    /// <summary>
    /// Indicates a response that combines both local and network data.
    /// </summary>
    LocalAndNetwork,
    /// <summary>
    /// Indicates a response that contains incremental sync data from the server after applying it on local data.
    /// </summary>
    Sync
}


/// <summary>
/// Specifies the type of error that can occur in a use case operation.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// An unknown error occurred.
    /// </summary>
    Unknown,
    /// <summary>
    /// A database error occurred.
    /// </summary>
    DBError,
    /// <summary>
    /// A server error occurred.
    /// </summary>
    ServerError,
    /// <summary>
    /// The operation timed out.
    /// </summary>
    TimedOut,
    /// <summary>
    /// No internet access is available.
    /// </summary>
    NoInternetAccess
}


/// <summary>
/// Represents a response for a use case operation.
/// </summary>
public interface IUsecaseResponse
{
    /// <summary>
    /// Gets the request associated with this response.
    /// </summary>
    IUsecaseRequest Request { get; }

    /// <summary>
    /// Gets the type of the response.
    /// </summary>
    ResponseType ResponseType { get; }
}


/// <summary>
/// Represents an error response for a use case operation.
/// </summary>
public class ErrorResponse(IUsecaseRequest request, ErrorType errorType, Exception exception)
{
    /// <summary>
    /// Gets or sets the request associated with this response.
    /// </summary>
    public IUsecaseRequest Request => _request;
    protected readonly IUsecaseRequest _request = request;

    /// <summary>
    /// Gets or sets the type of error.
    /// </summary>
    public ErrorType ErrorType => _errorType;
    protected readonly ErrorType _errorType = errorType;

    /// <summary>
    /// Gets or sets the exception associated with the error.
    /// </summary>
    public Exception Exception => _exception;
    protected readonly Exception _exception = exception;
}
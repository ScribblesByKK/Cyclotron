namespace Cyclotron.Utilities.CleanArchitecture;


/// <summary>
/// Specifies the type of request for a use case operation.
/// </summary>
public enum RequestType
{
    /// <summary>
    /// Indicates a request to local storage or database.
    /// </summary>
    Local,
    /// <summary>
    /// Indicates a request that fetches the data from server.
    /// </summary>
    Network,
    /// <summary>
    /// Indicates a request that fetches from both local storage and server.
    /// </summary>
    LocalAndNetwork,
    /// <summary>
    /// Indicates a request that fetches incremental sync data from server after applying it on local data.
    /// </summary>
    Sync
}


/// <summary>
/// Represents a request for a use case operation.
/// </summary>
public interface IUsecaseRequest
{
    /// <summary>
    /// Gets the type of the request.
    /// </summary>
    RequestType RequestType { get; }

    /// <summary>
    /// Gets the user ID associated with the request.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// Gets the <see cref="CancellationToken"/> associated with the request.
    /// </summary>
    CancellationToken CancellationToken { get; }
}


/// <summary>
/// Default implementation of <see cref="IUsecaseRequest"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UsecaseRequest"/> class.
/// </remarks>
/// <param name="requestType">The type of the request.</param>
/// <param name="userId">The user ID associated with the request.</param>
/// <param name="cancellationToken">The cancellation token for the request (optional).</param>
public abstract class UsecaseRequest(RequestType requestType, string userId, CancellationToken cancellationToken = default) : IUsecaseRequest
{
    /// <inheritdoc/>
    public RequestType RequestType { get => _requestType; }
    private readonly RequestType _requestType = requestType;

    /// <inheritdoc/>
    public string UserId { get => _userId; }
    private readonly string _userId = userId;

    /// <inheritdoc/>
    public CancellationToken CancellationToken { get => _cancellationToken; }
    private readonly CancellationToken _cancellationToken = cancellationToken;
}
using AwesomeAssertions;
using Cyclotron.Utilities.CleanArchitecture;
using NSubstitute;

namespace Cyclotron.Tests.Unit.Utilities;

[Category("Unit")]
[Category("Utilities")]
public class UtilitiesTests
{
    private IUsecaseRequest _request = null!;
    private ICallback<IUsecaseResponse> _callback = null!;

    [Before(Test)]
    public void Setup()
    {
        _request = Substitute.For<IUsecaseRequest>();
        _callback = Substitute.For<ICallback<IUsecaseResponse>>();
    }

    #region RequestType Enum

    [Test]
    public void RequestType_HasExpectedValues()
    {
        Enum.GetValues<RequestType>().Should().HaveCount(4);
        RequestType.Local.Should().Be((RequestType)0);
        RequestType.Network.Should().Be((RequestType)1);
        RequestType.LocalAndNetwork.Should().Be((RequestType)2);
        RequestType.Sync.Should().Be((RequestType)3);
    }

    #endregion

    #region ResponseType Enum

    [Test]
    public void ResponseType_HasExpectedValues()
    {
        Enum.GetValues<ResponseType>().Should().HaveCount(4);
        ResponseType.Local.Should().Be((ResponseType)0);
        ResponseType.Network.Should().Be((ResponseType)1);
        ResponseType.LocalAndNetwork.Should().Be((ResponseType)2);
        ResponseType.Sync.Should().Be((ResponseType)3);
    }

    #endregion

    #region ErrorType Enum

    [Test]
    public void ErrorType_HasExpectedValues()
    {
        Enum.GetValues<ErrorType>().Should().HaveCount(5);
        ErrorType.Unknown.Should().Be((ErrorType)0);
        ErrorType.DBError.Should().Be((ErrorType)1);
        ErrorType.ServerError.Should().Be((ErrorType)2);
        ErrorType.TimedOut.Should().Be((ErrorType)3);
        ErrorType.NoInternetAccess.Should().Be((ErrorType)4);
    }

    #endregion

    #region UsecaseRequest Construction

    [Test]
    public void UsecaseRequest_WithValidParameters_StoresRequestType()
    {
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");

        request.RequestType.Should().Be(RequestType.Network);
    }

    [Test]
    public void UsecaseRequest_WithValidParameters_StoresUserId()
    {
        var request = new TestUsecaseRequest(RequestType.Local, "user-42");

        request.UserId.Should().Be("user-42");
    }

    [Test]
    public void UsecaseRequest_WithCancellationToken_StoresToken()
    {
        using var cts = new CancellationTokenSource();
        var request = new TestUsecaseRequest(RequestType.Sync, "user-1", cts.Token);

        request.CancellationToken.Should().Be(cts.Token);
    }

    [Test]
    public void UsecaseRequest_WithDefaultCancellationToken_StoresDefaultToken()
    {
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");

        request.CancellationToken.Should().Be(CancellationToken.None);
    }

    [Test]
    public void UsecaseRequest_ImplementsIUsecaseRequest()
    {
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");

        request.Should().BeAssignableTo<IUsecaseRequest>();
    }

    #endregion

    #region IsLocalRequest Extension

    [Test]
    public void IsLocalRequest_WithLocalType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Local);

        _request.IsLocalRequest().Should().BeTrue();
    }

    [Test]
    public void IsLocalRequest_WithNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Network);

        _request.IsLocalRequest().Should().BeFalse();
    }

    [Test]
    public void IsLocalRequest_WithLocalAndNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        _request.IsLocalRequest().Should().BeFalse();
    }

    [Test]
    public void IsLocalRequest_WithSyncType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Sync);

        _request.IsLocalRequest().Should().BeFalse();
    }

    #endregion

    #region IsNetworkRequest Extension

    [Test]
    public void IsNetworkRequest_WithNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Network);

        _request.IsNetworkRequest().Should().BeTrue();
    }

    [Test]
    public void IsNetworkRequest_WithLocalType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Local);

        _request.IsNetworkRequest().Should().BeFalse();
    }

    [Test]
    public void IsNetworkRequest_WithLocalAndNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        _request.IsNetworkRequest().Should().BeFalse();
    }

    [Test]
    public void IsNetworkRequest_WithSyncType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Sync);

        _request.IsNetworkRequest().Should().BeFalse();
    }

    #endregion

    #region HasLocalRequest Extension

    [Test]
    public void HasLocalRequest_WithLocalType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Local);

        _request.HasLocalRequest().Should().BeTrue();
    }

    [Test]
    public void HasLocalRequest_WithLocalAndNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        _request.HasLocalRequest().Should().BeTrue();
    }

    [Test]
    public void HasLocalRequest_WithSyncType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Sync);

        _request.HasLocalRequest().Should().BeTrue();
    }

    [Test]
    public void HasLocalRequest_WithNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Network);

        _request.HasLocalRequest().Should().BeFalse();
    }

    #endregion

    #region HasNetworkRequest Extension

    [Test]
    public void HasNetworkRequest_WithNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Network);

        _request.HasNetworkRequest().Should().BeTrue();
    }

    [Test]
    public void HasNetworkRequest_WithLocalAndNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        _request.HasNetworkRequest().Should().BeTrue();
    }

    [Test]
    public void HasNetworkRequest_WithSyncType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Sync);

        _request.HasNetworkRequest().Should().BeTrue();
    }

    [Test]
    public void HasNetworkRequest_WithLocalType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Local);

        _request.HasNetworkRequest().Should().BeFalse();
    }

    #endregion

    #region ErrorResponse Construction

    [Test]
    public void ErrorResponse_Constructor_StoresRequest()
    {
        var exception = new InvalidOperationException("test");

        var errorResponse = new ErrorResponse(_request, ErrorType.Unknown, exception);

        errorResponse.Request.Should().BeSameAs(_request);
    }

    [Test]
    public void ErrorResponse_Constructor_StoresErrorType()
    {
        var exception = new InvalidOperationException("test");

        var errorResponse = new ErrorResponse(_request, ErrorType.ServerError, exception);

        errorResponse.ErrorType.Should().Be(ErrorType.ServerError);
    }

    [Test]
    public void ErrorResponse_Constructor_StoresException()
    {
        var exception = new InvalidOperationException("specific error");

        var errorResponse = new ErrorResponse(_request, ErrorType.DBError, exception);

        errorResponse.Exception.Should().BeSameAs(exception);
    }

    [Test]
    public void ErrorResponse_Constructor_WithAllErrorTypes_StoresCorrectly()
    {
        var exception = new Exception("test");

        foreach (var errorType in Enum.GetValues<ErrorType>())
        {
            var response = new ErrorResponse(_request, errorType, exception);
            response.ErrorType.Should().Be(errorType);
        }
    }

    #endregion

    #region CallbackExtensions.OnError — Cancellation

    [Test]
    public void OnError_WithOperationCanceledAndTokenCanceled_CallsOnCanceled()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        _request.CancellationToken.Returns(cts.Token);
        var exception = new OperationCanceledException(cts.Token);

        _callback.OnError(_request, exception);

        _callback.Received(1).OnCanceled(Arg.Any<IUsecaseResponse>());
        _callback.DidNotReceive().OnError(Arg.Any<ErrorResponse>());
    }

    #endregion

    #region CallbackExtensions.OnError — Timeout

    [Test]
    public void OnError_WithOperationCanceledButTokenNotCanceled_CallsOnErrorWithTimedOut()
    {
        _request.CancellationToken.Returns(CancellationToken.None);
        var exception = new OperationCanceledException();

        _callback.OnError(_request, exception);

        _callback.Received(1).OnError(Arg.Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.TimedOut &&
            e.Request == _request &&
            e.Exception == exception));
        _callback.DidNotReceive().OnCanceled(Arg.Any<IUsecaseResponse>());
    }

    #endregion

    #region CallbackExtensions.OnError — Unknown Exception

    [Test]
    public void OnError_WithGenericException_CallsOnErrorWithUnknown()
    {
        _request.CancellationToken.Returns(CancellationToken.None);
        var exception = new InvalidOperationException("something broke");

        _callback.OnError(_request, exception);

        _callback.Received(1).OnError(Arg.Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.Unknown &&
            e.Request == _request &&
            e.Exception == exception));
        _callback.DidNotReceive().OnCanceled(Arg.Any<IUsecaseResponse>());
    }

    [Test]
    public void OnError_WithArgumentException_CallsOnErrorWithUnknown()
    {
        _request.CancellationToken.Returns(CancellationToken.None);
        var exception = new ArgumentException("bad arg");

        _callback.OnError(_request, exception);

        _callback.Received(1).OnError(Arg.Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.Unknown));
    }

    #endregion

    #region CallbackExtensions.OnError — Null Callback

    [Test]
    public void OnError_WithNullCallback_DoesNotThrow()
    {
        ICallback<IUsecaseResponse>? nullCallback = null;
        var exception = new Exception("test");

        var act = () => nullCallback!.OnError(_request, exception);

        act.Should().NotThrow();
    }

    #endregion

    #region UsecaseBase — Execute with Cache Hit

    [Test]
    public async Task Execute_WithLocalRequestAndCacheHit_ReturnsEarlyWithoutCallingActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new TestUsecase(request, _callback, tryServeFromCacheResult: true, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        });

        usecase.Execute();

        // Give a short window for Task.Run to potentially execute if it was erroneously called
        var completed = await Task.WhenAny(tcs.Task, Task.Delay(200));
        completed.Should().NotBeSameAs(tcs.Task, "ActionAsync should not have been called when cache hit");
    }

    #endregion

    #region UsecaseBase — Execute with Cache Miss

    [Test]
    public async Task Execute_WithLocalRequestAndCacheMiss_CallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new TestUsecase(request, _callback, tryServeFromCacheResult: false, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        });

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        actionCalled.Should().BeTrue();
    }

    #endregion

    #region UsecaseBase — Execute with Non-Local Request

    [Test]
    public async Task Execute_WithNetworkRequest_SkipsCacheAndCallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");
        var cacheChecked = false;
        var usecase = new TestUsecase(request, _callback, tryServeFromCacheResult: false, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        }, onTryServeFromCache: () => cacheChecked = true);

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        actionCalled.Should().BeTrue();
        cacheChecked.Should().BeFalse("cache should not be checked for network requests");
    }

    #endregion

    #region UsecaseBase — Execute with Cache Exception

    [Test]
    public async Task Execute_WhenTryServeFromCacheThrows_SwallowsAndCallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new TestUsecase(request, _callback, tryServeFromCacheResult: false, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        }, throwOnCache: true);

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        actionCalled.Should().BeTrue();
    }

    #endregion

    #region UsecaseBase — Execute with Cancellation

    [Test]
    public async Task Execute_WhenCancellationRequested_CallsOnCanceled()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var request = new TestUsecaseRequest(RequestType.Network, "user-1", cts.Token);
        var actionCalled = false;
        var callbackCalled = new TaskCompletionSource<bool>();
        _callback.When(c => c.OnCanceled(Arg.Any<IUsecaseResponse>()))
            .Do(_ => callbackCalled.TrySetResult(true));
        var usecase = new TestUsecase(request, _callback, tryServeFromCacheResult: false, actionAsync: () =>
        {
            actionCalled = true;
            return Task.CompletedTask;
        });

        usecase.Execute();

        await callbackCalled.Task.WaitAsync(TimeSpan.FromSeconds(5));
        _callback.Received(1).OnCanceled(Arg.Any<IUsecaseResponse>());
        actionCalled.Should().BeFalse("ActionAsync should not run when cancellation is requested");
    }

    #endregion

    #region UsecaseBase — Execute with ActionAsync Exception

    [Test]
    public async Task Execute_WhenActionAsyncThrows_CallsCallbackOnError()
    {
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");
        var exception = new InvalidOperationException("action failed");
        var errorCalled = new TaskCompletionSource<bool>();
        _callback.When(c => c.OnError(Arg.Any<ErrorResponse>()))
            .Do(_ => errorCalled.TrySetResult(true));
        var usecase = new TestUsecase(request, _callback, tryServeFromCacheResult: false, actionAsync: () =>
        {
            throw exception;
        });

        usecase.Execute();

        await errorCalled.Task.WaitAsync(TimeSpan.FromSeconds(5));
        _callback.Received(1).OnError(Arg.Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.Unknown &&
            e.Exception == exception));
    }

    #endregion

    #region UsecaseBase — Execute with Null Callback

    [Test]
    public void Execute_WithNullCallbackAndCancellation_DoesNotThrow()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var request = new TestUsecaseRequest(RequestType.Network, "user-1", cts.Token);
        var usecase = new TestUsecase(request, null!, tryServeFromCacheResult: false, actionAsync: () => Task.CompletedTask);

        var act = () => usecase.Execute();

        act.Should().NotThrow();
    }

    [Test]
    public void Execute_WithNullCallbackAndActionException_DoesNotThrow()
    {
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");
        var usecase = new TestUsecase(request, null!, tryServeFromCacheResult: false, actionAsync: () =>
        {
            throw new InvalidOperationException("boom");
        });

        var act = () => usecase.Execute();

        act.Should().NotThrow();
    }

    #endregion

    #region UsecaseBase — TryServeFromCache Default

    [Test]
    public async Task TryServeFromCache_DefaultImplementation_ReturnsFalseAndCallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new DefaultCacheUsecase(request, _callback, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        });

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        actionCalled.Should().BeTrue("default TryServeFromCache returns false so ActionAsync should run");
    }

    #endregion

    #region IUsecaseResponse Interface

    [Test]
    public void IUsecaseResponse_MockedInstance_ExposesRequestAndResponseType()
    {
        var response = Substitute.For<IUsecaseResponse>();
        response.Request.Returns(_request);
        response.ResponseType.Returns(ResponseType.Network);

        response.Request.Should().BeSameAs(_request);
        response.ResponseType.Should().Be(ResponseType.Network);
    }

    #endregion

    #region ICallback Interface

    [Test]
    public void ICallback_OnSuccess_CanBeInvoked()
    {
        var response = Substitute.For<IUsecaseResponse>();

        _callback.OnSuccess(response);

        _callback.Received(1).OnSuccess(response);
    }

    [Test]
    public void ICallback_OnFailed_CanBeInvoked()
    {
        var response = Substitute.For<IUsecaseResponse>();

        _callback.OnFailed(response);

        _callback.Received(1).OnFailed(response);
    }

    [Test]
    public void ICallback_OnProgress_CanBeInvoked()
    {
        var response = Substitute.For<IUsecaseResponse>();

        _callback.OnProgress(response);

        _callback.Received(1).OnProgress(response);
    }

    [Test]
    public void ICallback_OnCanceled_CanBeInvoked()
    {
        var response = Substitute.For<IUsecaseResponse>();

        _callback.OnCanceled(response);

        _callback.Received(1).OnCanceled(response);
    }

    [Test]
    public void ICallback_OnError_CanBeInvoked()
    {
        var errorResponse = new ErrorResponse(_request, ErrorType.Unknown, new Exception());

        _callback.OnError(errorResponse);

        _callback.Received(1).OnError(errorResponse);
    }

    #endregion

    #region Test Doubles

    /// <summary>
    /// Concrete implementation of UsecaseRequest for testing.
    /// </summary>
    private sealed class TestUsecaseRequest(
        RequestType requestType,
        string userId,
        CancellationToken cancellationToken = default)
        : UsecaseRequest(requestType, userId, cancellationToken);

    /// <summary>
    /// Concrete implementation of UsecaseBase for testing with configurable behavior.
    /// </summary>
    private sealed class TestUsecase : UsecaseBase<IUsecaseRequest, IUsecaseResponse>
    {
        private readonly bool _tryServeFromCacheResult;
        private readonly Func<Task> _actionAsync;
        private readonly bool _throwOnCache;
        private readonly Action? _onTryServeFromCache;

        public TestUsecase(
            IUsecaseRequest request,
            ICallback<IUsecaseResponse> callback,
            bool tryServeFromCacheResult,
            Func<Task> actionAsync,
            bool throwOnCache = false,
            Action? onTryServeFromCache = null)
            : base(request, callback)
        {
            _tryServeFromCacheResult = tryServeFromCacheResult;
            _actionAsync = actionAsync;
            _throwOnCache = throwOnCache;
            _onTryServeFromCache = onTryServeFromCache;
        }

        protected override bool TryServeFromCache()
        {
            _onTryServeFromCache?.Invoke();
            if (_throwOnCache) throw new InvalidOperationException("Cache error");
            return _tryServeFromCacheResult;
        }

        protected override Task ActionAsync() => _actionAsync();
    }

    /// <summary>
    /// Concrete implementation of UsecaseBase that does NOT override TryServeFromCache,
    /// exercising the default (return false) path.
    /// </summary>
    private sealed class DefaultCacheUsecase : UsecaseBase<IUsecaseRequest, IUsecaseResponse>
    {
        private readonly Func<Task> _actionAsync;

        public DefaultCacheUsecase(
            IUsecaseRequest request,
            ICallback<IUsecaseResponse> callback,
            Func<Task> actionAsync)
            : base(request, callback)
        {
            _actionAsync = actionAsync;
        }

        protected override Task ActionAsync() => _actionAsync();
    }

    #endregion
}

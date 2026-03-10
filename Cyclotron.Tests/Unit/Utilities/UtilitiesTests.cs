using Cyclotron.Utilities.CleanArchitecture;

namespace Cyclotron.Tests.Unit.Utilities;

[Category("Unit")]
[Category("Utilities")]
public class UtilitiesTests
{
    private Mock<IUsecaseRequest> _request = null!;
    private Mock<ICallback<IUsecaseResponse>> _callback = null!;

    [Before(Test)]
    public void Setup()
    {
        _request = Mock.Of<IUsecaseRequest>();
        _callback = Mock.Of<ICallback<IUsecaseResponse>>();
    }

    #region RequestType Enum

    [Test]
    public async Task RequestType_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<RequestType>()).Count().IsEqualTo(4);
        await Assert.That(RequestType.Local).IsEqualTo((RequestType)0);
        await Assert.That(RequestType.Network).IsEqualTo((RequestType)1);
        await Assert.That(RequestType.LocalAndNetwork).IsEqualTo((RequestType)2);
        await Assert.That(RequestType.Sync).IsEqualTo((RequestType)3);
    }

    #endregion

    #region ResponseType Enum

    [Test]
    public async Task ResponseType_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<ResponseType>()).Count().IsEqualTo(4);
        await Assert.That(ResponseType.Local).IsEqualTo((ResponseType)0);
        await Assert.That(ResponseType.Network).IsEqualTo((ResponseType)1);
        await Assert.That(ResponseType.LocalAndNetwork).IsEqualTo((ResponseType)2);
        await Assert.That(ResponseType.Sync).IsEqualTo((ResponseType)3);
    }

    #endregion

    #region ErrorType Enum

    [Test]
    public async Task ErrorType_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<ErrorType>()).Count().IsEqualTo(5);
        await Assert.That(ErrorType.Unknown).IsEqualTo((ErrorType)0);
        await Assert.That(ErrorType.DBError).IsEqualTo((ErrorType)1);
        await Assert.That(ErrorType.ServerError).IsEqualTo((ErrorType)2);
        await Assert.That(ErrorType.TimedOut).IsEqualTo((ErrorType)3);
        await Assert.That(ErrorType.NoInternetAccess).IsEqualTo((ErrorType)4);
    }

    #endregion

    #region UsecaseRequest Construction

    [Test]
    public async Task UsecaseRequest_WithValidParameters_StoresRequestType()
    {
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");

        await Assert.That(request.RequestType).IsEqualTo(RequestType.Network);
    }

    [Test]
    public async Task UsecaseRequest_WithValidParameters_StoresUserId()
    {
        var request = new TestUsecaseRequest(RequestType.Local, "user-42");

        await Assert.That(request.UserId).IsEqualTo("user-42");
    }

    [Test]
    public async Task UsecaseRequest_WithCancellationToken_StoresToken()
    {
        using var cts = new CancellationTokenSource();
        var request = new TestUsecaseRequest(RequestType.Sync, "user-1", cts.Token);

        await Assert.That(request.CancellationToken).IsEqualTo(cts.Token);
    }

    [Test]
    public async Task UsecaseRequest_WithDefaultCancellationToken_StoresDefaultToken()
    {
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");

        await Assert.That(request.CancellationToken).IsEqualTo(CancellationToken.None);
    }

    [Test]
    public async Task UsecaseRequest_ImplementsIUsecaseRequest()
    {
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");

        await Assert.That<object>(request).IsAssignableTo<IUsecaseRequest>();
    }

    #endregion

    #region IsLocalRequest Extension

    [Test]
    public async Task IsLocalRequest_WithLocalType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Local);

        await Assert.That(_request.Object.IsLocalRequest()).IsTrue();
    }

    [Test]
    public async Task IsLocalRequest_WithNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Network);

        await Assert.That(_request.Object.IsLocalRequest()).IsFalse();
    }

    [Test]
    public async Task IsLocalRequest_WithLocalAndNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        await Assert.That(_request.Object.IsLocalRequest()).IsFalse();
    }

    [Test]
    public async Task IsLocalRequest_WithSyncType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Sync);

        await Assert.That(_request.Object.IsLocalRequest()).IsFalse();
    }

    #endregion

    #region IsNetworkRequest Extension

    [Test]
    public async Task IsNetworkRequest_WithNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Network);

        await Assert.That(_request.Object.IsNetworkRequest()).IsTrue();
    }

    [Test]
    public async Task IsNetworkRequest_WithLocalType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Local);

        await Assert.That(_request.Object.IsNetworkRequest()).IsFalse();
    }

    [Test]
    public async Task IsNetworkRequest_WithLocalAndNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        await Assert.That(_request.Object.IsNetworkRequest()).IsFalse();
    }

    [Test]
    public async Task IsNetworkRequest_WithSyncType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Sync);

        await Assert.That(_request.Object.IsNetworkRequest()).IsFalse();
    }

    #endregion

    #region HasLocalRequest Extension

    [Test]
    public async Task HasLocalRequest_WithLocalType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Local);

        await Assert.That(_request.Object.HasLocalRequest()).IsTrue();
    }

    [Test]
    public async Task HasLocalRequest_WithLocalAndNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        await Assert.That(_request.Object.HasLocalRequest()).IsTrue();
    }

    [Test]
    public async Task HasLocalRequest_WithSyncType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Sync);

        await Assert.That(_request.Object.HasLocalRequest()).IsTrue();
    }

    [Test]
    public async Task HasLocalRequest_WithNetworkType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Network);

        await Assert.That(_request.Object.HasLocalRequest()).IsFalse();
    }

    #endregion

    #region HasNetworkRequest Extension

    [Test]
    public async Task HasNetworkRequest_WithNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Network);

        await Assert.That(_request.Object.HasNetworkRequest()).IsTrue();
    }

    [Test]
    public async Task HasNetworkRequest_WithLocalAndNetworkType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.LocalAndNetwork);

        await Assert.That(_request.Object.HasNetworkRequest()).IsTrue();
    }

    [Test]
    public async Task HasNetworkRequest_WithSyncType_ReturnsTrue()
    {
        _request.RequestType.Returns(RequestType.Sync);

        await Assert.That(_request.Object.HasNetworkRequest()).IsTrue();
    }

    [Test]
    public async Task HasNetworkRequest_WithLocalType_ReturnsFalse()
    {
        _request.RequestType.Returns(RequestType.Local);

        await Assert.That(_request.Object.HasNetworkRequest()).IsFalse();
    }

    #endregion

    #region ErrorResponse Construction

    [Test]
    public async Task ErrorResponse_Constructor_StoresRequest()
    {
        var exception = new InvalidOperationException("test");

        var errorResponse = new ErrorResponse(_request.Object, ErrorType.Unknown, exception);

        await Assert.That(errorResponse.Request).IsSameReferenceAs(_request.Object);
    }

    [Test]
    public async Task ErrorResponse_Constructor_StoresErrorType()
    {
        var exception = new InvalidOperationException("test");

        var errorResponse = new ErrorResponse(_request.Object, ErrorType.ServerError, exception);

        await Assert.That(errorResponse.ErrorType).IsEqualTo(ErrorType.ServerError);
    }

    [Test]
    public async Task ErrorResponse_Constructor_StoresException()
    {
        var exception = new InvalidOperationException("specific error");

        var errorResponse = new ErrorResponse(_request.Object, ErrorType.DBError, exception);

        await Assert.That(errorResponse.Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task ErrorResponse_Constructor_WithAllErrorTypes_StoresCorrectly()
    {
        var exception = new Exception("test");

        foreach (var errorType in Enum.GetValues<ErrorType>())
        {
            var response = new ErrorResponse(_request.Object, errorType, exception);
            await Assert.That(response.ErrorType).IsEqualTo(errorType);
        }
    }

    #endregion

    #region CallbackExtensions.OnError — Cancellation

    [Test]
    public async Task OnError_WithOperationCanceledAndTokenCanceled_CallsOnCanceled()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        _request.CancellationToken.Returns(cts.Token);
        var exception = new OperationCanceledException(cts.Token);

        _callback.Object.OnError(_request.Object, exception);

        _callback.OnCanceled(Any<IUsecaseResponse>()).WasCalled(Times.Once);
        _callback.OnError(Any<ErrorResponse>()).WasNeverCalled();
    }

    #endregion

    #region CallbackExtensions.OnError — Timeout

    [Test]
    public async Task OnError_WithOperationCanceledButTokenNotCanceled_CallsOnErrorWithTimedOut()
    {
        _request.CancellationToken.Returns(CancellationToken.None);
        var exception = new OperationCanceledException();

        _callback.Object.OnError(_request.Object, exception);

        _callback.OnError(Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.TimedOut &&
            e.Request == _request.Object &&
            e.Exception == exception)).WasCalled(Times.Once);
        _callback.OnCanceled(Any<IUsecaseResponse>()).WasNeverCalled();
    }

    #endregion

    #region CallbackExtensions.OnError — Unknown Exception

    [Test]
    public async Task OnError_WithGenericException_CallsOnErrorWithUnknown()
    {
        _request.CancellationToken.Returns(CancellationToken.None);
        var exception = new InvalidOperationException("something broke");

        _callback.Object.OnError(_request.Object, exception);

        _callback.OnError(Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.Unknown &&
            e.Request == _request.Object &&
            e.Exception == exception)).WasCalled(Times.Once);
        _callback.OnCanceled(Any<IUsecaseResponse>()).WasNeverCalled();
    }

    [Test]
    public async Task OnError_WithArgumentException_CallsOnErrorWithUnknown()
    {
        _request.CancellationToken.Returns(CancellationToken.None);
        var exception = new ArgumentException("bad arg");

        _callback.Object.OnError(_request.Object, exception);

        _callback.OnError(Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.Unknown)).WasCalled(Times.Once);
    }

    #endregion

    #region CallbackExtensions.OnError — Null Callback

    [Test]
    public async Task OnError_WithNullCallback_DoesNotThrow()
    {
        ICallback<IUsecaseResponse>? nullCallback = null;
        var exception = new Exception("test");

        var act = () => nullCallback!.OnError(_request.Object, exception);

        await Assert.That(act).ThrowsNothing();
    }

    #endregion

    #region UsecaseBase — Execute with Cache Hit

    [Test]
    public async Task Execute_WithLocalRequestAndCacheHit_ReturnsEarlyWithoutCallingActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new TestUsecase(request, _callback.Object, tryServeFromCacheResult: true, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        });

        usecase.Execute();

        // Give a short window for Task.Run to potentially execute if it was erroneously called
        var completed = await Task.WhenAny(tcs.Task, Task.Delay(200));
        await Assert.That(ReferenceEquals(completed, tcs.Task)).IsFalse();
    }

    #endregion

    #region UsecaseBase — Execute with Cache Miss

    [Test]
    public async Task Execute_WithLocalRequestAndCacheMiss_CallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new TestUsecase(request, _callback.Object, tryServeFromCacheResult: false, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        });

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(actionCalled).IsTrue();
    }

    #endregion

    #region UsecaseBase — Execute with Non-Local Request

    [Test]
    public async Task Execute_WithNetworkRequest_SkipsCacheAndCallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");
        var cacheChecked = false;
        var usecase = new TestUsecase(request, _callback.Object, tryServeFromCacheResult: false, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        }, onTryServeFromCache: () => cacheChecked = true);

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(actionCalled).IsTrue();
        await Assert.That(cacheChecked).IsFalse();
    }

    #endregion

    #region UsecaseBase — Execute with Cache Exception

    [Test]
    public async Task Execute_WhenTryServeFromCacheThrows_SwallowsAndCallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new TestUsecase(request, _callback.Object, tryServeFromCacheResult: false, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        }, throwOnCache: true);

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(actionCalled).IsTrue();
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
        _callback.OnCanceled(Any<IUsecaseResponse>())
            .Callback(() => callbackCalled.TrySetResult(true));
        var usecase = new TestUsecase(request, _callback.Object, tryServeFromCacheResult: false, actionAsync: () =>
        {
            actionCalled = true;
            return Task.CompletedTask;
        });

        usecase.Execute();

        await callbackCalled.Task.WaitAsync(TimeSpan.FromSeconds(5));
        _callback.OnCanceled(Any<IUsecaseResponse>()).WasCalled(Times.Once);
        await Assert.That(actionCalled).IsFalse();
    }

    #endregion

    #region UsecaseBase — Execute with ActionAsync Exception

    [Test]
    public async Task Execute_WhenActionAsyncThrows_CallsCallbackOnError()
    {
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");
        var exception = new InvalidOperationException("action failed");
        var errorCalled = new TaskCompletionSource<bool>();
        _callback.OnError(Any<ErrorResponse>())
            .Callback(() => errorCalled.TrySetResult(true));
        var usecase = new TestUsecase(request, _callback.Object, tryServeFromCacheResult: false, actionAsync: () =>
        {
            throw exception;
        });

        usecase.Execute();

        await errorCalled.Task.WaitAsync(TimeSpan.FromSeconds(5));
        _callback.OnError(Is<ErrorResponse>(e =>
            e.ErrorType == ErrorType.Unknown &&
            e.Exception == exception)).WasCalled(Times.Once);
    }

    #endregion

    #region UsecaseBase — Execute with Null Callback

    [Test]
    public async Task Execute_WithNullCallbackAndCancellation_DoesNotThrow()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var request = new TestUsecaseRequest(RequestType.Network, "user-1", cts.Token);
        var usecase = new TestUsecase(request, null!, tryServeFromCacheResult: false, actionAsync: () => Task.CompletedTask);

        var act = () => usecase.Execute();

        await Assert.That(act).ThrowsNothing();
    }

    [Test]
    public async Task Execute_WithNullCallbackAndActionException_DoesNotThrow()
    {
        var request = new TestUsecaseRequest(RequestType.Network, "user-1");
        var usecase = new TestUsecase(request, null!, tryServeFromCacheResult: false, actionAsync: () =>
        {
            throw new InvalidOperationException("boom");
        });

        var act = () => usecase.Execute();

        await Assert.That(act).ThrowsNothing();
    }

    #endregion

    #region UsecaseBase — TryServeFromCache Default

    [Test]
    public async Task TryServeFromCache_DefaultImplementation_ReturnsFalseAndCallsActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new TestUsecaseRequest(RequestType.Local, "user-1");
        var usecase = new DefaultCacheUsecase(request, _callback.Object, actionAsync: () =>
        {
            tcs.SetResult(true);
            return Task.CompletedTask;
        });

        usecase.Execute();

        var actionCalled = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(actionCalled).IsTrue();
    }

    #endregion

    #region IUsecaseResponse Interface

    [Test]
    public async Task IUsecaseResponse_MockedInstance_ExposesRequestAndResponseType()
    {
        var response = Mock.Of<IUsecaseResponse>();
        response.Request.Returns(_request.Object);
        response.ResponseType.Returns(ResponseType.Network);

        await Assert.That(response.Object.Request).IsSameReferenceAs(_request.Object);
        await Assert.That(response.Object.ResponseType).IsEqualTo(ResponseType.Network);
    }

    #endregion

    #region ICallback Interface

    [Test]
    public async Task ICallback_OnSuccess_CanBeInvoked()
    {
        var response = Mock.Of<IUsecaseResponse>();

        _callback.Object.OnSuccess(response.Object);

        _callback.OnSuccess(Any<IUsecaseResponse>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task ICallback_OnFailed_CanBeInvoked()
    {
        var response = Mock.Of<IUsecaseResponse>();

        _callback.Object.OnFailed(response.Object);

        _callback.OnFailed(Any<IUsecaseResponse>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task ICallback_OnProgress_CanBeInvoked()
    {
        var response = Mock.Of<IUsecaseResponse>();

        _callback.Object.OnProgress(response.Object);

        _callback.OnProgress(Any<IUsecaseResponse>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task ICallback_OnCanceled_CanBeInvoked()
    {
        var response = Mock.Of<IUsecaseResponse>();

        _callback.Object.OnCanceled(response.Object);

        _callback.OnCanceled(Any<IUsecaseResponse>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task ICallback_OnError_CanBeInvoked()
    {
        var errorResponse = new ErrorResponse(_request.Object, ErrorType.Unknown, new Exception());

        _callback.Object.OnError(errorResponse);

        _callback.OnError(Any<ErrorResponse>()).WasCalled(Times.Once);
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

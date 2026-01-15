namespace Cyclotron.Utilities.CleanArchitecture;

public interface ICallback<R> where R : IUsecaseResponse
{
    void OnSuccess(R response);

    void OnFailed(R response);

    void OnError(ErrorResponse error);

    void OnProgress(R response);

    void OnCanceled(R response);
}
using System.Threading;

public static class CancellationTokenSourceExtensions
{
    public static CancellationToken SafeToken(this CancellationTokenSource cts) =>
        cts?.Token ?? CancellationToken.None;

    public static bool IsNullOrCanceled(this CancellationTokenSource cts) => 
        cts == null || cts.Token.IsCancellationRequested;

    public static void CancelAndDispose(this CancellationTokenSource cts)
    {
        cts.Cancel();
        cts.Dispose();
    }
};

// Requires Cysharp.Threading.Tasks

using System;
using System.Threading;

using Cysharp.Threading.Tasks;

namespace UIProgrammerTest.Model
{
    public static class GameModelAwaiterExtensions
    {
        public static async UniTask<OperationResultDto> WaitAsync(
            this IGameModel model,
            Guid operationId,
            CancellationToken cancellationToken = default,
            int timeoutMs = -1)
        {
            var tcs = new UniTaskCompletionSource<OperationResultDto>();

            void Handler(OperationResultDto r)
            {
                if (r.Guid == operationId)
                    tcs.TrySetResult(r);
            }

            model.OperationComplete += Handler;
            try
            {
                if (timeoutMs <= 0)
                {
                    return await tcs.Task.AttachExternalCancellation(cancellationToken);
                }

                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(timeoutMs);
                return await tcs.Task.AttachExternalCancellation(cts.Token);

            }
            finally
            {
                model.OperationComplete -= Handler;
            }
        }
    }
}

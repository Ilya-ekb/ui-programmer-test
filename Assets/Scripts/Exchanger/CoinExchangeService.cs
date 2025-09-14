using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UIProgrammerTest.Model;

namespace UIProgrammerTest.Exchanger
{
    public sealed class CoinExchanger : ICoinExchanger
    {
        public UniTask<OperationResultDto> ExchangeAsync(IGameModel model, int coinCount, CancellationToken ct = default, int timeoutMs = -1)
        {
            if (coinCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(coinCount), "Amount must be > 0");
            }

            var opId = model.ConvertCoinsToCredits(coinCount);
            return model.WaitAsync(opId, ct, timeoutMs);
        }
    }
}

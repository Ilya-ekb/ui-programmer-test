using System.Threading;
using Cysharp.Threading.Tasks;
using UIProgrammerTest.Model;

namespace UIProgrammerTest.Exchanger
{
    public interface ICoinExchanger
    {
        UniTask<OperationResultDto> ExchangeAsync(IGameModel model, int count, CancellationToken ct = default, int timeoutMs = -1);
    }
}

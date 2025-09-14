using System.Threading;
using Cysharp.Threading.Tasks;
using UIProgrammerTest.Consumable;
using UIProgrammerTest.Model;

namespace UIProgrammerTest.Purchase
{
    public interface IPurchaseActor
    {
        UniTask<OperationResultDto> PurchaseAsync(IGameModel model, IConsumableVm vm, CancellationToken ct = default, int timeoutMs = -1);
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UIProgrammerTest.Consumable;
using UIProgrammerTest.Model;

namespace UIProgrammerTest.Purchase
{
    public class GameModelPurchaseActor : IPurchaseActor
    {
        public async UniTask<OperationResultDto> PurchaseAsync(IGameModel gm, IConsumableVm vm,
            CancellationToken ct = default, int timeoutMs = -1)
        {
            var opId = StartPurchase(gm, vm);
            return await gm.WaitAsync(opId, ct, timeoutMs);
        }

        private static Guid StartPurchase(IGameModel gm, IConsumableVm vm)
        {
            if (vm is null)
            {
                throw new ArgumentNullException(nameof(vm));
            }

            if (vm.CoinPrice > 0)
            {
                return gm.BuyConsumableForCoin(vm.Id);
            }

            if (vm.CreditPrice > 0)
            {
                return gm.BuyConsumableForCredit(vm.Id);
            }

            throw new InvalidOperationException("No valid price for this consumable.");
        }
    }
}

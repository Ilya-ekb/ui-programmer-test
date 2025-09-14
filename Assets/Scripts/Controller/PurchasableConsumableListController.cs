using System;
using System.Threading;
using System.Threading.Tasks;
using UIProgrammerTest.Consumable;
using UIProgrammerTest.Purchase;

namespace UIProgrammerTest.Controller
{

    public class PurchasableConsumableListController : BaseConsumableListController, IMessageProvider
    {
        public event Action<bool, string> OnMessage;
        public Action<IConsumableVm, bool, string> OnPurchaseResult { get; set; }

        private IPurchaseActor purchaseActor;

        private CancellationTokenSource cts;

        private const int timeoutMs = 7000;

        protected override void OnEnable()
        {
            base.OnEnable();
            purchaseActor ??= new GameModelPurchaseActor();
            cts = new CancellationTokenSource();
            if (view)
            {
                view.OnBuyClicked += OnBuy;
            }

            model.ModelChanged += OnModelChanged;
        }

        protected override void OnDisable()
        {
            if (view)
            {
                view.OnBuyClicked -= OnBuy;
            }

            model.ModelChanged -= OnModelChanged;

            cts?.Cancel();
            cts?.Dispose();
            cts = null;

            base.OnDisable();
        }

        protected override void Refresh()
        {
            if (!view)
            {
                return;
            }

            var vms = provider.GetAll();

            var blockAll = model.HasRunningOperations;
            view.Render(vms, Loc, model.CoinBalance, model.CreditBalance);
            view.UpdateInteractability(!blockAll);
        }

        private void OnModelChanged() => UpdateInteractability();

        private void UpdateInteractability()
        {
            if (view == null)
            {
                return;
            }

            var blockAll = model.HasRunningOperations;
            view.UpdateInteractability(!blockAll);
            var vms = provider.GetAll();
            for (var i = 0; i < vms.Count; i++)
            {
                var canBuy = vms[i].CanBuy(model.CoinBalance, model.CreditBalance);
                view.ConsumableViews[i].UpdateInteractability(canBuy);
            }
        }

        private async void OnBuy(IConsumableVm vm)
        {
            view.UpdateInteractability(false);
            foreach (var consumableView in view.ConsumableViews)
            {
                consumableView.UpdateInteractability(false);
            }

            var isSuccess = false;
            var message = string.Empty;
            try
            {
                var result = await purchaseActor.PurchaseAsync(model, vm, cts.Token, timeoutMs);
                message = result.Success ? null : result.Message ?? "Purchase failed";
                isSuccess = result.Success;
            }
            catch (Exception e)
            {
                isSuccess = false;
                message = e.Message;
            }
            finally
            {
                UpdateInteractability();
            }
            OnPurchaseResult?.Invoke(vm, isSuccess, message);
            OnMessage?.Invoke(isSuccess, message);
        }
    }
}

using System;
using System.Threading;
using UIProgrammerTest.Currency;
using UIProgrammerTest.Exchanger;
using UIProgrammerTest.View;
using UnityEngine;

namespace UIProgrammerTest.Controller
{
    public class ExchangeCurrencyController : BaseCurrencyController, IMessageProvider
    {
        public event Action<bool, string> OnMessage;
        public Action<ICurrencyVm, bool, string> OnResultExchangeCurrency { get; set; }

        private ExchangeCurrencyView exchangeCurrencyView { get; set; }

        private ICoinExchanger exchangeService;
        private CancellationTokenSource cts;

        private int exchangeCoinCount;
        private const int timeoutMs = 7000;

        protected override void Awake()
        {
            base.Awake();
            exchangeService = new CoinExchanger();
            exchangeCurrencyView = view as ExchangeCurrencyView;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            cts = new CancellationTokenSource();
            if (!exchangeCurrencyView)
            {
                return;
            }

            exchangeCurrencyView.onCoinsChanged?.AddListener(OnCoinsChanged);
            exchangeCurrencyView.onExchangeButtonPressed?.AddListener(OnExchange);
            UpdateInteractability();
        }

        private void OnCoinsChanged(string arg0)
        {
            exchangeCoinCount = int.TryParse(arg0, out exchangeCoinCount) ? exchangeCoinCount : 0;
            UpdateInteractability();
        }

        protected override void UpdateInteractability()
        {
            base.UpdateInteractability();
            exchangeCurrencyView.SetResult(exchangeCoinCount * currency.Current.Rate);
            exchangeCurrencyView.SetExchangeButtonInteractable(currency.Current.CanExchange(exchangeCoinCount));

        }

        protected override void OnDisable()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;

            if (exchangeCurrencyView)
            {
                exchangeCurrencyView.onCoinsChanged?.RemoveListener(OnCoinsChanged);
                exchangeCurrencyView.onExchangeButtonPressed?.RemoveListener(OnExchange);
            }

            base.OnDisable();
        }

        private async void OnExchange()
        {
            view.UpdateInteractability(false);
            exchangeCurrencyView.SetExchangeButtonInteractable(false);
            string mes;
            bool isSuccess;
            try
            {
                var res = await exchangeService.ExchangeAsync(model, exchangeCoinCount, cts.Token, timeoutMs);
                if (res.Success)
                {
                    view.Bind(currency.Current, model.HasRunningOperations);
                }

                isSuccess = res.Success;
                mes = res.Success ? null : res.Message ?? "Exchange failed";
            }
            catch (Exception e)
            {
                isSuccess = false;
                mes = e.Message;
            }
            finally
            {
                UpdateInteractability();
            }

            OnResultExchangeCurrency?.Invoke(currency.Current, isSuccess, mes);
            OnMessage?.Invoke(isSuccess, mes);
        }
    }
}

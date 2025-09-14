using System;
using UIProgrammerTest.Model;

namespace UIProgrammerTest.Currency
{
    public sealed class GameModelCurrencyProvider : ICurrencyProvider
    {
        public event Action Changed;
        public ICurrencyVm Current { get; private set; }

        private readonly IGameModel model;

        public GameModelCurrencyProvider(IGameModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            Rebuild();
            this.model.ModelChanged += OnModelChanged;
        }

        private void OnModelChanged()
        {
            Rebuild();
            Changed?.Invoke();
        }

        private void Rebuild()
        {
            Current = new CurrencyVm(
                model.CoinBalance,
                model.CreditBalance,
                model.ConversionRate
            );
        }
    }
}

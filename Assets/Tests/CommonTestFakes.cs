using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UIProgrammerTest.Consumable;
using UIProgrammerTest.Model;
using UIProgrammerTest.Exchanger;
using UIProgrammerTest.Purchase;
using UnityEngine;


namespace Tests.Common
{
    public sealed class FakeGameModel : IGameModel
    {
        public event Action ModelChanged;
        public event Action<OperationResultDto> OperationComplete;

        public int CoinBalance { get; private set; } = 100;
        public int CreditBalance { get; private set; } = 10;
        public int ConversionRate { get; private set; } = 10;
        public bool HasRunningOperations { get; private set; }

        public FakeGameModel()
        {
        }

        public FakeGameModel(int coinBalance, int creditBalance, int conversionRate)
        {
            CoinBalance = coinBalance;
            CreditBalance = creditBalance;
            ConversionRate = conversionRate;
        }

        private readonly Dictionary<ConsumableTypeId, ConsumableConfigDto> prices = new()
        {
            { new ConsumableTypeId(1), new ConsumableConfigDto(coinPrice: 5, creditPrice: 2) },
            { new ConsumableTypeId(2), new ConsumableConfigDto(coinPrice: 0, creditPrice: 7) },
        };

        private readonly Dictionary<ConsumableTypeId, int> counts = new();

        public IReadOnlyDictionary<ConsumableTypeId, ConsumableConfigDto> ConsumablesPrice => prices;

        public int GetConsumableCount(ConsumableTypeId id) => counts.GetValueOrDefault(id, 0);

        public Guid BuyConsumableForCoin(ConsumableTypeId type)
        {
            if (!prices.TryGetValue(type, out var cfg) || cfg.CoinPrice <= 0)
                throw new InvalidOperationException("Coin purchase not available");
            var id = Guid.NewGuid();
            HasRunningOperations = true;
            // simulate async completion in Update (below) by subtracting immediately for tests:
            CoinBalance -= cfg.CoinPrice;
            counts[type] = GetConsumableCount(type) + 1;
            OperationComplete?.Invoke(new OperationResultDto(id, true, null));
            HasRunningOperations = false;
            ModelChanged?.Invoke();
            return id;
        }

        public Guid BuyConsumableForCredit(ConsumableTypeId type)
        {
            if (!prices.TryGetValue(type, out var cfg) || cfg.CreditPrice <= 0)
                throw new InvalidOperationException("Credit purchase not available");
            var id = Guid.NewGuid();
            HasRunningOperations = true;
            CreditBalance -= cfg.CreditPrice;
            counts[type] = GetConsumableCount(type) + 1;
            OperationComplete?.Invoke(new OperationResultDto(id, true, null));
            HasRunningOperations = false;
            ModelChanged?.Invoke();
            return id;
        }

        public Guid ConvertCoinsToCredits(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(coins));
            }

            var id = Guid.NewGuid();
            HasRunningOperations = true;
            CoinBalance -= coins;
            CreditBalance += coins / Math.Max(1, ConversionRate);
            OperationComplete?.Invoke(new OperationResultDto(id, true, null));
            HasRunningOperations = false;
            ModelChanged?.Invoke();
            return id;
        }

        public void Update()
        {
            /* no-op in fake */
        }
    }

    public sealed class FakeCoinExchanger : ICoinExchanger
    {
        private readonly IGameModel _model;
        private readonly int _delayMs;
        private readonly bool _fail;

        public FakeCoinExchanger(IGameModel model, int delayMs = 10, bool fail = false)
        {
            _model = model;
            _delayMs = delayMs;
            _fail = fail;
        }

        public async UniTask<OperationResultDto> ExchangeAsync(IGameModel model, int count,
            CancellationToken ct = default,
            int timeoutMs = -1)
        {
            await UniTask.Delay(_delayMs, cancellationToken: ct);
            if (_fail) return new OperationResultDto(Guid.NewGuid(), false, "fail");
            var id = _model.ConvertCoinsToCredits(count);
            return new OperationResultDto(id, true, null);
        }
    }

    public sealed class FakePurchaseActor : IPurchaseActor
    {
        public async UniTask<OperationResultDto> PurchaseAsync(IGameModel model, IConsumableVm vm,
            CancellationToken ct = default, int timeoutMs = -1)
        {
            await UniTask.Yield();
            Guid id = vm.CoinPrice > 0 ? model.BuyConsumableForCoin(vm.Id) : model.BuyConsumableForCredit(vm.Id);
            return new OperationResultDto(id, true, null);
        }
    }

    public sealed class FakeVm : IConsumableVm
    {
        public ConsumableTypeId Id { get; set; }
        public int Count { get; set; }
        public int CoinPrice { get; set; }
        public int CreditPrice { get; set; }
        public string NameKey { get; set; }
        public string DescriptionKey { get; set; }
        public Sprite Icon { get; set; }
    }
}

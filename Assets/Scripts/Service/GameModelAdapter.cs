using System;
using System.Collections.Generic;

using UIProgrammerTest.Model;

namespace UIProgrammerTest.Service
{
    public sealed class GameModelAdapter : IGameModel
    {

        public event Action ModelChanged;
        public event Action<OperationResultDto> OperationComplete;

        private readonly Dictionary<ConsumableTypeId, ConsumableConfigDto> priceCache = new();

        public GameModelAdapter()
        {
            RebuildPriceCache();

            GameModel.ModelChanged += () => ModelChanged?.Invoke();
            GameModel.OperationComplete += OnOperationCompleteFromModel;
        }

        private void RebuildPriceCache()
        {
            priceCache.Clear();
            foreach (var kv in GameModel.ConsumablesPrice)
            {
                var id = new ConsumableTypeId((int)kv.Key);
                var cfg = new ConsumableConfigDto(kv.Value.CoinPrice, kv.Value.CreditPrice);
                priceCache[id] = cfg;
            }
        }

        public int CoinBalance => GameModel.CoinCount;
        public int CreditBalance => GameModel.CreditCount;
        public int ConversionRate => GameModel.CoinToCreditRate;
        public bool HasRunningOperations => GameModel.HasRunningOperations;

        public IReadOnlyDictionary<ConsumableTypeId, ConsumableConfigDto> ConsumablesPrice => priceCache;

        public int GetConsumableCount(ConsumableTypeId id)
        {
            return GameModel.GetConsumableCount((GameModel.ConsumableTypes) id.value);
        }

        public Guid BuyConsumableForCoin(ConsumableTypeId type)
        {
            var modelType = (GameModel.ConsumableTypes)type.value;
            return GameModel.BuyConsumableForGold(modelType);
        }

        public Guid BuyConsumableForCredit(ConsumableTypeId type)
        {
            var modelType = (GameModel.ConsumableTypes)type.value;
            return GameModel.BuyConsumableForSilver(modelType);
        }

        public Guid ConvertCoinsToCredits(int coins) => GameModel.ConvertCoinToCredit(coins);

        public void Update()
        {
            GameModel.Update();
        }

        private void OnOperationCompleteFromModel(GameModel.OperationResult r)
        {
            var dto = new OperationResultDto(r.Guid, r.IsSuccess, r.ErrorDescription);
            OperationComplete?.Invoke(dto);
        }
    }
}

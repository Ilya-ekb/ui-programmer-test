using System;
using System.Collections.Generic;

namespace UIProgrammerTest.Model
{

    public interface IGameModel
    {
        event Action ModelChanged;
        event Action<OperationResultDto> OperationComplete;

        int CoinBalance { get; }
        int CreditBalance { get; }
        int ConversionRate { get; }
        bool HasRunningOperations { get; }

        IReadOnlyDictionary<ConsumableTypeId, ConsumableConfigDto> ConsumablesPrice { get; }
        int GetConsumableCount(ConsumableTypeId id);
        Guid BuyConsumableForCoin(ConsumableTypeId type);
        Guid BuyConsumableForCredit(ConsumableTypeId type);
        Guid ConvertCoinsToCredits(int coins);

        void Update();

    }
}

using UIProgrammerTest.Model;

namespace UIProgrammerTest.Consumable
{
    public interface IConsumableVm
    {
        ConsumableTypeId Id { get; }
        int Count { get; }
        int CoinPrice { get; }
        int CreditPrice { get; }
        string NameKey { get; }
        string DescriptionKey { get; }
        UnityEngine.Sprite Icon { get; }
    }
}

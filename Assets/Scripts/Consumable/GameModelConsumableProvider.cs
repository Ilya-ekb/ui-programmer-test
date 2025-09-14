using System.Collections.Generic;
using UIProgrammerTest.Model;
using UnityEngine;

namespace UIProgrammerTest.Consumable
{
    public sealed class GameModelConsumableProvider : IConsumableProvider
    {
        private readonly IGameModel model;
        private readonly IConsumableDatabase db;
        private readonly List<IConsumableVm> cache = new();

        public event System.Action Changed;

        public GameModelConsumableProvider(IGameModel model, IConsumableDatabase db)
        {
            this.model = model;
            this.db = db;
            Rebuild();
            this.model.ModelChanged += OnModelChanged;
        }

        public IReadOnlyList<IConsumableVm> GetAll() => cache;

        private void OnModelChanged()
        {
            Rebuild();
            Changed?.Invoke();
        }

        private void Rebuild()
        {
            cache.Clear();
            foreach (var (id, price) in model.ConsumablesPrice)
            {
                if (db is not null && db.TryGet(id, out var meta))
                {
                    cache.Add(new Vm(
                        id,
                        model.GetConsumableCount(id),
                        price.CoinPrice,
                        price.CreditPrice,
                        meta?.nameKey ?? $"consumable.{id}.name",
                        meta?.descriptionKey ?? $"consumable.{id}.desc",
                        meta?.icon
                    ));
                }
            }
        }

        private sealed class Vm : IConsumableVm
        {
            public ConsumableTypeId Id { get; }
            public int Count { get; }
            public int CoinPrice { get; }
            public int CreditPrice { get; }
            public string NameKey { get; }
            public string DescriptionKey { get; }
            public Sprite Icon { get; }

            public Vm(ConsumableTypeId id, int count, int coin, int credit, string nameKey, string descKey, Sprite icon)
            {
                Id = id;
                Count = count;
                CoinPrice = coin;
                CreditPrice = credit;
                NameKey = nameKey;
                DescriptionKey = descKey;
                Icon = icon;
            }
        }
    }
}

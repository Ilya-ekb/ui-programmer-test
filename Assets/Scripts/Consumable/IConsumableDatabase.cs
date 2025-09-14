using TriInspector;
using UIProgrammerTest.Model;
using UnityEngine;

namespace UIProgrammerTest.Consumable
{
    public interface IConsumableDatabase
    {
        bool TryGet(ConsumableTypeId id, out ConsumableMeta meta);
    }

    [System.Serializable]
    public class ConsumableMeta
    {
        [ReadOnly] public string nameKey;
        [ReadOnly] public string descriptionKey;
        public Sprite icon;
    }
}

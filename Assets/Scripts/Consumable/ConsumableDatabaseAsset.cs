using System;
using System.Collections.Generic;
using System.Linq;

using TriInspector;

using UIProgrammerTest.Model;

using UnityEngine;

namespace UIProgrammerTest.Consumable
{
    [CreateAssetMenu(menuName = "Game/Consumables/Database")]
    public class ConsumableDatabaseAsset : ScriptableObject, IConsumableDatabase
    {
        [Serializable]
        public class Entry
        {
            [Dropdown(nameof(GetID))] public ConsumableTypeId id;
            public ConsumableMeta meta;

            public void Validate()
            {
#if UNITY_EDITOR
                if (meta is not null)
                {
                    meta.nameKey = $"consumable.{id.ToString().ToLower()}.name";
                    meta.descriptionKey = $"consumable.{id.ToString().ToLower()}.description";
                    return;
                }

                meta = new ConsumableMeta
                {
                    nameKey = $"consumable.{id.ToString().ToLower()}.name",
                    descriptionKey = $"consumable.{id.ToString().ToLower()}.desc",
                };
#endif
            }

            protected virtual IEnumerable<TriDropdownItem<ConsumableTypeId>> GetID()
            {
                var list = new TriDropdownList<ConsumableTypeId>();
#if UNITY_EDITOR
                var types = Enum.GetValues(typeof(GameModel.ConsumableTypes)).Cast<GameModel.ConsumableTypes>();
                foreach (var type in types)
                {
                    list.Add(type.ToString(), new ConsumableTypeId((int)type, type.ToString()));
                }
#endif
                return list;
            }
        }

        [SerializeField] private List<Entry> entries = new();

        private Dictionary<ConsumableTypeId, ConsumableMeta> map;

        private void OnEnable()
        {
            Rebuild();
        }

        public void Rebuild()
        {
            map = new Dictionary<ConsumableTypeId, ConsumableMeta>();
            foreach (var e in entries)
            {
                if (!map.ContainsKey(e.id))
                {
                    map.Add(e.id, e.meta);
                }
            }
        }

        public bool TryGet(ConsumableTypeId id, out ConsumableMeta meta)
        {
            if (map is null)
            {
                Rebuild();
            }

            return map.TryGetValue(id, out meta);
        }

        private void OnValidate()
        {
            foreach (var entry in entries)
            {
                entry.Validate();
            }
        }
    }
}

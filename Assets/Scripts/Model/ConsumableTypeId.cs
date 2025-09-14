using System;

namespace UIProgrammerTest.Model
{
    [Serializable]
    public struct ConsumableTypeId : IEquatable<ConsumableTypeId>
    {
        public int value;
        public string desc;

        public ConsumableTypeId(int value, string desc = null)
        {
            this.desc = desc;
            this.value = value;
        }

        public bool Equals(ConsumableTypeId other) => value == other.value;
        public override bool Equals(object obj) => obj is ConsumableTypeId o && Equals(o);
        public override int GetHashCode() => value;
        public override string ToString() => string.IsNullOrEmpty(desc) ? value.ToString() : desc;
        public static implicit operator int(ConsumableTypeId id) => id.value;
        public static explicit operator ConsumableTypeId(int v) => new(v);
    }
}

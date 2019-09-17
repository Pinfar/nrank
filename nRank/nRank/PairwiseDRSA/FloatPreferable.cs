using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class FloatPreferable : IPreferable
    {
        public FloatPreferable(float value, AttributeType type)
        {
            Value = value;
            Type = type;
        }

        public float Value { get; }

        public AttributeType Type { get; }

        public bool IsWeaklyPreferedTo(IPreferable other)
        {
            var otherSameType = other as FloatPreferable;
            if (otherSameType == null) throw new InvalidOperationException($"{other.GetType()} can not be cast to FloatPreferable!");
            if (otherSameType.Type != Type) throw new InvalidOperationException($"Attribute type does not match!");
            if (Type == AttributeType.Cost)
            {
                return Value <= otherSameType.Value;
            }
            else
            {
                return Value >= otherSameType.Value;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

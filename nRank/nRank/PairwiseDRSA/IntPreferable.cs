using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class IntPreferable : IPreferable
    {
        public IntPreferable(int value, AttributeType type)
        {
            Value = value;
            Type = type;
        }

        public int Value { get; }

        public AttributeType Type { get; }

        public bool IsWeaklyPreferedTo(IPreferable other)
        {
            var otherSameType = other as IntPreferable;
            if (otherSameType == null) throw new InvalidOperationException($"{other.GetType()} can not be cast to IntPreferable!");
            if (otherSameType.Type != Type) throw new InvalidOperationException($"Attribute type does not match!");
            if(Type == AttributeType.Cost)
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

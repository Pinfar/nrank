using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class FloatValue : INominalValue
    {
        public FloatValue(float value, AttributeType type)
        {
            Value = value;
            Type = type;
        }

        public float Value { get; }

        public AttributeType Type { get; }

        public IPreferable DifferenceWith(INominalValue value)
        {
            if (value is FloatValue floatVal)
            {
                if (Type != floatVal.Type) throw new InvalidOperationException($"Attribute type does not match!");
                return new FloatPreferable(Value - floatVal.Value, Type);
            }
            else
            {
                throw new InvalidOperationException($"Cannot cast {value.GetType()} to FloatValue!");
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

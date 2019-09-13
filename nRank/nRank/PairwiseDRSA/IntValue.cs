using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class IntValue : INominalValue
    {
        public IntValue(int value, AttributeType type)
        {
            Value = value;
            Type = type;
        }

        public int Value { get; }

        public AttributeType Type { get; }

        public IPreferable DifferenceWith(INominalValue value)
        {
            if (value is IntValue intVal)
            {
                if(Type != intVal.Type) throw new InvalidOperationException($"Attribute type does not match!");
                return new IntPreferable(Value - intVal.Value, Type);
            }
            else
            {
                throw new InvalidOperationException($"Cannot cast {value.GetType()} to IntValue!");
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

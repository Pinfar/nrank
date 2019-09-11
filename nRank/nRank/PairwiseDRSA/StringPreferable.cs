using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class StringPreferable : IPreferable
    {
        public string Value { get; }
        public int Index { get; }
        private List<string> _preferenceOrder;

        public StringPreferable(string value, List<string> preferenceOrder)
        {
            if (!preferenceOrder.Contains(value)) throw new InvalidOperationException($"There is no {value} in preferenceOrder list!");

            Value = value;
            _preferenceOrder = preferenceOrder;
            Index = preferenceOrder.IndexOf(value);
        }

        public bool IsWeaklyPreferedTo(IPreferable other)
        {
            if(other is StringPreferable preferable)
            {
                bool areOrdersEqual = preferable._preferenceOrder.Zip(_preferenceOrder, (x, y) => x == y).All(x => x);
                if (!areOrdersEqual) throw new InvalidOperationException("Preference orders are not equal!");
                return Index >= preferable.Index;
            }
            else
            {
                throw new InvalidOperationException($"Cannot cast {other.GetType()} to StringPreferable");
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

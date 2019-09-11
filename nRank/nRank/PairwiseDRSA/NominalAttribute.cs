using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class NominalAttribute : IAttribute
    {
        public NominalAttribute(string label, INominalValue value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; }

        public string StringValue => Value.ToString();

        public INominalValue Value { get; }

        public IPreferable DifferenceWith(NominalAttribute attribute)
        {
            return Value.DifferenceWith(attribute.Value);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class OrdinalAttribute : IAttribute
    {
        public string Label { get; }

        public string StringValue => _internalPreferable.Value;

        private StringPreferable _internalPreferable;

        public OrdinalAttribute(string label, string value, List<string> preferenceOrder)
        {
            Label = label;
            _internalPreferable = new StringPreferable(value, preferenceOrder);
        }

        public bool IsWeaklyPreferedTo(OrdinalAttribute attribute)
        {
            return _internalPreferable.IsWeaklyPreferedTo(attribute._internalPreferable);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class OrdinalAttributePair : IAttributePair
    {
        public string Label => _first.Label;

        private OrdinalAttribute _first;
        private OrdinalAttribute _second;

        public OrdinalAttributePair(OrdinalAttribute first, OrdinalAttribute second)
        {
            _first = first;
            _second = second;
        }

        public bool IsWeaklyPreferredTo(IAttributePair other)
        {
            if (!(other is OrdinalAttributePair otherOrdinal))
                throw new InvalidOperationException("Preferrence relation exist between attributes of the same type!");
            var xFirst = otherOrdinal._first;
            var xSecond = otherOrdinal._second;
            return _first.IsWeaklyPreferedTo(xFirst) && xSecond.IsWeaklyPreferedTo(_second);
        }

        public string ToString(PairwiseComparisonTable.RelationType relation)
        {
            var label = _first.Label;
            if(!label.ToLower().StartsWith("evaluations_on_"))
            {
                label = $"Evaluations_on_{label}";
            }
            var labelPart = $"PAIR({label})";
            var valuePart = $"({_first.StringValue},{_second.StringValue})";
            if (relation == PairwiseComparisonTable.RelationType.S)
            {
                return $"{{{labelPart} D {valuePart} }}";
            }
            return $"{{{valuePart} D {labelPart} }}";
        }

        public string GetValueAsString()
        {
            return $"({_first.StringValue}, {_second.StringValue})";
        }
    }
}

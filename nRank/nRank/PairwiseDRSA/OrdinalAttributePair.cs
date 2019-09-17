﻿using System;
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
            var labelPart = $"PAIR(Evaluations_on_{_first.Label})";
            var valuePart = $"({_first.StringValue},{_second.StringValue})";
            if (relation == PairwiseComparisonTable.RelationType.S)
            {
                return $"{{{labelPart} D {valuePart} }}";
            }
            return $"{{{valuePart} D {labelPart} }}";
        }
    }
}

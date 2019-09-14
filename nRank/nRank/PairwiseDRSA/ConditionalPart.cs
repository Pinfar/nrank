using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class ConditionalPart : IConditionalPart
    {
        private readonly AttributePair _attributes;
        private readonly PairwiseComparisonTable.RelationType _relation;

        public ConditionalPart(AttributePair attributePair, PairwiseComparisonTable.RelationType relation)
        {
            _attributes = attributePair;
            _relation = relation;
            //if( _attributes.First is OrdinalAttribute firstOrd && _attributes.Second is OrdinalAttribute secondOrd)
            //{
            //    firstOrd.
            //}
        }

        public bool IsEmpty()
        {
            return false;
        }

        public bool IsTrueFor(InformationObjectPair pair)
        {
            var attribute = pair.GetAttribute(_attributes.Label);
            if(_relation == PairwiseComparisonTable.RelationType.S)
            {
                return attribute.IsWeaklyPreferredTo(_attributes);
            }
            else
            {
                return _attributes.IsWeaklyPreferredTo(attribute);
            }

        }

        public string ToLatexString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return _attributes.ToString(_relation);
        }
    }
}

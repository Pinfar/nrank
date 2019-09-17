using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    public class NominalAttributePair : IAttributePair
    {
        public string Label { get; }

        private IPreferable _preferable;

        public NominalAttributePair(string label, IPreferable preferable)
        {
            Label = label;
            _preferable = preferable;
        }

        public bool IsWeaklyPreferredTo(IAttributePair other)
        {
            if(!(other is NominalAttributePair nominaOther))
                throw new InvalidOperationException("Preferrence relation exist between attributes of the same type!");
            var xPreferable = nominaOther._preferable;
            return _preferable.IsWeaklyPreferedTo(xPreferable);
        }

        public string ToString(PairwiseComparisonTable.RelationType relation)
        {
            var symbol = ">=";
            if ((relation == PairwiseComparisonTable.RelationType.S) == (_preferable.Type == AttributeType.Cost))
            {
                symbol = "<=";
            }
            return $"[DIFF(Evaluations_difference_on_{Label}) {symbol} {_preferable.ToString()} ]";
        }
    }
}

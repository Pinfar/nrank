using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class PApproximation
    {
        public PApproximation(List<InformationObjectPair> approximation, List<InformationObjectPair> positiveRegion, 
            PairwiseComparisonTable originalTable, PairwiseComparisonTable.RelationType relation)
        {
            Approximation = approximation;
            PositiveRegion = positiveRegion;
            OriginalTable = originalTable;
            var approxHashset = new HashSet<InformationObjectPair>(approximation);
            NegativeApproximation = new HashSet<InformationObjectPair>(originalTable.AsInformationObjectPairs().Where(x => !approxHashset.Contains(x)).ToList());
            Relation = relation;
        }

        public PairwiseComparisonTable.RelationType Relation { get; }
        public List<InformationObjectPair> Approximation { get; }
        public List<InformationObjectPair> PositiveRegion { get; }
        public PairwiseComparisonTable OriginalTable { get; }
        public HashSet<InformationObjectPair> NegativeApproximation { get; }
    }
}

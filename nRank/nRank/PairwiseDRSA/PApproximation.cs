using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class PApproximation
    {
        public PApproximation(List<InformationObjectPair> approximation, List<InformationObjectPair> positiveRegion, PairwiseComparisonTable originalTable)
        {
            Approximation = approximation;
            PositiveRegion = positiveRegion;
            OriginalTable = originalTable;
        }

        public List<InformationObjectPair> Approximation { get; }
        public List<InformationObjectPair> PositiveRegion { get; }
        public PairwiseComparisonTable OriginalTable { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class PDominatedSetGenerator : IPSetGenerator
    {
        public List<InformationObjectPair> Generate(PairwiseComparisonTable table, InformationObjectPair pair)
        {
            return table
                .AsInformationObjectPairs()
                .Where(x => pair.Dominates(x))
                .ToList();
        }
    }
}

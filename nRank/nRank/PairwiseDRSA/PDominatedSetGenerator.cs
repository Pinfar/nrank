using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class PDominatedSetGenerator : IPSetGenerator
    {
        public List<InformationObjectPair> Generate(InformationTable table, InformationObjectPair pair)
        {
            return table.Objects
                .SelectMany(x => table.Objects, (x, y) => new InformationObjectPair(x, y))
                .Where(x => x.First.IntIdentifier != x.Second.IntIdentifier)
                .Where(x => pair.Dominates(x))
                .ToList();
        }
    }
}

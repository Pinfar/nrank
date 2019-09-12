using System.Collections.Generic;

namespace nRank.PairwiseDRSA
{
    interface IPSetGenerator
    {
        List<InformationObjectPair> Generate(InformationTable table, InformationObjectPair pair);
    }
}
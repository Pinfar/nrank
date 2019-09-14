﻿using System.Collections.Generic;

namespace nRank.PairwiseDRSA
{
    interface IPSetGenerator
    {
        List<InformationObjectPair> Generate(PairwiseComparisonTable table, InformationObjectPair pair);
    }
}
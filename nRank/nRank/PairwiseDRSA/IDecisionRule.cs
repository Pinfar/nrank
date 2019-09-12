using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    interface IDecisionRule
    {
        float Accuracy { get; }
        
        IDecisionRule And(IDecisionRule decisionRule);
        bool IsEmpty();
        bool SatisfiesConsistencyLevel(float consistencyLevel);
        IDecisionRule CreateOptimizedRule(float consistencyLevel, List<InformationObjectPair> notCoveredYet);
        bool Contains(IDecisionRule rule);
        string ToLatexString();
        Func<PairwiseComparisonTable.PairwiseComparisonTableEntry, bool> AsFunc();
        Func<InformationObjectPair, bool> AsListFilterFunc();
    }
}

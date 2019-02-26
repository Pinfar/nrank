using System.Collections.Generic;

namespace nRank
{
    internal interface IDecisionRuleGenerator
    {
        IEnumerable<IDecisionRule> GenerateMinimalRulesFrom(ILowerApproximation approximation);
    }
}
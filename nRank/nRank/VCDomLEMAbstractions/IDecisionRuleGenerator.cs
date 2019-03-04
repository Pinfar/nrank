using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    internal interface IDecisionRuleGenerator
    {
        IEnumerable<IDecisionRule> GenerateRulesFrom(IUnion approximation);
    }
}
using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    public interface IDecisionRule
    {
        IDecisionRule And(string attribute, string operatorStr, float value);
        IDecisionRule And(IDecisionRule decisionRule);
        Dictionary<string, bool> Satisfy(IInformationTable informationTable);
        bool IsEmpty();
        bool IsCreatingSubsetOf(IInformationTable source, IInformationTable target);
        IDecisionRule CreateOptimizedRule(IInformationTable source, IInformationTable target);
    }
}
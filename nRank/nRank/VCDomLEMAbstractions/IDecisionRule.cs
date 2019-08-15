using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    public interface IDecisionRule
    {
        float Accuracy { get; }

        IDecisionRule And(string attribute, string operatorStr, float value);
        IDecisionRule And(IDecisionRule decisionRule);
        Dictionary<string, bool> Satisfy(IInformationTable informationTable);
        IEnumerable<string> GetSatisfiedObjectsIdentifiers(IInformationTable informationTable);
        bool IsEmpty();
        bool SatisfiesConsistencyLevel(float consistencyLevel);
        IDecisionRule CreateOptimizedRule(float consistencyLevel, IEnumerable<string> notCoveredYet);
        bool Contains(IDecisionRule rule);
        IEnumerable<string> GetCoveredItems();
    }
}
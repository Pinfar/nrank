using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    public interface IDecisionRule
    {
        float Accuracy { get; }
        ISet<int> Classes { get; }

        IDecisionRule And(string attribute, string operatorStr, float value);
        IDecisionRule And(IDecisionRule decisionRule);
        Dictionary<string, bool> Satisfy(IInformationTable informationTable);
        IEnumerable<string> GetSatisfiedObjectsIdentifiers(IInformationTable informationTable);
        bool IsSatisfiedFor(IInformationTable informationTable, string identifier);
        bool IsEmpty();
        bool SatisfiesConsistencyLevel(float consistencyLevel);
        IDecisionRule CreateOptimizedRule(float consistencyLevel, IEnumerable<string> notCoveredYet);
        bool Contains(IDecisionRule rule);
        IEnumerable<string> GetCoveredItems();
    }
}
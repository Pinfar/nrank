using System.Collections.Generic;

namespace nRank.VCDomLEMAbstractions
{
    public interface IInformationTable
    {
        string DecisionAttributeName { get; }

        void AddAttribute(string name, IEnumerable<float> values, bool isCost = true);
        void AddDecisionAttribute(string name, IEnumerable<int> values, bool isCost = true);
        IInformationTable Filter(IEnumerable<bool> pattern);
        IEnumerable<float> GetAttribute(string name);
        IEnumerable<int> GetDecisionAttribute();
        IEnumerable<int> GetDecicionClassesWorstFirst();
    }
}